using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainScript : MonoBehaviour {

	//スクリーンが消えないようにする
	public bool ScreenNOTSleep = true;

	//Canvasたち
	private GameObject InfoCanvas;
	private GameObject VehicleCanvas;
	private GameObject RightArmCanvas;
	private GameObject LeftArmCanvas;
	private GameObject RightGripperCanvas;
	private GameObject LeftGripperCanvas;

	//Canvasを遷移させるボタンたち
	private Button Info_ChangeToVehicle_Button;
	private Button Info_ChangeToRightArm_Button;
	private Button Info_ChangeToLeftArm_Button;
	private Button Info_ChangeToRightGripper_Button;
	private Button Info_ChangeToLeftGripper_Button;

	private Button Vehicle_ChangeToInfo_Button;
	private Button Vehicle_ChengeToRightArm_Button;
	private Button Vehicle_ChengeToLeftArm_Button;
	private Button Vehicle_ChangeToRightGripper_Button;
	private Button Vehicle_ChangeToLeftGripper_Button;

	private Button RightArm_ChangeToInfo_Button;
	private Button RightArm_ChengeToVehicle_Button;
	private Button RightArm_ChengeToLeftArm_Button;
	private Button RightArm_ChangeToRightGripper_Button;
	private Button RightArm_ChangeToLeftGripper_Button;

	private Button LeftArm_ChangeToInfo_Button;
	private Button LeftArm_ChengeToVehicle_Button;
	private Button LeftArm_ChengeToRightArm_Button;
	private Button LeftArm_ChangeToRightGripper_Button;
	private Button LeftArm_ChangeToLeftGripper_Button;

	private Button RightGripper_ChangeToInfo_Button;
	private Button RightGripper_ChangeToVehicle_Button;
	private Button RightGripper_ChangeToRightArm_Button;
	private Button RightGripper_ChangeToLeftArm_Button;
	private Button RightGripper_ChangeToLeftGripper_Button;

	private Button LeftGripper_ChangeToInfo_Button;
	private Button LeftGripper_ChangeToVehicle_Button;
	private Button LeftGripper_ChangeToRightArm_Button;
	private Button LeftGripper_ChangeToLeftArm_Button;
	private Button LeftGripper_ChangeToRightGripper_Button;

	//いまどのCanvasを使用中か示す変数・それに対応する辞書
	private int CanvasState = 0;
	private Dictionary<int, GameObject> CanvasDictionary = new Dictionary<int, GameObject>();

	private CommunicationManager cm;

	// Start is called before the first frame update
	void Start() {
		//画面が消えないようにする
		if (ScreenNOTSleep) {
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
		}
		else {
			Screen.sleepTimeout = SleepTimeout.SystemSetting;
		}

		//Canvasを取得・辞書に追加
		InfoCanvas = GameObject.Find("Main System/Info Canvas");
		VehicleCanvas = GameObject.Find("Main System/Vehicle Canvas");
		RightArmCanvas = GameObject.Find("Main System/Right Arm Canvas");
		LeftArmCanvas = GameObject.Find("Main System/Left Arm Canvas");
		RightGripperCanvas = GameObject.Find("Main System/Right Gripper Canvas");
		LeftGripperCanvas = GameObject.Find("Main System/Left Gripper Canvas");
		CanvasDictionary.Add(0, InfoCanvas);
		CanvasDictionary.Add(1, VehicleCanvas);
		CanvasDictionary.Add(2, RightArmCanvas);
		CanvasDictionary.Add(3, LeftArmCanvas);
		CanvasDictionary.Add(4, RightGripperCanvas);
		CanvasDictionary.Add(5, LeftGripperCanvas);

		//Canvas移動用ボタンを取得・設定
		Info_ChangeToVehicle_Button = GameObject.Find("Main System/Info Canvas/Change to Vehicle Button").GetComponent<Button>();
		Info_ChangeToRightArm_Button = GameObject.Find("Main System/Info Canvas/Change to Right Arm Button").GetComponent<Button>();
		Info_ChangeToLeftArm_Button = GameObject.Find("Main System/Info Canvas/Change to Left Arm Button").GetComponent<Button>();
		Info_ChangeToRightGripper_Button = GameObject.Find("Main System/Info Canvas/Change to Right Gripper Button").GetComponent<Button>();
		Info_ChangeToLeftGripper_Button = GameObject.Find("Main System/Info Canvas/Change to Left Gripper Button").GetComponent<Button>();
		Info_ChangeToVehicle_Button.onClick.AddListener(ChangeToVehicle);
		Info_ChangeToRightArm_Button.onClick.AddListener(ChangeToRightArm);
		Info_ChangeToLeftArm_Button.onClick.AddListener(ChangeToLeftArm);
		Info_ChangeToRightGripper_Button.onClick.AddListener(ChangeToRightGripper);
		Info_ChangeToLeftGripper_Button.onClick.AddListener(ChangeToLeftGripper);

		Vehicle_ChangeToInfo_Button = GameObject.Find("Main System/Vehicle Canvas/Change to Info Button").GetComponent<Button>();
		Vehicle_ChengeToRightArm_Button = GameObject.Find("Main System/Vehicle Canvas/Change to Right Arm Button").GetComponent<Button>();
		Vehicle_ChengeToLeftArm_Button = GameObject.Find("Main System/Vehicle Canvas/Change to Left Arm Button").GetComponent<Button>();
		Vehicle_ChangeToRightGripper_Button = GameObject.Find("Main System/Vehicle Canvas/Change to Right Gripper Button").GetComponent<Button>();
		Vehicle_ChangeToLeftGripper_Button = GameObject.Find("Main System/Vehicle Canvas/Change to Left Gripper Button").GetComponent<Button>();
		Vehicle_ChangeToInfo_Button.onClick.AddListener(ChangeToInfo);
		Vehicle_ChengeToRightArm_Button.onClick.AddListener(ChangeToRightArm);
		Vehicle_ChengeToLeftArm_Button.onClick.AddListener(ChangeToLeftArm);
		Vehicle_ChangeToRightGripper_Button.onClick.AddListener(ChangeToRightGripper);
		Vehicle_ChangeToLeftGripper_Button.onClick.AddListener(ChangeToLeftGripper);

		RightArm_ChangeToInfo_Button = GameObject.Find("Main System/Right Arm Canvas/Change to Info Button").GetComponent<Button>();
		RightArm_ChengeToVehicle_Button = GameObject.Find("Main System/Right Arm Canvas/Change to Vehicle Button").GetComponent<Button>();
		RightArm_ChengeToLeftArm_Button = GameObject.Find("Main System/Right Arm Canvas/Change to Left Arm Button").GetComponent<Button>();
		RightArm_ChangeToRightGripper_Button = GameObject.Find("Main System/Right Arm Canvas/Change to Right Gripper Button").GetComponent<Button>();
		RightArm_ChangeToLeftGripper_Button = GameObject.Find("Main System/Right Arm Canvas/Change to Left Gripper Button").GetComponent<Button>();
		RightArm_ChangeToInfo_Button.onClick.AddListener(ChangeToInfo);
		RightArm_ChengeToVehicle_Button.onClick.AddListener(ChangeToVehicle);
		RightArm_ChengeToLeftArm_Button.onClick.AddListener(ChangeToLeftArm);
		RightArm_ChangeToRightGripper_Button.onClick.AddListener(ChangeToRightGripper);
		RightArm_ChangeToLeftGripper_Button.onClick.AddListener(ChangeToLeftGripper);

		LeftArm_ChangeToInfo_Button = GameObject.Find("Main System/Left Arm Canvas/Change to Info Button").GetComponent<Button>();
		LeftArm_ChengeToVehicle_Button = GameObject.Find("Main System/Left Arm Canvas/Change to Vehicle Button").GetComponent<Button>();
		LeftArm_ChengeToRightArm_Button = GameObject.Find("Main System/Left Arm Canvas/Change to Right Arm Button").GetComponent<Button>();
		LeftArm_ChangeToRightGripper_Button = GameObject.Find("Main System/Left Arm Canvas/Change to Right Gripper Button").GetComponent<Button>();
		LeftArm_ChangeToLeftGripper_Button = GameObject.Find("Main System/Left Arm Canvas/Change to Left Gripper Button").GetComponent<Button>();
		LeftArm_ChangeToInfo_Button.onClick.AddListener(ChangeToInfo);
		LeftArm_ChengeToVehicle_Button.onClick.AddListener(ChangeToVehicle);
		LeftArm_ChengeToRightArm_Button.onClick.AddListener(ChangeToRightArm);
		LeftArm_ChangeToRightGripper_Button.onClick.AddListener(ChangeToRightGripper);
		LeftArm_ChangeToLeftGripper_Button.onClick.AddListener(ChangeToLeftGripper);

		RightGripper_ChangeToInfo_Button = GameObject.Find("Main System/Right Gripper Canvas/Change to Info Button").GetComponent<Button>();
		RightGripper_ChangeToVehicle_Button = GameObject.Find("Main System/Right Gripper Canvas/Change to Vehicle Button").GetComponent<Button>();
		RightGripper_ChangeToRightArm_Button = GameObject.Find("Main System/Right Gripper Canvas/Change to Right Arm Button").GetComponent<Button>();
		RightGripper_ChangeToLeftArm_Button = GameObject.Find("Main System/Right Gripper Canvas/Change to Left Arm Button").GetComponent<Button>();
		RightGripper_ChangeToLeftGripper_Button = GameObject.Find("Main System/Right Gripper Canvas/Change to Left Gripper Button").GetComponent<Button>();
		RightGripper_ChangeToInfo_Button.onClick.AddListener(ChangeToInfo);
		RightGripper_ChangeToVehicle_Button.onClick.AddListener(ChangeToVehicle);
		RightGripper_ChangeToRightArm_Button.onClick.AddListener(ChangeToRightArm);
		RightGripper_ChangeToLeftArm_Button.onClick.AddListener(ChangeToLeftArm);
		RightGripper_ChangeToLeftGripper_Button.onClick.AddListener(ChangeToLeftGripper);

		LeftGripper_ChangeToInfo_Button = GameObject.Find("Main System/Left Gripper Canvas/Change to Info Button").GetComponent<Button>();
		LeftGripper_ChangeToVehicle_Button = GameObject.Find("Main System/Left Gripper Canvas/Change to Vehicle Button").GetComponent<Button>();
		LeftGripper_ChangeToRightArm_Button = GameObject.Find("Main System/Left Gripper Canvas/Change to Right Arm Button").GetComponent<Button>();
		LeftGripper_ChangeToLeftArm_Button = GameObject.Find("Main System/Left Gripper Canvas/Change to Left Arm Button").GetComponent<Button>();
		LeftGripper_ChangeToRightGripper_Button = GameObject.Find("Main System/Left Gripper Canvas/Change to Right Gripper Button").GetComponent<Button>();
		LeftGripper_ChangeToInfo_Button.onClick.AddListener(ChangeToInfo);
		LeftGripper_ChangeToVehicle_Button.onClick.AddListener(ChangeToVehicle);
		LeftGripper_ChangeToRightArm_Button.onClick.AddListener(ChangeToRightArm);
		LeftGripper_ChangeToLeftArm_Button.onClick.AddListener(ChangeToLeftArm);
		LeftGripper_ChangeToRightGripper_Button.onClick.AddListener(ChangeToRightGripper);

		//CanvasをInfoCanvasのみに設定
		CanvasState = 0;
		VehicleCanvas.SetActive(false);
		RightArmCanvas.SetActive(false);
		LeftArmCanvas.SetActive(false);
		RightGripperCanvas.SetActive(false);
		LeftGripperCanvas.SetActive(false);

		cm = GameObject.Find("Communication Manager").GetComponent<CommunicationManager>();
	}


	// Update is called once per frame
	void Update() {
		//戻るボタンでアプリ終了
		if (Input.GetKey(KeyCode.Escape)) {
			Application.Quit();
		}
	}


	/**************************************************
	 * キャンバス切り替え
	 **************************************************/
	void ChangeToInfo() {
		IEnumerator coroutine = WaitForChangeToInfo();
		StartCoroutine(coroutine);
	}

	void ChangeToVehicle() {
		IEnumerator coroutine = WaitForChangeToVehicle();
		StartCoroutine(coroutine);
	}

	void ChangeToRightArm() {
		IEnumerator coroutine = WaitForChangeToRightArm();
		StartCoroutine(coroutine);
	}

	void ChangeToLeftArm() {
		IEnumerator coroutine = WaitForChangeToLeftArm();
		StartCoroutine(coroutine);
	}

	void ChangeToRightGripper() {
		IEnumerator coroutine = WaitForChangeToRightGripper();
		StartCoroutine(coroutine);
	}

	void ChangeToLeftGripper() {
		IEnumerator coroutine = WaitForChangeToLeftGripper();
		StartCoroutine(coroutine);
	}

	IEnumerator WaitForChangeToInfo() {
		while (cm.CheckWaitAnything()) {
			yield return null;
		}
		CanvasDictionary[CanvasState].SetActive(false);
		InfoCanvas.SetActive(true);
		CanvasState = 0;
	}

	IEnumerator WaitForChangeToVehicle() {
		while (cm.CheckWaitAnything()) {
			yield return null;
		}
		CanvasDictionary[CanvasState].SetActive(false);
		VehicleCanvas.SetActive(true);
		CanvasState = 1;
	}

	IEnumerator WaitForChangeToRightArm() {
		while (cm.CheckWaitAnything()) {
			yield return null;
		}
		CanvasDictionary[CanvasState].SetActive(false);
		RightArmCanvas.SetActive(true);
		CanvasState = 2;
	}

	IEnumerator WaitForChangeToLeftArm() {
		while (cm.CheckWaitAnything()) {
			yield return null;
		}
		CanvasDictionary[CanvasState].SetActive(false);
		LeftArmCanvas.SetActive(true);
		CanvasState = 3;
	}

	IEnumerator WaitForChangeToRightGripper() {
		while (cm.CheckWaitAnything()) {
			yield return null;
		}
		CanvasDictionary[CanvasState].SetActive(false);
		RightGripperCanvas.SetActive(true);
		CanvasState = 4;
	}

	IEnumerator WaitForChangeToLeftGripper() {
		while (cm.CheckWaitAnything()) {
			yield return null;
		}
		CanvasDictionary[CanvasState].SetActive(false);
		LeftGripperCanvas.SetActive(true);
		CanvasState = 5;
	}

	/**************************************************
	 * どのキャンバスを使用中か返す
	 **************************************************/
	public KeyValuePair<int, string> WhichCanvasActive() {
		return new KeyValuePair<int, string>(CanvasState, CanvasDictionary[CanvasState].name);
	}
}
