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
	}

	void SettingButton(Button button) {
		EventTrigger trigger = button.GetComponent<EventTrigger>();
		EventTrigger.Entry entry_down = new EventTrigger.Entry();
		entry_down.eventID = EventTriggerType.PointerDown;
		EventTrigger.Entry entry_up = new EventTrigger.Entry();
		entry_up.eventID = EventTriggerType.PointerUp;
		switch (button.name.ToString()) {
			case "Forward Button":
				break;
			case "Back Button":
				break;
			case "Right Button":
				break;
			case "Left Button":
				break;
			case "Turn Right Button":
				break;
			case "Turn Left Button":
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
}
