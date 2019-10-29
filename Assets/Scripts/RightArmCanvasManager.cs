using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RightArmCanvasManager : MonoBehaviour {

	//UIたち
	private Text RightArm_State_Text;
	private Text RightArm_Pos_Text;

	private Button RightArm_J1Up_Button;
	private Button RightArm_J1Down_Button;
	private Button RightArm_J2Up_Button;
	private Button RightArm_J2Down_Button;
	private Button RightArm_J3Up_Button;
	private Button RightArm_J3Down_Button;
	private Button RightArm_J4Up_Button;
	private Button RightArm_J4Down_Button;
	private Button RightArm_J5Up_Button;
	private Button RightArm_J5Down_Button;
	private Button RightArm_J6Up_Button;
	private Button RightArm_J6Down_Button;
	private Button RightArm_J7Up_Button;
	private Button RightArm_J7Down_Button;

	private Button RightArm_Stop_Button;
	private Button RightArm_Reset_Button;

	//Communication Manager
	private CommunicationManager cm;

	//更新するタイミング用
	private float time_rightarm_state = 0.0f;
	private float time_rightarm_pos = 0.0f;

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
		RightArm_State_Text = GameObject.Find("Main System/Right Arm Canvas/Right Arm State Text").GetComponent<Text>();
		RightArm_Pos_Text = GameObject.Find("Main System/Right Arm Canvas/Right Arm Pos Text").GetComponent<Text>();

		RightArm_J1Up_Button = GameObject.Find("Main System/Right Arm Canvas/RJ1 up Button").GetComponent<Button>();
		RightArm_J1Down_Button = GameObject.Find("Main System/Right Arm Canvas/RJ1 down Button").GetComponent<Button>();
		RightArm_J2Up_Button = GameObject.Find("Main System/Right Arm Canvas/RJ2 up Button").GetComponent<Button>();
		RightArm_J2Down_Button = GameObject.Find("Main System/Right Arm Canvas/RJ2 down Button").GetComponent<Button>();
		RightArm_J3Up_Button = GameObject.Find("Main System/Right Arm Canvas/RJ3 up Button").GetComponent<Button>();
		RightArm_J3Down_Button = GameObject.Find("Main System/Right Arm Canvas/RJ3 down Button").GetComponent<Button>();
		RightArm_J4Up_Button = GameObject.Find("Main System/Right Arm Canvas/RJ4 up Button").GetComponent<Button>();
		RightArm_J4Down_Button = GameObject.Find("Main System/Right Arm Canvas/RJ4 down Button").GetComponent<Button>();
		RightArm_J5Up_Button = GameObject.Find("Main System/Right Arm Canvas/RJ5 up Button").GetComponent<Button>();
		RightArm_J5Down_Button = GameObject.Find("Main System/Right Arm Canvas/RJ5 down Button").GetComponent<Button>();
		RightArm_J6Up_Button = GameObject.Find("Main System/Right Arm Canvas/RJ6 up Button").GetComponent<Button>();
		RightArm_J6Down_Button = GameObject.Find("Main System/Right Arm Canvas/RJ6 down Button").GetComponent<Button>();
		RightArm_J7Up_Button = GameObject.Find("Main System/Right Arm Canvas/RJ7 up Button").GetComponent<Button>();
		RightArm_J7Down_Button = GameObject.Find("Main System/Right Arm Canvas/RJ7 down Button").GetComponent<Button>();

		RightArm_Stop_Button = GameObject.Find("Main System/Right Arm Canvas/Stop Button").GetComponent<Button>();
		RightArm_Reset_Button = GameObject.Find("Main System/Right Arm Canvas/Reset Button").GetComponent<Button>();

		ButtonSetting(RightArm_J1Up_Button);
		ButtonSetting(RightArm_J1Down_Button);
		ButtonSetting(RightArm_J2Up_Button);
		ButtonSetting(RightArm_J2Down_Button);
		ButtonSetting(RightArm_J3Up_Button);
		ButtonSetting(RightArm_J3Down_Button);
		ButtonSetting(RightArm_J4Up_Button);
		ButtonSetting(RightArm_J4Down_Button);
		ButtonSetting(RightArm_J5Up_Button);
		ButtonSetting(RightArm_J5Down_Button);
		ButtonSetting(RightArm_J6Up_Button);
		ButtonSetting(RightArm_J6Down_Button);
		ButtonSetting(RightArm_J7Up_Button);
		ButtonSetting(RightArm_J7Down_Button);

		RightArm_Stop_Button.onClick.AddListener(PushStop);
		RightArm_Reset_Button.onClick.AddListener(PushReset);

		cm = GameObject.Find("Communication Manager").GetComponent<CommunicationManager>();
	}

	void ButtonSetting(Button button) {
		EventTrigger trigger = button.GetComponent<EventTrigger>();
		EventTrigger.Entry entry_down = new EventTrigger.Entry();
		entry_down.eventID = EventTriggerType.PointerDown;
		EventTrigger.Entry entry_up = new EventTrigger.Entry();
		entry_up.eventID = EventTriggerType.PointerUp;
		switch (button.name.ToString()) {
			case "RJ1 up Button":
				entry_down.callback.AddListener((x) => {
					push_j1_up = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j1_up = false;
					MakeCommand();
				});
				break;
			case "RJ1 down Button":
				entry_down.callback.AddListener((x) => {
					push_j1_down = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j1_down = false;
					MakeCommand();
				});
				break;
			case "RJ2 up Button":
				entry_down.callback.AddListener((x) => {
					push_j2_up = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j2_up = false;
					MakeCommand();
				});
				break;
			case "RJ2 down Button":
				entry_down.callback.AddListener((x) => {
					push_j2_down = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j2_down = false;
					MakeCommand();
				});
				break;
			case "RJ3 up Button":
				entry_down.callback.AddListener((x) => {
					push_j3_up = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j3_up = false;
					MakeCommand();
				});
				break;
			case "RJ3 down Button":
				entry_down.callback.AddListener((x) => {
					push_j3_down = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j3_down = false;
					MakeCommand();
				});
				break;
			case "RJ4 up Button":
				entry_down.callback.AddListener((x) => {
					push_j4_up = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j4_up = false;
					MakeCommand();
				});
				break;
			case "RJ4 down Button":
				entry_down.callback.AddListener((x) => {
					push_j4_down = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j4_down = false;
					MakeCommand();
				});
				break;
			case "RJ5 up Button":
				entry_down.callback.AddListener((x) => {
					push_j5_up = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j5_up = false;
					MakeCommand();
				});
				break;
			case "RJ5 down Button":
				entry_down.callback.AddListener((x) => {
					push_j5_down = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j5_down = false;
					MakeCommand();
				});
				break;
			case "RJ6 up Button":
				entry_down.callback.AddListener((x) => {
					push_j6_up = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j6_up = false;
					MakeCommand();
				});
				break;
			case "RJ6 down Button":
				entry_down.callback.AddListener((x) => {
					push_j6_down = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j6_down = false;
					MakeCommand();
				});
				break;
			case "RJ7 up Button":
				entry_down.callback.AddListener((x) => {
					push_j7_up = true;
					MakeCommand();
				});
				entry_up.callback.AddListener((x) => {
					push_j7_up = false;
					MakeCommand();
				});
				break;
			case "RJ7 down Button":
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
		time_rightarm_state += Time.deltaTime;
		if (!cm.CheckWaitAnything() && time_rightarm_state > 0.5f) {
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
					float val_tmp = val * Mathf.Rad2Deg;
					val_string += val_tmp.ToString("f0") + ", ";
				}
				//Debug.Log(val_string.Length);
				if (val_string.Length > 1) {
					val_string = val_string.Substring(0, val_string.Length - 2);
				}
				val_string += ")";
				RightArm_Pos_Text.text = "Pos: " + val_string;
			}
		}
	}

	void MakeCommand() {
		if(!push_j1_up && !push_j1_down) {
			if(!push_j2_up && !push_j2_down) {
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

		IEnumerator coroutine = cm.RightArmStop();
		StartCoroutine(coroutine);

		while (cm.CheckWaitRightArmStop()) {
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

		IEnumerator coroutine = cm.RightArmReset();
		StartCoroutine(coroutine);

		while (cm.CheckWaitRightArmReset()) {
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

		IEnumerator coroutine = cm.RightArmMove(arg);
		StartCoroutine(coroutine);

		while (cm.CheckWaitRightArmMove()) {
			if (cm.CheckAbort() || cm.CheckSuccess()) {
				cm.FinishAccess();
			}
			yield return null;
		}
	}
}
