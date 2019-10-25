using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RightArmCanvasManager : MonoBehaviour {

	//UIたち
	private Text RightArm_State_Text;

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

	// Start is called before the first frame update
	void Start() {
		RightArm_State_Text = GameObject.Find("Main System/Right Arm Canvas/Right Arm State Text").GetComponent<Text>();

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
	}

	void ButtonSetting(Button button) {
		EventTrigger trigger = button.GetComponent<EventTrigger>();
		EventTrigger.Entry entry_down = new EventTrigger.Entry();
		entry_down.eventID = EventTriggerType.PointerDown;
		EventTrigger.Entry entry_up = new EventTrigger.Entry();
		entry_up.eventID = EventTriggerType.PointerUp;
		switch (button.name.ToString()) {
			case "RJ1 up Button":
				break;
			case "RJ1 down Button":
				break;
			case "RJ2 up Button":
				break;
			case "RJ2 down Button":
				break;
			case "RJ3 up Button":
				break;
			case "RJ3 down Button":
				break;
			case "RJ4 up Button":
				break;
			case "RJ4 down Button":
				break;
			case "RJ5 up Button":
				break;
			case "RJ5 down Button":
				break;
			case "RJ6 up Button":
				break;
			case "RJ6 down Button":
				break;
			case "RJ7 up Button":
				break;
			case "RJ7 down Button":
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
