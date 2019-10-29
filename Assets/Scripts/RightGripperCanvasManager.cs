using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RightGripperCanvasManager : MonoBehaviour {

	//UIたち
	private Text RightGripper_State_Text;
	private Text RightGripper_Pos_Text;

	private Button RightGripper_Open_Button;
	private Button RightGripper_Close_Button;

	private Button RightGripper_Stop_Button;
	private Button RightGripper_Reset_Button;

	//Communication Manager
	private CommunicationManager cm;

	//更新するタイミング用
	private float time_rightgripper_state = 0.0f;
	private float time_rightgripper_pos = 0.0f;

	//どのボタンを押しているか
	private bool push_open = false;
	private bool push_close = false;

	// Start is called before the first frame update
	void Start() {
		RightGripper_State_Text = GameObject.Find("Main System/Right Gripper Canvas/Right Gripper State Text").GetComponent<Text>();
		RightGripper_Pos_Text = GameObject.Find("Main System/Right Gripper Canvas/Right Gripper Pos Text").GetComponent<Text>();

		RightGripper_Open_Button = GameObject.Find("Main System/Right Gripper Canvas/RG Open Button").GetComponent<Button>();
		RightGripper_Close_Button = GameObject.Find("Main System/Right Gripper Canvas/RG Close Button").GetComponent<Button>();

		RightGripper_Stop_Button = GameObject.Find("Main System/Right Gripper Canvas/Stop Button").GetComponent<Button>();
		RightGripper_Reset_Button = GameObject.Find("Main System/Right Gripper Canvas/Reset Button").GetComponent<Button>();

		ButtonSetting(RightGripper_Open_Button);
		ButtonSetting(RightGripper_Close_Button);

		RightGripper_Stop_Button.onClick.AddListener(PushStop);
		RightGripper_Reset_Button.onClick.AddListener(PushReset);

		cm = GameObject.Find("Communication Manager").GetComponent<CommunicationManager>();
	}

	void ButtonSetting(Button button) {
		EventTrigger trigger = button.GetComponent<EventTrigger>();
		EventTrigger.Entry entry_down = new EventTrigger.Entry();
		entry_down.eventID = EventTriggerType.PointerDown;
		EventTrigger.Entry entry_up = new EventTrigger.Entry();
		entry_up.eventID = EventTriggerType.PointerUp;
		switch (button.name.ToString()) {
			case "RG Open Button":
				entry_down.callback.AddListener((x) => {
					push_open = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_open = false;
					MakeCommand();
				});
				break;
			case "RG Close Button":
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
		time_rightgripper_state += Time.deltaTime;
		if (!cm.CheckWaitAnything() && time_rightgripper_state > 0.5f) {
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
		if (!cm.CheckWaitAnything() && time_rightgripper_pos > 0.5f) {
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

				RightGripper_Pos_Text.text = "Joint: (" + responce.values.val[0].ToString("f2") + ")";
			}
		}
	}

	void MakeCommand() {
		if(!push_open && !push_close) {
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

		IEnumerator coroutine = cm.RightGripperStop();
		StartCoroutine(coroutine);

		while (cm.CheckWaitRightGripperStop()) {
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

		IEnumerator coroutine = cm.RightGripperMove(-0.2f);
		StartCoroutine(coroutine);

		while (cm.CheckWaitRightGripperMove()) {
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

		IEnumerator coroutine = cm.RightGripperMove(theta_rad);
		StartCoroutine(coroutine);

		while (cm.CheckWaitRightGripperMove()) {
			if (cm.CheckAbort() || cm.CheckSuccess()) {
				cm.FinishAccess();
			}
			yield return null;
		}
	}
}
