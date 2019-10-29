using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LeftGripperCanvasManager : MonoBehaviour {

	//UIたち
	private Text LeftGripper_State_Text;
	private Text LeftGripper_Pos_Text;

	private Button LeftGripper_Open_Button;
	private Button LeftGripper_Close_Button;

	private Button LeftGripper_Stop_Button;
	private Button LeftGripper_Reset_Button;

	//Communication Manager
	private CommunicationManager cm;

	//更新するタイミング用
	private float time_leftgripper_state = 0.0f;
	private float time_leftgripper_pos = 0.0f;

	//どのボタンを押しているか
	private bool push_open = false;
	private bool push_close = false;

	// Start is called before the first frame update
	void Start() {
		LeftGripper_State_Text = GameObject.Find("Main System/Left Gripper Canvas/Left Gripper State Text").GetComponent<Text>();
		LeftGripper_Pos_Text = GameObject.Find("Main System/Left Gripper Canvas/Left Gripper Pos Text").GetComponent<Text>();

		LeftGripper_Open_Button = GameObject.Find("Main System/Left Gripper Canvas/LG Open Button").GetComponent<Button>();
		LeftGripper_Close_Button = GameObject.Find("Main System/Left Gripper Canvas/LG Close Button").GetComponent<Button>();

		LeftGripper_Stop_Button = GameObject.Find("Main System/Left Gripper Canvas/Stop Button").GetComponent<Button>();
		LeftGripper_Reset_Button = GameObject.Find("Main System/Left Gripper Canvas/Reset Button").GetComponent<Button>();

		ButtonSetting(LeftGripper_Open_Button);
		ButtonSetting(LeftGripper_Close_Button);

		LeftGripper_Stop_Button.onClick.AddListener(PushStop);
		LeftGripper_Reset_Button.onClick.AddListener(PushReset);

		cm = GameObject.Find("Communication Manager").GetComponent<CommunicationManager>();
	}

	void ButtonSetting(Button button) {
		EventTrigger trigger = button.GetComponent<EventTrigger>();
		EventTrigger.Entry entry_down = new EventTrigger.Entry();
		entry_down.eventID = EventTriggerType.PointerDown;
		EventTrigger.Entry entry_up = new EventTrigger.Entry();
		entry_up.eventID = EventTriggerType.PointerUp;
		switch (button.name.ToString()) {
			case "LG Open Button":
				entry_down.callback.AddListener((x) => {
					push_open = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_open = false;
					MakeCommand();
				});
				break;
			case "LG Close Button":
				entry_down.callback.AddListener((x) => {
					push_close = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_close = false;
					MakeCommand();
				});
				break;
		}
		trigger.triggers.Add(entry_down);
		trigger.triggers.Add(entry_up);
	}

	// Update is called once per frame
	void Update() {
		time_leftgripper_state += Time.deltaTime;
		if (!cm.CheckWaitAnything() && time_leftgripper_state > 0.5f) {
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
		if (!cm.CheckWaitAnything() && time_leftgripper_pos > 0.5f) {
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

				LeftGripper_Pos_Text.text = "Joint: (" + (responce.values.val[0] * Mathf.Rad2Deg).ToString("f0") + ")";
			}
		}
	}

	void MakeCommand() {
		if (!push_open && !push_close) {
			IEnumerator coroutine = SendStop();
			StartCoroutine(coroutine);
		}
		else if (push_open) {
			IEnumerator coroutine = SendMove(-1.0f);
			StartCoroutine(coroutine);
		}
		else if (push_close) {
			IEnumerator coroutine = SendMove(-0.2f);
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

		IEnumerator coroutine = cm.LeftGripperStop();
		StartCoroutine(coroutine);

		while (cm.CheckWaitLeftGripperStop()) {
			if (cm.CheckAbort() || cm.CheckSuccess()) {
				cm.FinishAccess();
			}
			yield return null;
		}
	}

	void PushReset() {
		IEnumerator coroutine = SendReset();
		StartCoroutine(coroutine);
	}

	IEnumerator SendReset() {
		while (cm.CheckWaitAnything()) {
			yield return null;
		}

		IEnumerator coroutine = cm.LeftGripperMove(-0.2f);
		StartCoroutine(coroutine);

		while (cm.CheckWaitLeftGripperMove()) {
			if (cm.CheckAbort() || cm.CheckSuccess()) {
				cm.FinishAccess();
			}
			yield return null;
		}
	}

	IEnumerator SendMove(float theta_rad) {
		while (cm.CheckWaitAnything()) {
			yield return null;
		}

		IEnumerator coroutine = cm.LeftGripperMove(theta_rad);
		StartCoroutine(coroutine);

		while (cm.CheckWaitLeftGripperMove()) {
			if (cm.CheckAbort() || cm.CheckSuccess()) {
				cm.FinishAccess();
			}
			yield return null;
		}
	}
}
