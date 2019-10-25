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

	//Canvasを遷移させるボタンたち
	private Button Info_ChangeToVehicle_Button;
	private Button Info_ChangeToRightArm_Button;
	private Button Info_ChangeToLeftArm_Button;

	private Button Vehicle_ChangeToInfo_Button;
	private Button Vehicle_ChengeToRightArm_Button;
	private Button Vehicle_ChengeToLeftArm_Button;

	private Button RightArm_ChangeToInfo_Button;
	private Button RightArm_ChengeToVehicle_Button;
	private Button RightArm_ChengeToLeftArm_Button;

	private Button LeftArm_ChangeToInfo_Button;
	private Button LeftArm_ChengeToVehicle_Button;
	private Button LeftArm_ChengeToRightArm_Button;

	//いまどのCanvasを使用中か示す変数・それに対応する辞書
	private int CanvasState = 0;
	private Dictionary<int, GameObject> CanvasDictionary = new Dictionary<int, GameObject>();


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
		CanvasDictionary.Add(0, InfoCanvas);
		CanvasDictionary.Add(1, VehicleCanvas);
		CanvasDictionary.Add(2, RightArmCanvas);
		CanvasDictionary.Add(3, LeftArmCanvas);

		//Canvas移動用ボタンを取得・設定
		Info_ChangeToVehicle_Button = GameObject.Find("Main System/Info Canvas/Change to Vehicle Button").GetComponent<Button>();
		Info_ChangeToRightArm_Button = GameObject.Find("Main System/Info Canvas/Change to Right Arm Button").GetComponent<Button>();
		Info_ChangeToLeftArm_Button = GameObject.Find("Main System/Info Canvas/Change to Left Arm Button").GetComponent<Button>();
		Info_ChangeToVehicle_Button.onClick.AddListener(ChangeToVehicle);
		Info_ChangeToRightArm_Button.onClick.AddListener(ChangeToRightArm);
		Info_ChangeToLeftArm_Button.onClick.AddListener(ChangeToLeftArm);

		Vehicle_ChangeToInfo_Button = GameObject.Find("Main System/Vehicle Canvas/Change to Info Button").GetComponent<Button>();
		Vehicle_ChengeToRightArm_Button = GameObject.Find("Main System/Vehicle Canvas/Change to Right Arm Button").GetComponent<Button>();
		Vehicle_ChengeToLeftArm_Button = GameObject.Find("Main System/Vehicle Canvas/Change to Left Arm Button").GetComponent<Button>();
		Vehicle_ChangeToInfo_Button.onClick.AddListener(ChangeToInfo);
		Vehicle_ChengeToRightArm_Button.onClick.AddListener(ChangeToRightArm);
		Vehicle_ChengeToLeftArm_Button.onClick.AddListener(ChangeToLeftArm);

		RightArm_ChangeToInfo_Button = GameObject.Find("Main System/Right Arm Canvas/Change to Info Button").GetComponent<Button>();
		RightArm_ChengeToVehicle_Button = GameObject.Find("Main System/Right Arm Canvas/Change to Vehicle Button").GetComponent<Button>();
		RightArm_ChengeToLeftArm_Button = GameObject.Find("Main System/Right Arm Canvas/Change to Left Arm Button").GetComponent<Button>();
		RightArm_ChangeToInfo_Button.onClick.AddListener(ChangeToInfo);
		RightArm_ChengeToVehicle_Button.onClick.AddListener(ChangeToVehicle);
		RightArm_ChengeToLeftArm_Button.onClick.AddListener(ChangeToLeftArm);

		LeftArm_ChangeToInfo_Button = GameObject.Find("Main System/Left Arm Canvas/Change to Info Button").GetComponent<Button>();
		LeftArm_ChengeToVehicle_Button = GameObject.Find("Main System/Left Arm Canvas/Change to Vehicle Button").GetComponent<Button>();
		LeftArm_ChengeToRightArm_Button = GameObject.Find("Main System/Left Arm Canvas/Change to Right Arm Button").GetComponent<Button>();
		LeftArm_ChangeToInfo_Button.onClick.AddListener(ChangeToInfo);
		LeftArm_ChengeToVehicle_Button.onClick.AddListener(ChangeToVehicle);
		LeftArm_ChengeToRightArm_Button.onClick.AddListener(ChangeToRightArm);

		//CanvasをInfoCanvasのみに設定
		CanvasState = 0;
		VehicleCanvas.SetActive(false);
		RightArmCanvas.SetActive(false);
		LeftArmCanvas.SetActive(false);
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
		CanvasDictionary[CanvasState].SetActive(false);
		InfoCanvas.SetActive(true);
		CanvasState = 0;
	}

	void ChangeToVehicle() {
		CanvasDictionary[CanvasState].SetActive(false);
		VehicleCanvas.SetActive(true);
		CanvasState = 1;
	}

	void ChangeToRightArm() {
		CanvasDictionary[CanvasState].SetActive(false);
		RightArmCanvas.SetActive(true);
		CanvasState = 2;
	}

	void ChangeToLeftArm() {
		CanvasDictionary[CanvasState].SetActive(false);
		LeftArmCanvas.SetActive(true);
		CanvasState = 3;
	}

	/**************************************************
	 * どのキャンバスを使用中か返す
	 **************************************************/
	public KeyValuePair<int, string> WhichCanvasActive() {
		return new KeyValuePair<int, string>(CanvasState, CanvasDictionary[CanvasState].name);
	}
}
