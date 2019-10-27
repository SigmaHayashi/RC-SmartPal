using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VehicleCanvasManager : MonoBehaviour {

	//UIたち
	private Text Vehicle_State_Text;

	private Button Vehicle_Forward_Button;
	private Button Vehicle_Back_Button;
	private Button Vehicle_Right_Button;
	private Button Vehicle_Left_Button;
	private Button Vehicle_TurnRight_Button;
	private Button Vehicle_TurnLeft_Button;

	private Button Vehicle_Stop_Button;

	//Communication Manager
	private CommunicationManager cm;

	//更新するタイミング用
	private float time_vehicle_state = 0.0f;

	//どのボタンを押しているか
	private bool push_forward = false;
	private bool push_back = false;
	private bool push_right = false;
	private bool push_left = false;
	private bool push_turnright = false;
	private bool push_turnleft = false;

	// Start is called before the first frame update
	void Start() {
		Vehicle_State_Text = GameObject.Find("Main System/Vehicle Canvas/Vehicle State Text").GetComponent<Text>();

		Vehicle_Forward_Button = GameObject.Find("Main System/Vehicle Canvas/Forward Button").GetComponent<Button>();
		Vehicle_Back_Button = GameObject.Find("Main System/Vehicle Canvas/Back Button").GetComponent<Button>();
		Vehicle_Right_Button = GameObject.Find("Main System/Vehicle Canvas/Right Button").GetComponent<Button>();
		Vehicle_Left_Button = GameObject.Find("Main System/Vehicle Canvas/Left Button").GetComponent<Button>();
		Vehicle_TurnRight_Button = GameObject.Find("Main System/Vehicle Canvas/Turn Right Button").GetComponent<Button>();
		Vehicle_TurnLeft_Button = GameObject.Find("Main System/Vehicle Canvas/Turn Left Button").GetComponent<Button>();

		Vehicle_Stop_Button = GameObject.Find("Main System/Vehicle Canvas/Stop Button").GetComponent<Button>();

		SettingButton(Vehicle_Forward_Button);
		SettingButton(Vehicle_Back_Button);
		SettingButton(Vehicle_Right_Button);
		SettingButton(Vehicle_Left_Button);
		SettingButton(Vehicle_TurnRight_Button);
		SettingButton(Vehicle_TurnLeft_Button);

		Vehicle_Stop_Button.onClick.AddListener(PushStop);

		cm = GameObject.Find("Communication Manager").GetComponent<CommunicationManager>();
	}

	void SettingButton(Button button) {
		EventTrigger trigger = button.GetComponent<EventTrigger>();
		EventTrigger.Entry entry_down = new EventTrigger.Entry();
		entry_down.eventID = EventTriggerType.PointerDown;
		EventTrigger.Entry entry_up = new EventTrigger.Entry();
		entry_up.eventID = EventTriggerType.PointerUp;
		switch (button.name.ToString()) {
			case "Forward Button":
				entry_down.callback.AddListener((x) => {
					push_forward = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_forward = false;
					MakeCommand();
				});
				break;
			case "Back Button":
				entry_down.callback.AddListener((x) => {
					push_back = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_back = false;
					MakeCommand();
				});
				break;
			case "Right Button":
				entry_down.callback.AddListener((x) => {
					push_right = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_right = false;
					MakeCommand();
				});
				break;
			case "Left Button":
				entry_down.callback.AddListener((x) => {
					push_left = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_left = false;
					MakeCommand();
				});
				break;
			case "Turn Right Button":
				entry_down.callback.AddListener((x) => {
					push_turnright = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_turnright = false;
					MakeCommand();
				});
				break;
			case "Turn Left Button":
				entry_down.callback.AddListener((x) => {
					push_turnleft = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_turnleft = false;
					MakeCommand();
				});
				break;
		}
		trigger.triggers.Add(entry_down);
		trigger.triggers.Add(entry_up);
	}

	// Update is called once per frame
	void Update() {
		//Vehicle周りの情報取得
		time_vehicle_state += Time.deltaTime;
		if (!cm.CheckWaitAnything() && time_vehicle_state > 0.5f) {
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
	}

	void MakeCommand() {
		//単押し
		if(!push_forward && !push_back && !push_right && !push_left && !push_turnright && !push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
		}
		else if (push_forward && !push_back && !push_right && !push_left && !push_turnright && !push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
			IEnumerator coroutine2 = SendMove(10, 0, 0);
			StartCoroutine(coroutine2);
		}
		else if (!push_forward && push_back && !push_right && !push_left && !push_turnright && !push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
			IEnumerator coroutine2 = SendMove(-10, 0, 0);
			StartCoroutine(coroutine2);
		}
		else if (!push_forward && !push_back && push_right && !push_left && !push_turnright && !push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
			IEnumerator coroutine2 = SendMove(0, -10, 0);
			StartCoroutine(coroutine2);
		}
		else if (!push_forward && !push_back && !push_right && push_left && !push_turnright && !push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
			IEnumerator coroutine2 = SendMove(0, 10, 0);
			StartCoroutine(coroutine2);
		}
		else if (!push_forward && !push_back && !push_right && !push_left && push_turnright && !push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
			IEnumerator coroutine2 = SendMove(0, 0, -10);
			StartCoroutine(coroutine2);
		}
		else if (!push_forward && !push_back && !push_right && !push_left && !push_turnright && push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
			IEnumerator coroutine2 = SendMove(0, 0, 10);
			StartCoroutine(coroutine2);
		}

		//2個押し
		else if (push_forward && push_back && !push_right && !push_left && !push_turnright && !push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
		}
		else if (push_forward && !push_back && push_right && !push_left && !push_turnright && !push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
			IEnumerator coroutine2 = SendMove(10, -10, 0);
			StartCoroutine(coroutine2);
		}
		else if (push_forward && !push_back && !push_right && push_left && !push_turnright && !push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
			IEnumerator coroutine2 = SendMove(10, 10, 0);
			StartCoroutine(coroutine2);
		}
		else if (push_forward && !push_back && !push_right && !push_left && push_turnright && !push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
			IEnumerator coroutine2 = SendMove(10, 0, -10);
			StartCoroutine(coroutine2);
		}
		else if (push_forward && !push_back && !push_right && !push_left && !push_turnright && push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
			IEnumerator coroutine2 = SendMove(10, 0, 10);
			StartCoroutine(coroutine2);
		}

		else if (!push_forward && push_back && push_right && !push_left && !push_turnright && !push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
			IEnumerator coroutine2 = SendMove(-10, -10, 0);
			StartCoroutine(coroutine2);
		}
		else if (!push_forward && push_back && !push_right && push_left && !push_turnright && !push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
			IEnumerator coroutine2 = SendMove(-10, 10, 0);
			StartCoroutine(coroutine2);
		}
		else if (!push_forward && push_back && !push_right && !push_left && push_turnright && !push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
			IEnumerator coroutine2 = SendMove(-10, 0, -10);
			StartCoroutine(coroutine2);
		}
		else if (!push_forward && push_back && !push_right && !push_left && !push_turnright && push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
			IEnumerator coroutine2 = SendMove(-10, 0, 10);
			StartCoroutine(coroutine2);
		}

		else if (!push_forward && !push_back && push_right && push_left && !push_turnright && !push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
		}
		else if (!push_forward && !push_back && push_right && !push_left && push_turnright && !push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
			IEnumerator coroutine2 = SendMove(0, -10, -10);
			StartCoroutine(coroutine2);
		}
		else if (!push_forward && !push_back && push_right && !push_left && !push_turnright && push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
			IEnumerator coroutine2 = SendMove(0, -10, 10);
			StartCoroutine(coroutine2);
		}

		else if (!push_forward && !push_back && !push_right && push_left && push_turnright && !push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
			IEnumerator coroutine2 = SendMove(0, 10, -10);
			StartCoroutine(coroutine2);
		}
		else if (!push_forward && !push_back && !push_right && push_left && !push_turnright && push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
			IEnumerator coroutine2 = SendMove(0, 10, 10);
			StartCoroutine(coroutine2);
		}

		else if (!push_forward && !push_back && !push_right && !push_left && push_turnright && push_turnleft) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
		}
	}

	void PushStop() {
		IEnumerator coroutine = SendStop();
		StartCoroutine(coroutine);
	}

	IEnumerator SendStop() {
		while (cm.CheckWaitAnything()) {
			yield return null;
		}

		IEnumerator coroutine = cm.VehicleStop();
		StartCoroutine(coroutine);

		while (cm.CheckWaitVehicleStop()) {
			if (cm.CheckAbort() || cm.CheckSuccess()) {
				cm.FinishAccess();
			}
			yield return null;
		}
	}

	IEnumerator SendMove(float x_m, float y_m, float theta_rad) {
		//Debug.Log("Move: " + x_m.ToString());
		while (cm.CheckWaitAnything()) {
			//Debug.Log("Waiting");
			yield return null;
		}

		IEnumerator coroutine = cm.VehicleMove(x_m, y_m, theta_rad);
		StartCoroutine(coroutine);

		while (cm.CheckWaitVehicleMove()) {
			if (cm.CheckAbort() || cm.CheckSuccess()) {
				cm.FinishAccess();
			}
			yield return null;
		}
	}

}
