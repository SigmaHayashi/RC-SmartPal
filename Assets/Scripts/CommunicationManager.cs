using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

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

public class CommunicationManager : MonoBehaviour {
	
	public void ServiceCaller_sp5_control(Req_sp5_control srvReq) {
		ServiceCall_sp5_control call = new ServiceCall_sp5_control(srvReq);
		wsc.SendOpMsg(call);
	}

	private AndroidRosSocketClient wsc;
	private string srvRes;
	private Res_sp5_control responce;

	private float time_access = 0.0f;

	private bool wait_anything = false;
	private bool access_db = false;
	private bool success_access = false;
	private bool abort_access = false;

	private bool wait_VehicleState = false;
	private bool wait_VehiclePos = false;
	private bool wait_RightArmPower = false;
	private bool wait_RightArmServo = false;
	private bool wait_RightArmState = false;
	private bool wait_RightArmPos = false;
	private bool wait_LeftArmPower = false;
	private bool wait_LeftArmServo = false;
	private bool wait_LeftArmState = false;
	private bool wait_LeftArmPos = false;


	// Start is called before the first frame update
	void Start() {
		wsc = GameObject.Find("Android Ros Socket Client").GetComponent<AndroidRosSocketClient>();
	}


	// Update is called once per frame
	void Update() {
		if (wsc.conneciton_state == wscCONST.STATE_DISCONNECTED) { //切断時
			time_access += Time.deltaTime;
			if (time_access > 5.0f) {
				time_access = 0.0f;
				wsc.Connect();
			}
		}

		if (wsc.conneciton_state == wscCONST.STATE_CONNECTED) { //接続時
			if (!success_access || !abort_access) {
				if (wait_VehicleState) {
					WaitResponce(0.5f);
				}
				if (wait_VehiclePos) {
					WaitResponce(0.5f);
				}

				if (wait_RightArmPower) {
					WaitResponce(0.5f);
				}
				if (wait_RightArmServo) {
					WaitResponce(0.5f);
				}
				if (wait_RightArmState) {
					WaitResponce(0.5f);
				}
				if (wait_RightArmPos) {
					WaitResponce(0.5f);
				}

				if (wait_LeftArmPower) {
					WaitResponce(0.5f);
				}
				if (wait_LeftArmServo) {
					WaitResponce(0.5f);
				}
				if (wait_LeftArmState) {
					WaitResponce(0.5f);
				}
				if (wait_LeftArmPos) {
					WaitResponce(0.5f);
				}
			}
		}
	}

	
	/**************************************************
	 * ROSからの返答待ち
	 **************************************************/
	void WaitResponce(float timeout) {
		time_access += Time.deltaTime;
		if (time_access > timeout) {
			time_access = 0.0f;
			abort_access = true;
			access_db = false;
		}
		if (wsc.IsReceiveSrvRes() && wsc.GetSrvResValue("service") == "sp5_control") {
			srvRes = wsc.GetSrvResMsg();
			Debug.Log("ROS: " + srvRes);

			responce = JsonUtility.FromJson<Res_sp5_control>(srvRes);
			Debug.Log("result: " + responce.values.result.ToString());
			string val_string = "[";
			foreach (float val in responce.values.val) {
				val_string += val.ToString() + ", ";
			}
			Debug.Log(val_string.Length);
			if (val_string.Length > 1) {
				val_string = val_string.Substring(0, val_string.Length - 2);
			}
			val_string += "]";
			Debug.Log("val: " + val_string);

			success_access = true;
			access_db = false;
		}
	}


	/**************************************************
	 * データ取得時のAPI
	 **************************************************/
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
		time_access = 0.0f;

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

	/**************************************************
	 * Read Vehicle Pos
	 **************************************************/
	public IEnumerator ReadVehiclePos() {
		wait_anything = access_db = wait_VehiclePos = true;
		time_access = 0.0f;

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

	/**************************************************
	 * Read Right Arm Power
	 **************************************************/
	public IEnumerator ReadRightArmPower() {
		wait_anything = access_db = wait_RightArmPower = true;
		time_access = 0.0f;

		float[] arg = new float[0];
		Req_sp5_control srvReq = new Req_sp5_control(2, 12, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_RightArmPower = false;
	}

	public bool CheckWaitRightArmPower() {
		return wait_RightArmPower;
	}

	/**************************************************
	 * Read Right Arm Servo
	 **************************************************/
	public IEnumerator ReadRightArmServo() {
		wait_anything = access_db = wait_RightArmServo = true;
		time_access = 0.0f;

		float[] arg = new float[0];
		Req_sp5_control srvReq = new Req_sp5_control(2, 13, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_RightArmServo = false;
	}

	public bool CheckWaitRightArmServo() {
		return wait_RightArmServo;
	}

	/**************************************************
	 * Read Right Arm State
	 **************************************************/
	public IEnumerator ReadRightArmState() {
		wait_anything = access_db = wait_RightArmState = true;
		time_access = 0.0f;

		float[] arg = new float[0];
		Req_sp5_control srvReq = new Req_sp5_control(2, 7, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_RightArmState = false;
	}

	public bool CheckWaitRightArmState() {
		return wait_RightArmState;
	}

	/**************************************************
	 * Read Right Arm Pos
	 **************************************************/
	public IEnumerator ReadRightArmPos() {
		wait_anything = access_db = wait_RightArmPos = true;
		time_access = 0.0f;

		float[] arg = new float[1] { 0 };
		Req_sp5_control srvReq = new Req_sp5_control(2, 8, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_RightArmPos = false;
	}

	public bool CheckWaitRightArmPos() {
		return wait_RightArmPos;
	}

	/**************************************************
	 * Read Left Arm Power
	 **************************************************/
	public IEnumerator ReadLeftArmPower() {
		wait_anything = access_db = wait_LeftArmPower = true;
		time_access = 0.0f;

		float[] arg = new float[0];
		Req_sp5_control srvReq = new Req_sp5_control(3, 12, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_LeftArmPower = false;
	}

	public bool CheckWaitLeftArmPower() {
		return wait_LeftArmPower;
	}

	/**************************************************
	 * Read Left Arm Servo
	 **************************************************/
	public IEnumerator ReadLeftArmServo() {
		wait_anything = access_db = wait_LeftArmServo = true;
		time_access = 0.0f;

		float[] arg = new float[0];
		Req_sp5_control srvReq = new Req_sp5_control(3, 13, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_LeftArmServo = false;
	}

	public bool CheckWaitLeftArmServo() {
		return wait_LeftArmServo;
	}

	/**************************************************
	 * Read Left Arm State
	 **************************************************/
	public IEnumerator ReadLeftArmState() {
		wait_anything = access_db = wait_LeftArmState = true;
		time_access = 0.0f;

		float[] arg = new float[0];
		Req_sp5_control srvReq = new Req_sp5_control(3, 7, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_LeftArmState = false;
	}

	public bool CheckWaitLeftArmState() {
		return wait_LeftArmState;
	}

	/**************************************************
	 * Read Left Arm Pos
	 **************************************************/
	public IEnumerator ReadLeftArmPos() {
		wait_anything = access_db = wait_LeftArmPos = true;
		time_access = 0.0f;

		float[] arg = new float[1] { 0 };
		Req_sp5_control srvReq = new Req_sp5_control(3, 8, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_LeftArmPos = false;
	}

	public bool CheckWaitLeftArmPos() {
		return wait_LeftArmPos;
	}
}
