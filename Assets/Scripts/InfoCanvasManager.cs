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
	/*
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
	*/
	private CommunicationManager cm;

	private float time_vehicle_state = 0.0f;
	private float time_vehicle_pos = 0.0f;
	private float time_rightarm_power = 0.0f;
	private float time_rightarm_servo = 0.0f;
	private float time_rightarm_state = 0.0f;
	private float time_rightarm_pos = 0.0f;
	private float time_leftarm_power = 0.0f;
	private float time_leftarm_servo = 0.0f;
	private float time_leftarm_state = 0.0f;
	private float time_leftarm_pos = 0.0f;

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

		//wsc = GameObject.Find("Android Ros Socket Client").GetComponent<AndroidRosSocketClient>();
		cm = GameObject.Find("Communication Manager").GetComponent<CommunicationManager>();
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

		/*
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
					WaitResponce(0.5f);
				}
				if (wait_VehiclePos) {
					WaitResponce(0.5f);
				}
			}
		}
		*/

		time_vehicle_state += Time.deltaTime;
		if(!cm.CheckWaitAnything() && time_vehicle_state > 1.0f) {
			time_vehicle_state = 0.0f;
			IEnumerator coroutine = cm.ReadVehicleState();
			StartCoroutine(coroutine);
		}
		if (cm.CheckWaitVehicleState()) {
			if (cm.CheckAbort()) {
				cm.FinishAccess();
			}
			if (cm.CheckSuccess()) {
				Res_sp5_control responce = cm.GetResponce();
				cm.FinishAccess();

				//Vehicle_State_Text.text = "State: " + responce.values.result.ToString();
				switch (responce.values.result) {
					case 16:
						Vehicle_State_Text.text = "State: Ready";
						break;
					case 17:
						Vehicle_State_Text.text = "State: Busy";
						break;
					case 19:
						Vehicle_State_Text.text = "State: Alarm";
						break;
					case 20:
						Vehicle_State_Text.text = "State: Stuck";
						break;
					case 21:
						Vehicle_State_Text.text = "State: Paused";
						break;
					case 23:
						Vehicle_State_Text.text = "State: Locked";
						break;
					case 24:
						Vehicle_State_Text.text = "State: Powered";
						break;
					case 25:
						Vehicle_State_Text.text = "State: Unpowered";
						break;
					case 26:
						Vehicle_State_Text.text = "State: Caution";
						break;
					default:
						Vehicle_State_Text.text = "State: Unknown State";
						break;
				}
			}
		}

		time_vehicle_pos += Time.deltaTime;
		if (!cm.CheckWaitAnything() && time_vehicle_pos > 1.0f) {
			time_vehicle_pos = 0.0f;
			IEnumerator coroutine = cm.ReadVehiclePos();
			StartCoroutine(coroutine);
		}
		if (cm.CheckWaitVehiclePos()) {
			if (cm.CheckAbort()) {
				cm.FinishAccess();
			}
			if (cm.CheckSuccess()) {
				Res_sp5_control responce = cm.GetResponce();
				cm.FinishAccess();

				Vector3 pos = new Vector3(responce.values.val[0], responce.values.val[1], responce.values.val[2]);
				Vehicle_Pos_Text.text = "Pos: " + pos.ToString("f2");
			}
		}

		time_rightarm_power += Time.deltaTime;
		if(!cm.CheckWaitAnything() && time_rightarm_power > 1.0f) {
			time_rightarm_power = 0.0f;
			IEnumerator coroutine = cm.ReadRightArmPower();
			StartCoroutine(coroutine);
		}
		if (cm.CheckWaitRightArmPower()) {
			if (cm.CheckAbort()) {
				cm.FinishAccess();
			}
			if (cm.CheckSuccess()) {
				Res_sp5_control responce = cm.GetResponce();
				cm.FinishAccess();

				if(responce.values.result == 1) {
					RightArm_Power_Text.text = "Power: OK!!";
				}
				else {
					RightArm_Power_Text.text = "Power: NG...";
				}
			}
		}

		time_rightarm_servo += Time.deltaTime;
		if (!cm.CheckWaitAnything() && time_rightarm_servo > 1.0f) {
			time_rightarm_servo = 0.0f;
			IEnumerator coroutine = cm.ReadRightArmServo();
			StartCoroutine(coroutine);
		}
		if (cm.CheckWaitRightArmServo()) {
			if (cm.CheckAbort()) {
				cm.FinishAccess();
			}
			if (cm.CheckSuccess()) {
				Res_sp5_control responce = cm.GetResponce();
				cm.FinishAccess();

				if(responce.values.result == 1) {
					RightArm_Servo_Text.text = "Servo: OK!!";
				}
				else {
					RightArm_Servo_Text.text = "Servo: NG...";
				}
			}
		}

		time_rightarm_state += Time.deltaTime;
		if (!cm.CheckWaitAnything() && time_rightarm_state > 1.0f) {
			time_rightarm_state = 0.0f;
			IEnumerator coroutine = cm.ReadRightArmState();
			StartCoroutine(coroutine);
		}
		if (cm.CheckWaitRightArmState()) {
			if (cm.CheckAbort()) {
				cm.FinishAccess();
			}
			if (cm.CheckSuccess()) {
				Res_sp5_control responce = cm.GetResponce();
				cm.FinishAccess();

				//RightArm_State_Text.text = "State: " + responce.values.result.ToString();
				switch (responce.values.result) {
					case 16:
						RightArm_State_Text.text = "State: Unpowered";
						break;
					case 17:
						RightArm_State_Text.text = "State: Powered";
						break;
					case 18:
						RightArm_State_Text.text = "State: Ready";
						break;
					case 19:
						RightArm_State_Text.text = "State: Busy";
						break;
					case 20:
						RightArm_State_Text.text = "State: Paused";
						break;
					case 21:
						RightArm_State_Text.text = "State: Alarm";
						break;
					case 22:
						RightArm_State_Text.text = "State: Jog Busy";
						break;
					case 23:
						RightArm_State_Text.text = "State: Direct Busy";
						break;
					default:
						RightArm_State_Text.text = "State: Unknown State";
						break;
				}
			}
		}

		time_rightarm_pos += Time.deltaTime;
		if (!cm.CheckWaitAnything() && time_rightarm_pos > 1.0f) {
			time_rightarm_pos = 0.0f;
			IEnumerator coroutine = cm.ReadRightArmPos();
			StartCoroutine(coroutine);
		}
		if (cm.CheckWaitRightArmPos()) {
			if (cm.CheckAbort()) {
				cm.FinishAccess();
			}
			if (cm.CheckSuccess()) {
				Res_sp5_control responce = cm.GetResponce();
				cm.FinishAccess();

				string val_string = "(";
				foreach (float val in responce.values.val) {
					val_string += val.ToString("f2") + ", ";
				}
				Debug.Log(val_string.Length);
				if (val_string.Length > 1) {
					val_string = val_string.Substring(0, val_string.Length - 2);
				}
				val_string += ")";
				RightArm_Pos_Text.text = "Pos: " + val_string;
			}
		}

		time_leftarm_power += Time.deltaTime;
		if (!cm.CheckWaitAnything() && time_leftarm_power > 1.0f) {
			time_leftarm_power = 0.0f;
			IEnumerator coroutine = cm.ReadLeftArmPower();
			StartCoroutine(coroutine);
		}
		if (cm.CheckWaitLeftArmPower()) {
			if (cm.CheckAbort()) {
				cm.FinishAccess();
			}
			if (cm.CheckSuccess()) {
				Res_sp5_control responce = cm.GetResponce();
				cm.FinishAccess();

				if (responce.values.result == 1) {
					LeftArm_Power_Text.text = "Power: OK!!";
				}
				else {
					LeftArm_Power_Text.text = "Power: NG...";
				}
			}
		}

		time_leftarm_servo += Time.deltaTime;
		if (!cm.CheckWaitAnything() && time_leftarm_servo > 1.0f) {
			time_leftarm_servo = 0.0f;
			IEnumerator coroutine = cm.ReadLeftArmServo();
			StartCoroutine(coroutine);
		}
		if (cm.CheckWaitLeftArmServo()) {
			if (cm.CheckAbort()) {
				cm.FinishAccess();
			}
			if (cm.CheckSuccess()) {
				Res_sp5_control responce = cm.GetResponce();
				cm.FinishAccess();

				if (responce.values.result == 1) {
					LeftArm_Servo_Text.text = "Servo: OK!!";
				}
				else {
					LeftArm_Servo_Text.text = "Servo: NG...";
				}
			}
		}

		time_leftarm_state += Time.deltaTime;
		if (!cm.CheckWaitAnything() && time_leftarm_state > 1.0f) {
			time_leftarm_state = 0.0f;
			IEnumerator coroutine = cm.ReadLeftArmState();
			StartCoroutine(coroutine);
		}
		if (cm.CheckWaitLeftArmState()) {
			if (cm.CheckAbort()) {
				cm.FinishAccess();
			}
			if (cm.CheckSuccess()) {
				Res_sp5_control responce = cm.GetResponce();
				cm.FinishAccess();

				//LeftArm_State_Text.text = "State: " + responce.values.result.ToString();
				switch (responce.values.result) {
					case 16:
						LeftArm_State_Text.text = "State: Unpowered";
						break;
					case 17:
						LeftArm_State_Text.text = "State: Powered";
						break;
					case 18:
						LeftArm_State_Text.text = "State: Ready";
						break;
					case 19:
						LeftArm_State_Text.text = "State: Busy";
						break;
					case 20:
						LeftArm_State_Text.text = "State: Paused";
						break;
					case 21:
						LeftArm_State_Text.text = "State: Alarm";
						break;
					case 22:
						LeftArm_State_Text.text = "State: Jog Busy";
						break;
					case 23:
						LeftArm_State_Text.text = "State: Direct Busy";
						break;
					default:
						LeftArm_State_Text.text = "State: Unknown State";
						break;
				}
			}
		}

		time_leftarm_pos += Time.deltaTime;
		if (!cm.CheckWaitAnything() && time_leftarm_pos > 1.0f) {
			time_leftarm_pos = 0.0f;
			IEnumerator coroutine = cm.ReadLeftArmPos();
			StartCoroutine(coroutine);
		}
		if (cm.CheckWaitLeftArmPos()) {
			if (cm.CheckAbort()) {
				cm.FinishAccess();
			}
			if (cm.CheckSuccess()) {
				Res_sp5_control responce = cm.GetResponce();
				cm.FinishAccess();

				string val_string = "(";
				foreach (float val in responce.values.val) {
					val_string += val.ToString("f2") + ", ";
				}
				Debug.Log(val_string.Length);
				if (val_string.Length > 1) {
					val_string = val_string.Substring(0, val_string.Length - 2);
				}
				val_string += ")";
				LeftArm_Pos_Text.text = "Pos: " + val_string;
			}
		}
	}
	/*
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
	*/


	/**************************************************
	 * Read Vehicle State
	 **************************************************//*
	public IEnumerator ReadVehicleState() {
		wait_anything = access_db = wait_VehicleState = true;
		time = 0.0f;

		float[] arg = new float[0];
		Req_sp5_control srvReq = new Req_sp5_control(1, 7, arg);
		ServiceCaller_sp5_control(srvReq);
		
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
	*/

	/**************************************************
	 * Read Vehicle Pos
	 **************************************************//*
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
	*/
}
