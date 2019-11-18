using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InfoCanvasManager : MonoBehaviour {

	//UIたち
	private Text Information_Text;
	
	private Text Vehicle_State_Text;
	private Text Vehicle_Pos_Text;

	private Text RightArm_Power_Text;
	private Text RightArm_Servo_Text;
	private Text RightArm_State_Text;
	private Text RightArm_Pos_Text;
	private Button RightArm_ClearAlarm_Button;

	private Text LeftArm_Power_Text;
	private Text LeftArm_Servo_Text;
	private Text LeftArm_State_Text;
	private Text LeftArm_Pos_Text;
	private Button LeftArm_ClearAlarm_Button;

	private Text RightGripper_State_Text;
	private Text RightGripper_Pos_Text;

	private Text LeftGripper_State_Text;
	private Text LeftGripper_Pos_Text;

	//Communication Manager
	private CommunicationManager cm;

	//更新するタイミング用
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

	private float time_rightgripper_state = 0.0f;
	private float time_rightgripper_pos = 0.0f;

	private float time_leftgripper_state = 0.0f;
	private float time_leftgripper_pos = 0.0f;

	// Start is called before the first frame update
	void Start() {
		//UIを取得
		Information_Text = GameObject.Find("Main System/Info Canvas/Overall Information Text").GetComponent<Text>();
		
		Vehicle_State_Text = GameObject.Find("Main System/Info Canvas/Vehicle State Text").GetComponent<Text>();
		Vehicle_Pos_Text = GameObject.Find("Main System/Info Canvas/Vehicle Pos Text").GetComponent<Text>();

		RightArm_Power_Text = GameObject.Find("Main System/Info Canvas/Right Arm Power Text").GetComponent<Text>();
		RightArm_Servo_Text = GameObject.Find("Main System/Info Canvas/Right Arm Servo Text").GetComponent<Text>();
		RightArm_State_Text = GameObject.Find("Main System/Info Canvas/Right Arm State Text").GetComponent<Text>();
		RightArm_Pos_Text = GameObject.Find("Main System/Info Canvas/Right Arm Pos Text").GetComponent<Text>();
		RightArm_ClearAlarm_Button = GameObject.Find("Main System/Info Canvas/Right Arm Clear Alarm Button").GetComponent<Button>();
		RightArm_ClearAlarm_Button.onClick.AddListener(PushRightArmClearAlarm);
		RightArm_ClearAlarm_Button.gameObject.SetActive(false);

		LeftArm_Power_Text = GameObject.Find("Main System/Info Canvas/Left Arm Power Text").GetComponent<Text>();
		LeftArm_Servo_Text = GameObject.Find("Main System/Info Canvas/Left Arm Servo Text").GetComponent<Text>();
		LeftArm_State_Text = GameObject.Find("Main System/Info Canvas/Left Arm State Text").GetComponent<Text>();
		LeftArm_Pos_Text = GameObject.Find("Main System/Info Canvas/Left Arm Pos Text").GetComponent<Text>();
		LeftArm_ClearAlarm_Button = GameObject.Find("Main System/Info Canvas/Left Arm Clear Alarm Button").GetComponent<Button>();
		LeftArm_ClearAlarm_Button.onClick.AddListener(PushLeftArmClearAlarm);
		LeftArm_ClearAlarm_Button.gameObject.SetActive(false);

		RightGripper_State_Text = GameObject.Find("Main System/Info Canvas/Right Gripper State Text").GetComponent<Text>();
		RightGripper_Pos_Text = GameObject.Find("Main System/Info Canvas/Right Gripper Pos Text").GetComponent<Text>();

		LeftGripper_State_Text = GameObject.Find("Main System/Info Canvas/Left Gripper State Text").GetComponent<Text>();
		LeftGripper_Pos_Text = GameObject.Find("Main System/Info Canvas/Left Gripper Pos Text").GetComponent<Text>();

		//Communication Managerを取得
		cm = GameObject.Find("Communication Manager").GetComponent<CommunicationManager>();
	}

	// Update is called once per frame
	void Update() {
		switch (cm.wscConnectionState()) {
			case wscCONST.STATE_CONNECTED:
				Information_Text.text = "System: Connected";
				break;
			case wscCONST.STATE_DISCONNECTED:
				Information_Text.text = "System: Disconnected";
				break;
			case wscCONST.STATE_ERROR:
				Information_Text.text = "System: Error";
				break;
		}

		//Vehicle周りの情報取得
		time_vehicle_state += Time.deltaTime;
		if(cm.wscConnectionState() == wscCONST.STATE_CONNECTED && !cm.CheckWaitAnything() && time_vehicle_state > 1.0f) {
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
		if (cm.wscConnectionState() == wscCONST.STATE_CONNECTED && !cm.CheckWaitAnything() && time_vehicle_pos > 1.0f) {
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

		//Right Arm周りの情報取得
		time_rightarm_power += Time.deltaTime;
		if(cm.wscConnectionState() == wscCONST.STATE_CONNECTED && !cm.CheckWaitAnything() && time_rightarm_power > 3.0f) {
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
		if (cm.wscConnectionState() == wscCONST.STATE_CONNECTED && !cm.CheckWaitAnything() && time_rightarm_servo > 3.0f) {
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
		if (cm.wscConnectionState() == wscCONST.STATE_CONNECTED && !cm.CheckWaitAnything() && time_rightarm_state > 1.0f) {
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

				if (responce.values.result == 21) {
					RightArm_ClearAlarm_Button.gameObject.SetActive(true);
				}
				else {
					RightArm_ClearAlarm_Button.gameObject.SetActive(false);
				}
			}
		}

		time_rightarm_pos += Time.deltaTime;
		if (cm.wscConnectionState() == wscCONST.STATE_CONNECTED && !cm.CheckWaitAnything() && time_rightarm_pos > 1.0f) {
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
					float val_tmp = val * Mathf.Rad2Deg;
					val_string += val_tmp.ToString("f0") + ", ";
				}
				if (val_string.Length > 1) {
					val_string = val_string.Substring(0, val_string.Length - 2);
				}
				val_string += ")";
				RightArm_Pos_Text.text = "Joint: " + val_string;
			}
		}

		//Left Arm周りの情報取得
		time_leftarm_power += Time.deltaTime;
		if (cm.wscConnectionState() == wscCONST.STATE_CONNECTED && !cm.CheckWaitAnything() && time_leftarm_power > 3.0f) {
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
		if (cm.wscConnectionState() == wscCONST.STATE_CONNECTED && !cm.CheckWaitAnything() && time_leftarm_servo > 3.0f) {
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
		if (cm.wscConnectionState() == wscCONST.STATE_CONNECTED && !cm.CheckWaitAnything() && time_leftarm_state > 1.0f) {
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

				if (responce.values.result == 21) {
					LeftArm_ClearAlarm_Button.gameObject.SetActive(true);
				}
				else {
					LeftArm_ClearAlarm_Button.gameObject.SetActive(false);
				}
			}
		}

		time_leftarm_pos += Time.deltaTime;
		if (cm.wscConnectionState() == wscCONST.STATE_CONNECTED && !cm.CheckWaitAnything() && time_leftarm_pos > 1.0f) {
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
					float val_tmp = val * Mathf.Rad2Deg;
					val_string += val_tmp.ToString("f0") + ", ";
				}
				if (val_string.Length > 1) {
					val_string = val_string.Substring(0, val_string.Length - 2);
				}
				val_string += ")";
				LeftArm_Pos_Text.text = "Joint: " + val_string;
			}
		}

		//Right Gripper周りの情報取得
		time_rightgripper_state += Time.deltaTime;
		if (cm.wscConnectionState() == wscCONST.STATE_CONNECTED && !cm.CheckWaitAnything() && time_rightgripper_state > 1.0f) {
			time_rightgripper_state = 0.0f;
			IEnumerator coroutine = cm.ReadRightGripperState();
			StartCoroutine(coroutine);
		}
		if (cm.CheckWaitRightGripperState()) {
			if (cm.CheckAbort()) {
				cm.FinishAccess();
			}
			if (cm.CheckSuccess()) {
				Res_sp5_control responce = cm.GetResponce();
				cm.FinishAccess();

				switch (responce.values.result) {
					case 0:
						RightGripper_State_Text.text = "State: Not Moving";
						break;
					case 1:
						RightGripper_State_Text.text = "State: Moving";
						break;
					default:
						RightGripper_State_Text.text = "State: Unknown State";
						break;
				}
			}
		}

		time_rightgripper_pos += Time.deltaTime;
		if (cm.wscConnectionState() == wscCONST.STATE_CONNECTED && !cm.CheckWaitAnything() && time_rightgripper_pos > 1.0f) {
			time_rightgripper_pos = 0.0f;
			IEnumerator coroutine = cm.ReadRightGripperPos();
			StartCoroutine(coroutine);
		}
		if (cm.CheckWaitRightGripperPos()) {
			if (cm.CheckAbort()) {
				cm.FinishAccess();
			}
			if (cm.CheckSuccess()) {
				Res_sp5_control responce = cm.GetResponce();
				cm.FinishAccess();

				RightGripper_Pos_Text.text = "Joint: (" + (responce.values.val[0] * Mathf.Rad2Deg).ToString("f0") + ")";
			}
		}

		//Left Gripper周りの情報取得
		time_leftgripper_state += Time.deltaTime;
		if (cm.wscConnectionState() == wscCONST.STATE_CONNECTED && !cm.CheckWaitAnything() && time_leftgripper_state > 1.0f) {
			time_leftgripper_state = 0.0f;
			IEnumerator coroutine = cm.ReadLeftGripperState();
			StartCoroutine(coroutine);
		}
		if (cm.CheckWaitLeftGripperState()) {
			if (cm.CheckAbort()) {
				cm.FinishAccess();
			}
			if (cm.CheckSuccess()) {
				Res_sp5_control responce = cm.GetResponce();
				cm.FinishAccess();

				switch (responce.values.result) {
					case 0:
						LeftGripper_State_Text.text = "State: Not Moving";
						break;
					case 1:
						LeftGripper_State_Text.text = "State: Moving";
						break;
					default:
						LeftGripper_State_Text.text = "State: Unknown State";
						break;
				}
			}
		}

		time_leftgripper_pos += Time.deltaTime;
		if (cm.wscConnectionState() == wscCONST.STATE_CONNECTED && !cm.CheckWaitAnything() && time_leftgripper_pos > 1.0f) {
			time_leftgripper_pos = 0.0f;
			IEnumerator coroutine = cm.ReadLeftGripperPos();
			StartCoroutine(coroutine);
		}
		if (cm.CheckWaitLeftGripperPos()) {
			if (cm.CheckAbort()) {
				cm.FinishAccess();
			}
			if (cm.CheckSuccess()) {
				Res_sp5_control responce = cm.GetResponce();
				cm.FinishAccess();

				LeftGripper_Pos_Text.text = "Joint: (" + (responce.values.val[0]*Mathf.Rad2Deg).ToString("f0") + ")";
			}
		}
	}

	/**************************************************
	 * Clear Alarm Buttonを押したとき
	 **************************************************/
	void PushRightArmClearAlarm() {
		if(cm.wscConnectionState() == wscCONST.STATE_CONNECTED) {
			IEnumerator coroutine = SendRightArmClearAlarm();
			StartCoroutine(coroutine);
		}
	}

	IEnumerator SendRightArmClearAlarm() {
		while (cm.CheckWaitAnything()) {
			yield return null;
		}

		IEnumerator coroutine = cm.RightArmClearAlarm();
		StartCoroutine(coroutine);

		while (cm.CheckWaitRightArmClearAlarm()) {
			if (cm.CheckAbort() || cm.CheckSuccess()) {
				cm.FinishAccess();
			}
			yield return null;
		}
	}

	void PushLeftArmClearAlarm() {
		if (cm.wscConnectionState() == wscCONST.STATE_CONNECTED) {
			IEnumerator coroutine = SendLeftArmClearAlarm();
			StartCoroutine(coroutine);
		}
	}

	IEnumerator SendLeftArmClearAlarm() {
		while (cm.CheckWaitAnything()) {
			yield return null;
		}

		IEnumerator coroutine = cm.LeftArmClearAlarm();
		StartCoroutine(coroutine);

		while (cm.CheckWaitLeftArmClearAlarm()) {
			if (cm.CheckAbort() || cm.CheckSuccess()) {
				cm.FinishAccess();
			}
			yield return null;
		}
	}
}
