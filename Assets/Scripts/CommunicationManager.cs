﻿using System.Collections;
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

	private MainScript mainSystem;

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
	private bool wait_VehicleStop = false;
	private bool wait_VehicleMove = false;

	private bool wait_RightArmPower = false;
	private bool wait_RightArmServo = false;
	private bool wait_RightArmState = false;
	private bool wait_RightArmPos = false;
	private bool wait_RightArmStop = false;
	private bool wait_RightArmReset = false;
	private bool wait_RightArmMove = false;
	private bool wait_RightArmClearAlarm = false;

	private bool wait_LeftArmPower = false;
	private bool wait_LeftArmServo = false;
	private bool wait_LeftArmState = false;
	private bool wait_LeftArmPos = false;
	private bool wait_LeftArmStop = false;
	private bool wait_LeftArmReset = false;
	private bool wait_LeftArmMove = false;
	private bool wait_LeftArmClearAlarm = false;

	private bool wait_RightGripperState = false;
	private bool wait_RightGripperPos = false;
	private bool wait_RightGripperStop = false;
	private bool wait_RightGripperMove = false;

	private bool wait_LeftGripperState = false;
	private bool wait_LeftGripperPos = false;
	private bool wait_LeftGripperStop = false;
	private bool wait_LeftGripperMove = false;


	// Start is called before the first frame update
	void Start() {
		mainSystem = GameObject.Find("Main System").GetComponent<MainScript>();
		wsc = GameObject.Find("Android Ros Socket Client").GetComponent<AndroidRosSocketClient>();
	}


	// Update is called once per frame
	void Update() {
		if (!mainSystem.IsFinishReadConfig()) {
			return;
		}

		if (wsc.conneciton_state == wscCONST.STATE_DISCONNECTED) { //切断時
			time_access += Time.deltaTime;
			if (time_access > 5.0f) {
				time_access = 0.0f;
				wsc.Connect();
			}
		}

		if (wsc.conneciton_state == wscCONST.STATE_CONNECTED) { //接続時
			if (!success_access || !abort_access) {
				//Vehicle
				if (wait_VehicleState) {
					WaitResponce(0.5f);
				}
				if (wait_VehiclePos) {
					WaitResponce(0.5f);
				}
				if (wait_VehicleStop) {
					WaitResponce(1.0f);
				}
				if (wait_VehicleMove) {
					WaitResponce(1.0f);
				}

				//Right Arm
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
				if (wait_RightArmStop) {
					WaitResponce(1.0f);
				}
				if (wait_RightArmReset) {
					WaitResponce(1.0f);
				}
				if (wait_RightArmMove) {
					WaitResponce(1.0f);
				}
				if (wait_RightArmClearAlarm) {
					WaitResponce(1.0f);
				}

				//Left Arm
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
				if (wait_LeftArmStop) {
					WaitResponce(1.0f);
				}
				if (wait_LeftArmReset) {
					WaitResponce(1.0f);
				}
				if (wait_LeftArmMove) {
					WaitResponce(1.0f);
				}
				if (wait_LeftArmClearAlarm) {
					WaitResponce(1.0f);
				}

				//Right Gripper
				if (wait_RightGripperState) {
					WaitResponce(0.5f);
				}
				if (wait_RightGripperPos) {
					WaitResponce(0.5f);
				}
				if (wait_RightGripperStop) {
					WaitResponce(1.0f);
				}
				if (wait_RightGripperMove) {
					WaitResponce(1.0f);
				}

				//Left Gripper
				if (wait_LeftGripperState) {
					WaitResponce(0.5f);
				}
				if (wait_LeftGripperPos) {
					WaitResponce(0.5f);
				}
				if (wait_LeftGripperStop) {
					WaitResponce(1.0f);
				}
				if (wait_LeftGripperMove) {
					WaitResponce(1.0f);
				}
			}
		}
	}


	public int wscConnectionState() {
		return wsc.conneciton_state;
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
	 * Vehicle Stop
	 **************************************************/
	public IEnumerator VehicleStop() {
		wait_anything = access_db = wait_VehicleStop = true;
		time_access = 0.0f;

		float[] arg = new float[0];
		Req_sp5_control srvReq = new Req_sp5_control(1, 6, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_VehicleStop = false;
	}

	public bool CheckWaitVehicleStop() {
		return wait_VehicleStop;
	}

	/**************************************************
	 * Vehicle Move
	 **************************************************/
	public IEnumerator VehicleMove(float x_m, float y_m, float theta_rad) {
		wait_anything = access_db = wait_VehicleMove = true;
		time_access = 0.0f;

		float[] arg = new float[3] { x_m, y_m, theta_rad };
		Debug.Log("SendMessage arg: " + x_m.ToString() + ", " + y_m.ToString() + ", " + theta_rad.ToString());
		Req_sp5_control srvReq = new Req_sp5_control(1, 16, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_VehicleMove = false;
	}

	public bool CheckWaitVehicleMove() {
		return wait_VehicleMove;
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
	 * Right Arm Stop
	 **************************************************/
	public IEnumerator RightArmStop() {
		wait_anything = access_db = wait_RightArmStop = true;
		time_access = 0.0f;

		float[] arg = new float[0];
		Req_sp5_control srvReq = new Req_sp5_control(2, 5, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_RightArmStop = false;
	}

	public bool CheckWaitRightArmStop() {
		return wait_RightArmStop;
	}

	/**************************************************
	 * Right Arm Reset
	 **************************************************/
	public IEnumerator RightArmReset() {
		wait_anything = access_db = wait_RightArmReset = true;
		time_access = 0.0f;

		float[] arg = new float[8];
		arg[0] = mainSystem.GetConfig().right_arm_home_pos[0] * Mathf.Deg2Rad;
		arg[1] = mainSystem.GetConfig().right_arm_home_pos[1] * Mathf.Deg2Rad;
		arg[2] = mainSystem.GetConfig().right_arm_home_pos[2] * Mathf.Deg2Rad;
		arg[3] = mainSystem.GetConfig().right_arm_home_pos[3] * Mathf.Deg2Rad;
		arg[4] = mainSystem.GetConfig().right_arm_home_pos[4] * Mathf.Deg2Rad;
		arg[5] = mainSystem.GetConfig().right_arm_home_pos[5] * Mathf.Deg2Rad;
		arg[6] = mainSystem.GetConfig().right_arm_home_pos[6] * Mathf.Deg2Rad;
		arg[7] = mainSystem.GetConfig().right_arm_move_speed * Mathf.Deg2Rad;
		string msg = "";
		foreach (float n in arg) {
			msg += n.ToString() + ", ";
		}
		msg = msg.Substring(0, msg.Length - 2);
		Debug.Log("SendMessage arg: " + msg);
		Req_sp5_control srvReq = new Req_sp5_control(2, 15, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_RightArmReset = false;
	}

	public bool CheckWaitRightArmReset() {
		return wait_RightArmReset;
	}

	/**************************************************
	 * Right Arm Move
	 **************************************************/
	public IEnumerator RightArmMove(float[] theta_rad) {
		wait_anything = access_db = wait_RightArmMove = true;
		time_access = 0.0f;

		float[] arg = new float[8];
		arg[0] = theta_rad[0];
		arg[1] = theta_rad[1];
		arg[2] = theta_rad[2];
		arg[3] = theta_rad[3];
		arg[4] = theta_rad[4];
		arg[5] = theta_rad[5];
		arg[6] = theta_rad[6];
		arg[7] = mainSystem.GetConfig().right_arm_move_speed * Mathf.Deg2Rad;
		string msg = "";
		foreach (float n in arg) {
			msg += n.ToString() + ", ";
		}
		msg = msg.Substring(0, msg.Length - 2);
		Debug.Log("SendMessage arg: " + msg);
		Req_sp5_control srvReq = new Req_sp5_control(2, 16, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_RightArmMove = false;
	}

	public bool CheckWaitRightArmMove() {
		return wait_RightArmMove;
	}

	/**************************************************
	 * Right Arm Clear Alarm
	 **************************************************/
	public IEnumerator RightArmClearAlarm() {
		wait_anything = access_db = wait_RightArmClearAlarm = true;
		time_access = 0.0f;

		float[] arg = new float[0];
		Req_sp5_control srvReq = new Req_sp5_control(2, 0, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_RightArmClearAlarm = false;
	}

	public bool CheckWaitRightArmClearAlarm() {
		return wait_RightArmClearAlarm;
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

	/**************************************************
	 * Left Arm Stop
	 **************************************************/
	public IEnumerator LeftArmStop() {
		wait_anything = access_db = wait_LeftArmStop = true;
		time_access = 0.0f;

		float[] arg = new float[0];
		Req_sp5_control srvReq = new Req_sp5_control(3, 5, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_LeftArmStop = false;
	}

	public bool CheckWaitLeftArmStop() {
		return wait_LeftArmStop;
	}

	/**************************************************
	 * Left Arm Reset
	 **************************************************/
	public IEnumerator LeftArmReset() {
		wait_anything = access_db = wait_LeftArmReset = true;
		time_access = 0.0f;

		float[] arg = new float[8];
		arg[0] = mainSystem.GetConfig().left_arm_home_pos[0] * Mathf.Deg2Rad;
		arg[1] = mainSystem.GetConfig().left_arm_home_pos[1] * Mathf.Deg2Rad;
		arg[2] = mainSystem.GetConfig().left_arm_home_pos[2] * Mathf.Deg2Rad;
		arg[3] = mainSystem.GetConfig().left_arm_home_pos[3] * Mathf.Deg2Rad;
		arg[4] = mainSystem.GetConfig().left_arm_home_pos[4] * Mathf.Deg2Rad;
		arg[5] = mainSystem.GetConfig().left_arm_home_pos[5] * Mathf.Deg2Rad;
		arg[6] = mainSystem.GetConfig().left_arm_home_pos[6] * Mathf.Deg2Rad;
		arg[7] = mainSystem.GetConfig().left_arm_move_speed * Mathf.Deg2Rad;
		string msg = "";
		foreach (float n in arg) {
			msg += n.ToString() + ", ";
		}
		msg = msg.Substring(0, msg.Length - 2);
		Debug.Log("SendMessage arg: " + msg);
		Req_sp5_control srvReq = new Req_sp5_control(3, 15, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_LeftArmReset = false;
	}

	public bool CheckWaitLeftArmReset() {
		return wait_LeftArmReset;
	}

	/**************************************************
	 * Left Arm Move
	 **************************************************/
	public IEnumerator LeftArmMove(float[] theta_rad) {
		wait_anything = access_db = wait_LeftArmMove = true;
		time_access = 0.0f;

		float[] arg = new float[8];
		arg[0] = theta_rad[0];
		arg[1] = theta_rad[1];
		arg[2] = theta_rad[2];
		arg[3] = theta_rad[3];
		arg[4] = theta_rad[4];
		arg[5] = theta_rad[5];
		arg[6] = theta_rad[6];
		arg[7] = mainSystem.GetConfig().left_arm_move_speed * Mathf.Deg2Rad;
		string msg = "";
		foreach (float n in arg) {
			msg += n.ToString() + ", ";
		}
		msg = msg.Substring(0, msg.Length - 2);
		Debug.Log("SendMessage arg: " + msg);
		Req_sp5_control srvReq = new Req_sp5_control(3, 16, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_LeftArmMove = false;
	}

	public bool CheckWaitLeftArmMove() {
		return wait_LeftArmMove;
	}

	/**************************************************
	 * Left Arm Clear Alarm
	 **************************************************/
	public IEnumerator LeftArmClearAlarm() {
		wait_anything = access_db = wait_LeftArmClearAlarm = true;
		time_access = 0.0f;

		float[] arg = new float[0];
		Req_sp5_control srvReq = new Req_sp5_control(3, 0, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_LeftArmClearAlarm = false;
	}

	public bool CheckWaitLeftArmClearAlarm() {
		return wait_LeftArmClearAlarm;
	}

	/**************************************************
	 * Read Right Gripper State
	 **************************************************/
	public IEnumerator ReadRightGripperState() {
		wait_anything = access_db = wait_RightGripperState = true;
		time_access = 0.0f;

		float[] arg = new float[0] {};
		Req_sp5_control srvReq = new Req_sp5_control(4, 7, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_RightGripperState = false;
	}

	public bool CheckWaitRightGripperState() {
		return wait_RightGripperState;
	}

	/**************************************************
	 * Read Right Gripper Pos
	 **************************************************/
	public IEnumerator ReadRightGripperPos() {
		wait_anything = access_db = wait_RightGripperPos = true;
		time_access = 0.0f;

		float[] arg = new float[0] { };
		Req_sp5_control srvReq = new Req_sp5_control(4, 8, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_RightGripperPos = false;
	}

	public bool CheckWaitRightGripperPos() {
		return wait_RightGripperPos;
	}

	/**************************************************
	 * Right Gripper Stop
	 **************************************************/
	public IEnumerator RightGripperStop() {
		wait_anything = access_db = wait_RightGripperStop = true;
		time_access = 0.0f;

		float[] arg = new float[0] { };
		Req_sp5_control srvReq = new Req_sp5_control(4, 5, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_RightGripperStop = false;
	}

	public bool CheckWaitRightGripperStop() {
		return wait_RightGripperStop;
	}

	/**************************************************
	 * Right Gripper Move
	 **************************************************/
	public IEnumerator RightGripperMove(float theta_rad) {
		wait_anything = access_db = wait_RightGripperMove = true;
		time_access = 0.0f;

		float[] arg = new float[3];
		arg[0] = theta_rad;
		arg[1] = mainSystem.GetConfig().right_gripper_move_speed * Mathf.Deg2Rad;
		arg[2] = mainSystem.GetConfig().right_gripper_move_speed * Mathf.Deg2Rad;
		Req_sp5_control srvReq = new Req_sp5_control(4, 15, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_RightGripperMove = false;
	}

	public bool CheckWaitRightGripperMove() {
		return wait_RightGripperMove;
	}

	/**************************************************
	 * Read Left Gripper State
	 **************************************************/
	public IEnumerator ReadLeftGripperState() {
		wait_anything = access_db = wait_LeftGripperState = true;
		time_access = 0.0f;

		float[] arg = new float[0] {};
		Req_sp5_control srvReq = new Req_sp5_control(5, 7, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_LeftGripperState = false;
	}

	public bool CheckWaitLeftGripperState() {
		return wait_LeftGripperState;
	}

	/**************************************************
	 * Read Left Gripper Pos
	 **************************************************/
	public IEnumerator ReadLeftGripperPos() {
		wait_anything = access_db = wait_LeftGripperPos = true;
		time_access = 0.0f;

		float[] arg = new float[0] { };
		Req_sp5_control srvReq = new Req_sp5_control(5, 8, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_LeftGripperPos = false;
	}

	public bool CheckWaitLeftGripperPos() {
		return wait_LeftGripperPos;
	}

	/**************************************************
	 * Left Gripper Stop
	 **************************************************/
	public IEnumerator LeftGripperStop() {
		wait_anything = access_db = wait_LeftGripperStop = true;
		time_access = 0.0f;

		float[] arg = new float[0] { };
		Req_sp5_control srvReq = new Req_sp5_control(5, 5, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_LeftGripperStop = false;
	}

	public bool CheckWaitLeftGripperStop() {
		return wait_LeftGripperStop;
	}

	/**************************************************
	 * Left Gripper Move
	 **************************************************/
	public IEnumerator LeftGripperMove(float theta_rad) {
		wait_anything = access_db = wait_LeftGripperMove = true;
		time_access = 0.0f;

		float[] arg = new float[3];
		arg[0] = theta_rad;
		arg[1] = mainSystem.GetConfig().left_gripper_move_speed * Mathf.Deg2Rad;
		arg[2] = mainSystem.GetConfig().left_gripper_move_speed * Mathf.Deg2Rad;
		Req_sp5_control srvReq = new Req_sp5_control(5, 15, arg);
		ServiceCaller_sp5_control(srvReq);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = wait_LeftGripperMove = false;
	}

	public bool CheckWaitLeftGripperMove() {
		return wait_LeftGripperMove;
	}

}
