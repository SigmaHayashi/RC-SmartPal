using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InfoCanvasManager : MonoBehaviour {

	//UIたち
	private Text Information_Text;

	private Text Vehicle_Power_Text;
	private Text Vehicle_Servo_Text;
	private Text Vehicle_State_Text;
	private Text Vehicle_Pos_Text;

	private Text RightArm_Power_Text;
	private Text RightArm_Servo_Text;
	private Text RightArm_State_Text;
	private Text RightArm_Pos_Text;

	private Text LeftArm_Power_Text;
	private Text LeftArm_Servo_Text;
	private Text LeftArm_State_Text;
	private Text LeftArm_Pos_Text;

	//Android Ros Socket Client関連
	[Serializable]
	public class ServiceCall_sp5_control {
		public string op = "call_service";
		public string service = "sp5_control";
		public Req_sp5_control args;

		public ServiceCall_sp5_control(Req_sp5_control args) {
			this.args = args;
		}
	}

	[Serializable]
	public class Req_sp5_control {
		public int unit;
		public int cmd;
		public float[] arg;

		public Req_sp5_control(int unit, int cmd, float[] arg) {
			this.unit = unit;
			this.cmd = cmd;
			this.arg = arg;
		}
	}

	[Serializable]
	public class Res_sp5_control {
		public bool result;
		public string service;
		public string op;
		public ResValue_sp5_control values;
	}

	[Serializable]
	public class ResValue_sp5_control {
		public int result;
		public float[] val;
	}

	public void ServiceCaller_sp5_control(Req_sp5_control srvReq) {
		ServiceCall_sp5_control call = new ServiceCall_sp5_control(srvReq);
		wsc.SendOpMsg(call);
	}

	private AndroidRosSocketClient wsc;
	private string srvRes;
	private Res_sp5_control responce;

	private float time = 0.0f;

	private bool wait_anything = false;
	private bool access_db = false;
	private bool success_access = false;
	private bool abort_access = false;
	private bool wait_VehicleState = false;
	private bool wait_VehiclePos = false;

	private float time_vehicle_state = 0.0f;
	private float time_vehicle_pos = 0.0f;

	// Start is called before the first frame update
	void Start() {
		Information_Text = GameObject.Find("Main System/Info Canvas/Overall Information Text").GetComponent<Text>();

		Vehicle_Power_Text = GameObject.Find("Main System/Info Canvas/Vehicle Power Text").GetComponent<Text>();
		Vehicle_Servo_Text = GameObject.Find("Main System/Info Canvas/Vehicle Servo Text").GetComponent<Text>();
		Vehicle_State_Text = GameObject.Find("Main System/Info Canvas/Vehicle State Text").GetComponent<Text>();
		Vehicle_Pos_Text = GameObject.Find("Main System/Info Canvas/Vehicle Pos Text").GetComponent<Text>();

		RightArm_Power_Text = GameObject.Find("Main System/Info Canvas/Right Arm Power Text").GetComponent<Text>();
		RightArm_Servo_Text = GameObject.Find("Main System/Info Canvas/Right Arm Servo Text").GetComponent<Text>();
		RightArm_State_Text = GameObject.Find("Main System/Info Canvas/Right Arm State Text").GetComponent<Text>();
		RightArm_Pos_Text = GameObject.Find("Main System/Info Canvas/Right Arm Pos Text").GetComponent<Text>();

		LeftArm_Power_Text = GameObject.Find("Main System/Info Canvas/Left Arm Power Text").GetComponent<Text>();
		LeftArm_Servo_Text = GameObject.Find("Main System/Info Canvas/Left Arm Servo Text").GetComponent<Text>();
		LeftArm_State_Text = GameObject.Find("Main System/Info Canvas/Left Arm State Text").GetComponent<Text>();
		LeftArm_Pos_Text = GameObject.Find("Main System/Info Canvas/Left Arm Pos Text").GetComponent<Text>();

		wsc = GameObject.Find("Android Ros Socket Client").GetComponent<AndroidRosSocketClient>();
	}

	// Update is called once per frame
	void Update() {
		/*
		time += Time.deltaTime;
		if(time > 1.0f) {
			time = 0.0f;

			float[] arg = new float[0];
			Req_sp5_control srvReq = new Req_sp5_control(1, 7, arg);
			ServiceCaller_sp5_control(srvReq);
		}
		if (wsc.IsReceiveSrvRes() && wsc.GetSrvResValue("service") == "sp5_control") {
			srvRes = wsc.GetSrvResMsg();
			Debug.Log("ROS: " + srvRes);

			responce = JsonUtility.FromJson<Res_sp5_control>(srvRes);
			Debug.Log("result: " + responce.values.result.ToString());
			Debug.Log("val: " + responce.values.val.ToString());

			Vehicle_State_Text.text = "State: " + responce.values.result.ToString();
		}
		*/

		if(wsc.conneciton_state == wscCONST.STATE_DISCONNECTED) {
			time += Time.deltaTime;
			if(time > 5.0f) {
				time = 0.0f;
				wsc.Connect();
			}
		}

		if(wsc.conneciton_state == wscCONST.STATE_CONNECTED) {
			if(!success_access || !abort_access) {
				if (wait_VehicleState) {
					/*
					time += Time.deltaTime;
					if(time > 0.5f) {
						time = 0.0f;
						abort_access = true;
						//wait_VehicleState = false;
					}
					if(wsc.IsReceiveSrvRes() && wsc.GetSrvResValue("service") == "sp5_control") {
						srvRes = wsc.GetSrvResMsg();
						Debug.Log("ROS: " + srvRes);

						responce = JsonUtility.FromJson<Res_sp5_control>(srvRes);
						Debug.Log("result: " + responce.values.result.ToString());
						Debug.Log("val: " + responce.values.val.ToString());

						//Vehicle_State_Text.text = "State: " + responce.values.result.ToString();

						success_access = true;
						//wait_VehicleState = false;
					}
					*/
					WaitResponce(0.5f);
				}
				if (wait_VehiclePos) {
					/*
					time += Time.deltaTime;
					if (time > 0.5f) {
						time = 0.0f;
						abort_access = true;
					}
					if (wsc.IsReceiveSrvRes() && wsc.GetSrvResValue("service") == "sp5_control") {
						srvRes = wsc.GetSrvResMsg();
						Debug.Log("ROS: " + srvRes);

						responce = JsonUtility.FromJson<Res_sp5_control>(srvRes);
						Debug.Log("result: " + responce.values.result.ToString());
						Debug.Log("val: " + responce.values.val.ToString());
						
						success_access = true;
					}
					*/
					WaitResponce(0.5f);
				}
			}
		}

		time_vehicle_state += Time.deltaTime;
		if(!CheckWaitAnything() && time_vehicle_state > 1.0f) {
			time_vehicle_state = 0.0f;
			IEnumerator coroutine = ReadVehicleState();
			StartCoroutine(coroutine);
		}
		if (CheckWaitVehicleState()) {
			if (CheckAbort()) {
				FinishAccess();
			}
			if (CheckSuccess()) {
				Res_sp5_control responce = GetResponce();
				FinishAccess();

				Vehicle_State_Text.text = "State: " + responce.values.result.ToString();
			}
		}

		time_vehicle_pos += Time.deltaTime;
		if (!CheckWaitAnything() && time_vehicle_pos > 1.0f) {
			time_vehicle_pos = 0.0f;
			IEnumerator coroutine = ReadVehiclePos();
			StartCoroutine(coroutine);
		}
		if (CheckWaitVehiclePos()) {
			if (CheckAbort()) {
				FinishAccess();
			}
			if (CheckSuccess()) {
				Res_sp5_control responce = GetResponce();
				FinishAccess();

				Vector3 pos = new Vector3(responce.values.val[0], responce.values.val[1], responce.values.val[2]);
				Vehicle_Pos_Text.text = "Pos: " + pos.ToString("f2");
			}
		}
	}

	void WaitResponce(float timeout) {
		time += Time.deltaTime;
		if (time > timeout) {
			time = 0.0f;
			abort_access = true;
			access_db = false;
		}
		if (wsc.IsReceiveSrvRes() && wsc.GetSrvResValue("service") == "sp5_control") {
			srvRes = wsc.GetSrvResMsg();
			Debug.Log("ROS: " + srvRes);

			responce = JsonUtility.FromJson<Res_sp5_control>(srvRes);
			Debug.Log("result: " + responce.values.result.ToString());
			string val_string = "[";
			foreach(float val in responce.values.val) {
				val_string += val.ToString() + ",";
			}
			Debug.Log(val_string.Length);
			if(val_string.Length > 1) {
				val_string = val_string.Substring(0, val_string.Length - 1);
			}
			val_string += "]";
			Debug.Log("val: " + val_string);

			success_access = true;
			access_db = false;
		}
	}

	public bool CheckWaitAnything() {
		return wait_anything;
	}

	public bool CheckSuccess() {
		return success_access;
	}

	public bool CheckAbort() {
		return abort_access;
	}

	public Res_sp5_control GetResponce() {
		return responce;
	}

	public void FinishAccess() {
		success_access = abort_access = false;
	}


	/**************************************************
	 * Read Vehicle State
	 **************************************************/
	public IEnumerator ReadVehicleState() {
		wait_anything = access_db = wait_VehicleState = true;
		time = 0.0f;

		float[] arg = new float[0];
		Req_sp5_control srvReq = new Req_sp5_control(1, 7, arg);
		ServiceCaller_sp5_control(srvReq);

		/*
		if(wsc.IsReceiveSrvRes() && wsc.GetSrvResValue("service") == "sp5_control") {
			srvRes = wsc.GetSrvResMsg();
			Debug.Log("ROS: " + srvRes);

			responce = JsonUtility.FromJson<Res_sp5_control>(srvRes);
			Debug.Log("result: " + responce.result.ToString());
			Debug.Log("val: " + responce.values.val.ToString());

			Vehicle_State_Text.text = "State: " + responce.result;
		}
		*/
		/*
		while (wait_VehicleState) {
			yield return null;
		}
		*/
		/*
		while(!success_access && !abort_access) {
			Debug.Log("Accessing.......................");
			yield return null;
		}
		*/
		while (access_db) {
			yield return null;
		}

		while(success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_VehicleState = false;
	}

	public bool CheckWaitVehicleState() {
		return wait_VehicleState;
	}

	/**************************************************
	 * Read Vehicle Pos
	 **************************************************/
	public IEnumerator ReadVehiclePos() {
		wait_anything = access_db = wait_VehiclePos = true;
		time = 0.0f;

		float[] arg = new float[0];
		Req_sp5_control srvReq = new Req_sp5_control(1, 8, arg);
		ServiceCaller_sp5_control(srvReq);
		
		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_VehiclePos = false;
	}

	public bool CheckWaitVehiclePos() {
		return wait_VehiclePos;
	}
}
