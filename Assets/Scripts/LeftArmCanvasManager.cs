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
	}

	void ButtonSetting(Button button) {
		EventTrigger trigger = button.GetComponent<EventTrigger>();
		EventTrigger.Entry entry_down = new EventTrigger.Entry();
		entry_down.eventID = EventTriggerType.PointerDown;
		EventTrigger.Entry entry_up = new EventTrigger.Entry();
		entry_up.eventID = EventTriggerType.PointerUp;
		switch (button.name.ToString()) {
			case "LJ1 up Button":
				break;
			case "LJ1 down Button":
				break;
			case "LJ2 up Button":
				break;
			case "LJ2 down Button":
				break;
			case "LJ3 up Button":
				break;
			case "LJ3 down Button":
				break;
			case "LJ4 up Button":
				break;
			case "LJ4 down Button":
				break;
			case "LJ5 up Button":
				break;
			case "LJ5 down Button":
				break;
			case "LJ6 up Button":
				break;
			case "LJ6 down Button":
				break;
			case "LJ7 up Button":
				break;
			case "LJ7 down Button":
				break;
		}
		trigger.triggers.Add(entry_down);
		trigger.triggers.Add(entry_up);
	}

	// Update is called once per frame
	void Update() {

	}

	void PushStop() {

	}

	void PushReset() {

	}
}
