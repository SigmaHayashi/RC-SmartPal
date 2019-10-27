using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LeftArmCanvasManager : MonoBehaviour {

	//UIたち
	private Text LeftArm_State_Text;

	private Button LeftArm_J1Up_Button;
	private Button LeftArm_J1Down_Button;
	private Button LeftArm_J2Up_Button;
	private Button LeftArm_J2Down_Button;
	private Button LeftArm_J3Up_Button;
	private Button LeftArm_J3Down_Button;
	private Button LeftArm_J4Up_Button;
	private Button LeftArm_J4Down_Button;
	private Button LeftArm_J5Up_Button;
	private Button LeftArm_J5Down_Button;
	private Button LeftArm_J6Up_Button;
	private Button LeftArm_J6Down_Button;
	private Button LeftArm_J7Up_Button;
	private Button LeftArm_J7Down_Button;

	private Button LeftArm_Stop_Button;
	private Button LeftArm_Reset_Button;

	//Communication Manager
	private CommunicationManager cm;

	//更新するタイミング用
	private float time_leftarm_state = 0.0f;

	//どのボタンを押しているか
	private bool push_j1_up = false;
	private bool push_j1_down = false;
	private bool push_j2_up = false;
	private bool push_j2_down = false;
	private bool push_j3_up = false;
	private bool push_j3_down = false;
	private bool push_j4_up = false;
	private bool push_j4_down = false;
	private bool push_j5_up = false;
	private bool push_j5_down = false;
	private bool push_j6_up = false;
	private bool push_j6_down = false;
	private bool push_j7_up = false;
	private bool push_j7_down = false;

	// Start is called before the first frame update
	void Start() {
		LeftArm_State_Text = GameObject.Find("Main System/Left Arm Canvas/Left Arm State Text").GetComponent<Text>();

		LeftArm_J1Up_Button = GameObject.Find("Main System/Left Arm Canvas/LJ1 up Button").GetComponent<Button>();
		LeftArm_J1Down_Button = GameObject.Find("Main System/Left Arm Canvas/LJ1 down Button").GetComponent<Button>();
		LeftArm_J2Up_Button = GameObject.Find("Main System/Left Arm Canvas/LJ2 up Button").GetComponent<Button>();
		LeftArm_J2Down_Button = GameObject.Find("Main System/Left Arm Canvas/LJ2 down Button").GetComponent<Button>();
		LeftArm_J3Up_Button = GameObject.Find("Main System/Left Arm Canvas/LJ3 up Button").GetComponent<Button>();
		LeftArm_J3Down_Button = GameObject.Find("Main System/Left Arm Canvas/LJ3 down Button").GetComponent<Button>();
		LeftArm_J4Up_Button = GameObject.Find("Main System/Left Arm Canvas/LJ4 up Button").GetComponent<Button>();
		LeftArm_J4Down_Button = GameObject.Find("Main System/Left Arm Canvas/LJ4 down Button").GetComponent<Button>();
		LeftArm_J5Up_Button = GameObject.Find("Main System/Left Arm Canvas/LJ5 up Button").GetComponent<Button>();
		LeftArm_J5Down_Button = GameObject.Find("Main System/Left Arm Canvas/LJ5 down Button").GetComponent<Button>();
		LeftArm_J6Up_Button = GameObject.Find("Main System/Left Arm Canvas/LJ6 up Button").GetComponent<Button>();
		LeftArm_J6Down_Button = GameObject.Find("Main System/Left Arm Canvas/LJ6 down Button").GetComponent<Button>();
		LeftArm_J7Up_Button = GameObject.Find("Main System/Left Arm Canvas/LJ7 up Button").GetComponent<Button>();
		LeftArm_J7Down_Button = GameObject.Find("Main System/Left Arm Canvas/LJ7 down Button").GetComponent<Button>();

		LeftArm_Stop_Button = GameObject.Find("Main System/Left Arm Canvas/Stop Button").GetComponent<Button>();
		LeftArm_Reset_Button = GameObject.Find("Main System/Left Arm Canvas/Reset Button").GetComponent<Button>();

		ButtonSetting(LeftArm_J1Up_Button);
		ButtonSetting(LeftArm_J1Down_Button);
		ButtonSetting(LeftArm_J2Up_Button);
		ButtonSetting(LeftArm_J2Down_Button);
		ButtonSetting(LeftArm_J3Up_Button);
		ButtonSetting(LeftArm_J3Down_Button);
		ButtonSetting(LeftArm_J4Up_Button);
		ButtonSetting(LeftArm_J4Down_Button);
		ButtonSetting(LeftArm_J5Up_Button);
		ButtonSetting(LeftArm_J5Down_Button);
		ButtonSetting(LeftArm_J6Up_Button);
		ButtonSetting(LeftArm_J6Down_Button);
		ButtonSetting(LeftArm_J7Up_Button);
		ButtonSetting(LeftArm_J7Down_Button);

		LeftArm_Stop_Button.onClick.AddListener(PushStop);
		LeftArm_Reset_Button.onClick.AddListener(PushReset);

		cm = GameObject.Find("Communication Manager").GetComponent<CommunicationManager>();
	}

	void ButtonSetting(Button button) {
		EventTrigger trigger = button.GetComponent<EventTrigger>();
		EventTrigger.Entry entry_down = new EventTrigger.Entry();
		entry_down.eventID = EventTriggerType.PointerDown;
		EventTrigger.Entry entry_up = new EventTrigger.Entry();
		entry_up.eventID = EventTriggerType.PointerUp;
		switch (button.name.ToString()) {
			case "LJ1 up Button":
				entry_down.callback.AddListener((x) => {
					push_j1_up = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j1_up = false;
					MakeCommand();
				});
				break;
			case "LJ1 down Button":
				entry_down.callback.AddListener((x) => {
					push_j1_down = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j1_down = false;
					MakeCommand();
				});
				break;
			case "LJ2 up Button":
				entry_down.callback.AddListener((x) => {
					push_j2_up = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j2_up = false;
					MakeCommand();
				});
				break;
			case "LJ2 down Button":
				entry_down.callback.AddListener((x) => {
					push_j2_down = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j2_down = false;
					MakeCommand();
				});
				break;
			case "LJ3 up Button":
				entry_down.callback.AddListener((x) => {
					push_j3_up = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j3_up = false;
					MakeCommand();
				});
				break;
			case "LJ3 down Button":
				entry_down.callback.AddListener((x) => {
					push_j3_down = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j3_down = false;
					MakeCommand();
				});
				break;
			case "LJ4 up Button":
				entry_down.callback.AddListener((x) => {
					push_j4_up = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j4_up = false;
					MakeCommand();
				});
				break;
			case "LJ4 down Button":
				entry_down.callback.AddListener((x) => {
					push_j4_down = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j4_down = false;
					MakeCommand();
				});
				break;
			case "LJ5 up Button":
				entry_down.callback.AddListener((x) => {
					push_j5_up = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j5_up = false;
					MakeCommand();
				});
				break;
			case "LJ5 down Button":
				entry_down.callback.AddListener((x) => {
					push_j5_down = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j5_down = false;
					MakeCommand();
				});
				break;
			case "LJ6 up Button":
				entry_down.callback.AddListener((x) => {
					push_j6_up = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j6_up = false;
					MakeCommand();
				});
				break;
			case "LJ6 down Button":
				entry_down.callback.AddListener((x) => {
					push_j6_down = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j6_down = false;
					MakeCommand();
				});
				break;
			case "LJ7 up Button":
				entry_down.callback.AddListener((x) => {
					push_j7_up = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j7_up = false;
					MakeCommand();
				});
				break;
			case "LJ7 down Button":
				entry_down.callback.AddListener((x) => {
					push_j7_down = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j7_down = false;
					MakeCommand();
				});
				break;
		}
		trigger.triggers.Add(entry_down);
		trigger.triggers.Add(entry_up);
	}

	// Update is called once per frame
	void Update() {
		time_leftarm_state += Time.deltaTime;
		if (!cm.CheckWaitAnything() && time_leftarm_state > 0.5f) {
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
			}
		}
	}

	void MakeCommand() {
		if (!push_j1_up && !push_j1_down) {
			if (!push_j2_up && !push_j2_down) {
				if (!push_j3_up && !push_j3_down) {
					if (!push_j4_up && !push_j4_down) {
						if (!push_j5_up && !push_j5_down) {
							if (!push_j6_up && !push_j6_down) {
								if (!push_j7_up && !push_j7_down) {
									IEnumerator coroutine = SendStop();
									StartCoroutine(coroutine);
								}
							}
						}
					}
				}
			}
		}

		if (push_j1_up) {
			IEnumerator coroutine = SendMove(new float[7] { 5, 0, 0, 0, 0, 0, 0 });
			StartCoroutine(coroutine);
		}
		else if (push_j1_down) {
			IEnumerator coroutine = SendMove(new float[7] { -5, 0, 0, 0, 0, 0, 0 });
			StartCoroutine(coroutine);
		}
		else if (push_j2_up) {
			IEnumerator coroutine = SendMove(new float[7] { 0, 5, 0, 0, 0, 0, 0 });
			StartCoroutine(coroutine);
		}
		else if (push_j2_down) {
			IEnumerator coroutine = SendMove(new float[7] { 0, -5, 0, 0, 0, 0, 0 });
			StartCoroutine(coroutine);
		}
		else if (push_j3_up) {
			IEnumerator coroutine = SendMove(new float[7] { 0, 0, 5, 0, 0, 0, 0 });
			StartCoroutine(coroutine);
		}
		else if (push_j3_down) {
			IEnumerator coroutine = SendMove(new float[7] { 0, 0, -5, 0, 0, 0, 0 });
			StartCoroutine(coroutine);
		}
		else if (push_j4_up) {
			IEnumerator coroutine = SendMove(new float[7] { 0, 0, 0, 5, 0, 0, 0 });
			StartCoroutine(coroutine);
		}
		else if (push_j4_down) {
			IEnumerator coroutine = SendMove(new float[7] { 0, 0, 0, -5, 0, 0, 0 });
			StartCoroutine(coroutine);
		}
		else if (push_j5_up) {
			IEnumerator coroutine = SendMove(new float[7] { 0, 0, 0, 0, 5, 0, 0 });
			StartCoroutine(coroutine);
		}
		else if (push_j5_down) {
			IEnumerator coroutine = SendMove(new float[7] { 0, 0, 0, 0, -5, 0, 0 });
			StartCoroutine(coroutine);
		}
		else if (push_j6_up) {
			IEnumerator coroutine = SendMove(new float[7] { 0, 0, 0, 0, 0, 5, 0 });
			StartCoroutine(coroutine);
		}
		else if (push_j6_down) {
			IEnumerator coroutine = SendMove(new float[7] { 0, 0, 0, 0, 0, -5, 0 });
			StartCoroutine(coroutine);
		}
		else if (push_j7_up) {
			IEnumerator coroutine = SendMove(new float[7] { 0, 0, 0, 0, 0, 0, 5 });
			StartCoroutine(coroutine);
		}
		else if (push_j7_down) {
			IEnumerator coroutine = SendMove(new float[7] { 0, 0, 0, 0, 0, 0, -5 });
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

		IEnumerator coroutine = cm.LeftArmStop();
		StartCoroutine(coroutine);

		while (cm.CheckWaitLeftArmStop()) {
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

		IEnumerator coroutine = cm.LeftArmReset();
		StartCoroutine(coroutine);

		while (cm.CheckWaitLeftArmReset()) {
			if (cm.CheckAbort() || cm.CheckSuccess()) {
				cm.FinishAccess();
			}
			yield return null;
		}
	}

	IEnumerator SendMove(float[] arg) {
		while (cm.CheckWaitAnything()) {
			yield return null;
		}

		IEnumerator coroutine = cm.LeftArmMove(arg);
		StartCoroutine(coroutine);

		while (cm.CheckWaitLeftArmMove()) {
			if (cm.CheckAbort() || cm.CheckSuccess()) {
				cm.FinishAccess();
			}
			yield return null;
		}
	}
}
