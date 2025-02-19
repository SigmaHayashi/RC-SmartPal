﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.SceneManagement;

public class RCSmartPalConfig {
	public string ros_ip = "ws://192.168.4.170:9090";
	public float[] right_arm_home_pos = new float[7];
	public float right_arm_move_speed = 10.0f;
	public float[] left_arm_home_pos = new float[7];
	public float left_arm_move_speed = 10.0f;
	public float right_gripper_home_pos = -10.0f;
	public float right_gripper_move_speed = 10.0f;
	public float left_gripper_home_pos = -10.0f;
	public float left_gripper_move_speed = 10.0f;
}

public class MainScript : MonoBehaviour {

	//スクリーンが消えないようにする
	public bool ScreenNOTSleep = true;

	//Canvasたち
	private GameObject InfoCanvas;
	private GameObject SettingsCanvas;
	private GameObject VehicleCanvas;
	private GameObject RightArmCanvas;
	private GameObject LeftArmCanvas;
	private GameObject RightGripperCanvas;
	private GameObject LeftGripperCanvas;

	//Canvasを遷移させるボタンたち
	private Button Info_ChangeToSettings_Button;
	private Button Info_ChangeToVehicle_Button;
	private Button Info_ChangeToRightArm_Button;
	private Button Info_ChangeToLeftArm_Button;
	private Button Info_ChangeToRightGripper_Button;
	private Button Info_ChangeToLeftGripper_Button;

	private Button Settings_ChangeToInfo_Button;
	private GameObject Settings_ChangeToSettings_Button;
	private Button Settings_RestartApp_Button;
	private Button Settings_ChangeToVehicle_Button;
	private Button Settings_ChangeToRightArm_Button;
	private Button Settings_ChangeToLeftArm_Button;
	private Button Settings_ChangeToRightGripper_Button;
	private Button Settings_ChangeToLeftGripper_Button;

	private Button Vehicle_ChangeToInfo_Button;
	private Button Vehicle_ChangeToSettings_Button;
	private Button Vehicle_ChengeToRightArm_Button;
	private Button Vehicle_ChengeToLeftArm_Button;
	private Button Vehicle_ChangeToRightGripper_Button;
	private Button Vehicle_ChangeToLeftGripper_Button;

	private Button RightArm_ChangeToInfo_Button;
	private Button RightArm_ChangeToSettings_Button;
	private Button RightArm_ChengeToVehicle_Button;
	private Button RightArm_ChengeToLeftArm_Button;
	private Button RightArm_ChangeToRightGripper_Button;
	private Button RightArm_ChangeToLeftGripper_Button;

	private Button LeftArm_ChangeToInfo_Button;
	private Button LeftArm_ChangeToSettings_Button;
	private Button LeftArm_ChengeToVehicle_Button;
	private Button LeftArm_ChengeToRightArm_Button;
	private Button LeftArm_ChangeToRightGripper_Button;
	private Button LeftArm_ChangeToLeftGripper_Button;

	private Button RightGripper_ChangeToInfo_Button;
	private Button RightGripper_ChangeToSettings_Button;
	private Button RightGripper_ChangeToVehicle_Button;
	private Button RightGripper_ChangeToRightArm_Button;
	private Button RightGripper_ChangeToLeftArm_Button;
	private Button RightGripper_ChangeToLeftGripper_Button;

	private Button LeftGripper_ChangeToInfo_Button;
	private Button LeftGripper_ChangeToSettings_Button;
	private Button LeftGripper_ChangeToVehicle_Button;
	private Button LeftGripper_ChangeToRightArm_Button;
	private Button LeftGripper_ChangeToLeftArm_Button;
	private Button LeftGripper_ChangeToRightGripper_Button;

	//いまどのCanvasを使用中か示す変数・それに対応する辞書
	private int CanvasState = 0;
	private Dictionary<int, GameObject> CanvasDictionary = new Dictionary<int, GameObject>();

	private CommunicationManager cm;

	//コンフィグ周り
	private bool finish_read_config = false;
	private string config_filepath;
	private RCSmartPalConfig config_data = new RCSmartPalConfig();

	public bool IsFinishReadConfig() {
		return finish_read_config;
	}

	public RCSmartPalConfig GetConfig() {
		return config_data;
	}

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
		SettingsCanvas = GameObject.Find("Main System/Settings Canvas");
		VehicleCanvas = GameObject.Find("Main System/Vehicle Canvas");
		RightArmCanvas = GameObject.Find("Main System/Right Arm Canvas");
		LeftArmCanvas = GameObject.Find("Main System/Left Arm Canvas");
		RightGripperCanvas = GameObject.Find("Main System/Right Gripper Canvas");
		LeftGripperCanvas = GameObject.Find("Main System/Left Gripper Canvas");
		CanvasDictionary.Add(0, InfoCanvas);
		CanvasDictionary.Add(-1, SettingsCanvas);
		CanvasDictionary.Add(1, VehicleCanvas);
		CanvasDictionary.Add(2, RightArmCanvas);
		CanvasDictionary.Add(3, LeftArmCanvas);
		CanvasDictionary.Add(4, RightGripperCanvas);
		CanvasDictionary.Add(5, LeftGripperCanvas);

		//Canvas移動用ボタンを取得・設定
		Info_ChangeToSettings_Button = GameObject.Find("Main System/Info Canvas/Change to Settings Button").GetComponent<Button>();
		Info_ChangeToVehicle_Button = GameObject.Find("Main System/Info Canvas/Change to Vehicle Button").GetComponent<Button>();
		Info_ChangeToRightArm_Button = GameObject.Find("Main System/Info Canvas/Change to Right Arm Button").GetComponent<Button>();
		Info_ChangeToLeftArm_Button = GameObject.Find("Main System/Info Canvas/Change to Left Arm Button").GetComponent<Button>();
		Info_ChangeToRightGripper_Button = GameObject.Find("Main System/Info Canvas/Change to Right Gripper Button").GetComponent<Button>();
		Info_ChangeToLeftGripper_Button = GameObject.Find("Main System/Info Canvas/Change to Left Gripper Button").GetComponent<Button>();
		Info_ChangeToSettings_Button.onClick.AddListener(ChangeToSettings);
		Info_ChangeToVehicle_Button.onClick.AddListener(ChangeToVehicle);
		Info_ChangeToRightArm_Button.onClick.AddListener(ChangeToRightArm);
		Info_ChangeToLeftArm_Button.onClick.AddListener(ChangeToLeftArm);
		Info_ChangeToRightGripper_Button.onClick.AddListener(ChangeToRightGripper);
		Info_ChangeToLeftGripper_Button.onClick.AddListener(ChangeToLeftGripper);

		Settings_ChangeToInfo_Button = GameObject.Find("Main System/Settings Canvas/Change to Info Button").GetComponent<Button>();
		Settings_ChangeToSettings_Button = GameObject.Find("Main System/Settings Canvas/Change to Settings Button");
		Settings_RestartApp_Button = GameObject.Find("Main System/Settings Canvas/Restart App Button").GetComponent<Button>();
		Settings_ChangeToVehicle_Button = GameObject.Find("Main System/Settings Canvas/Change to Vehicle Button").GetComponent<Button>();
		Settings_ChangeToRightArm_Button = GameObject.Find("Main System/Settings Canvas/Change to Right Arm Button").GetComponent<Button>();
		Settings_ChangeToLeftArm_Button = GameObject.Find("Main System/Settings Canvas/Change to Left Arm Button").GetComponent<Button>();
		Settings_ChangeToRightGripper_Button = GameObject.Find("Main System/Settings Canvas/Change to Right Gripper Button").GetComponent<Button>();
		Settings_ChangeToLeftGripper_Button = GameObject.Find("Main System/Settings Canvas/Change to Left Gripper Button").GetComponent<Button>();
		Settings_ChangeToInfo_Button.onClick.AddListener(ChangeToInfo);
		Settings_RestartApp_Button.onClick.AddListener(RestartApp);
		Settings_ChangeToVehicle_Button.onClick.AddListener(ChangeToVehicle);
		Settings_ChangeToRightArm_Button.onClick.AddListener(ChangeToRightArm);
		Settings_ChangeToLeftArm_Button.onClick.AddListener(ChangeToLeftArm);
		Settings_ChangeToRightGripper_Button.onClick.AddListener(ChangeToRightGripper);
		Settings_ChangeToLeftGripper_Button.onClick.AddListener(ChangeToLeftGripper);

		Vehicle_ChangeToInfo_Button = GameObject.Find("Main System/Vehicle Canvas/Change to Info Button").GetComponent<Button>();
		Vehicle_ChangeToSettings_Button = GameObject.Find("Main System/Vehicle Canvas/Change to Settings Button").GetComponent<Button>();
		Vehicle_ChengeToRightArm_Button = GameObject.Find("Main System/Vehicle Canvas/Change to Right Arm Button").GetComponent<Button>();
		Vehicle_ChengeToLeftArm_Button = GameObject.Find("Main System/Vehicle Canvas/Change to Left Arm Button").GetComponent<Button>();
		Vehicle_ChangeToRightGripper_Button = GameObject.Find("Main System/Vehicle Canvas/Change to Right Gripper Button").GetComponent<Button>();
		Vehicle_ChangeToLeftGripper_Button = GameObject.Find("Main System/Vehicle Canvas/Change to Left Gripper Button").GetComponent<Button>();
		Vehicle_ChangeToInfo_Button.onClick.AddListener(ChangeToInfo);
		Vehicle_ChangeToSettings_Button.onClick.AddListener(ChangeToSettings);
		Vehicle_ChengeToRightArm_Button.onClick.AddListener(ChangeToRightArm);
		Vehicle_ChengeToLeftArm_Button.onClick.AddListener(ChangeToLeftArm);
		Vehicle_ChangeToRightGripper_Button.onClick.AddListener(ChangeToRightGripper);
		Vehicle_ChangeToLeftGripper_Button.onClick.AddListener(ChangeToLeftGripper);

		RightArm_ChangeToInfo_Button = GameObject.Find("Main System/Right Arm Canvas/Change to Info Button").GetComponent<Button>();
		RightArm_ChangeToSettings_Button = GameObject.Find("Main System/Right Arm Canvas/Change to Settings Button").GetComponent<Button>();
		RightArm_ChengeToVehicle_Button = GameObject.Find("Main System/Right Arm Canvas/Change to Vehicle Button").GetComponent<Button>();
		RightArm_ChengeToLeftArm_Button = GameObject.Find("Main System/Right Arm Canvas/Change to Left Arm Button").GetComponent<Button>();
		RightArm_ChangeToRightGripper_Button = GameObject.Find("Main System/Right Arm Canvas/Change to Right Gripper Button").GetComponent<Button>();
		RightArm_ChangeToLeftGripper_Button = GameObject.Find("Main System/Right Arm Canvas/Change to Left Gripper Button").GetComponent<Button>();
		RightArm_ChangeToInfo_Button.onClick.AddListener(ChangeToInfo);
		RightArm_ChangeToSettings_Button.onClick.AddListener(ChangeToSettings);
		RightArm_ChengeToVehicle_Button.onClick.AddListener(ChangeToVehicle);
		RightArm_ChengeToLeftArm_Button.onClick.AddListener(ChangeToLeftArm);
		RightArm_ChangeToRightGripper_Button.onClick.AddListener(ChangeToRightGripper);
		RightArm_ChangeToLeftGripper_Button.onClick.AddListener(ChangeToLeftGripper);

		LeftArm_ChangeToInfo_Button = GameObject.Find("Main System/Left Arm Canvas/Change to Info Button").GetComponent<Button>();
		LeftArm_ChangeToSettings_Button = GameObject.Find("Main System/Left Arm Canvas/Change to Settings Button").GetComponent<Button>();
		LeftArm_ChengeToVehicle_Button = GameObject.Find("Main System/Left Arm Canvas/Change to Vehicle Button").GetComponent<Button>();
		LeftArm_ChengeToRightArm_Button = GameObject.Find("Main System/Left Arm Canvas/Change to Right Arm Button").GetComponent<Button>();
		LeftArm_ChangeToRightGripper_Button = GameObject.Find("Main System/Left Arm Canvas/Change to Right Gripper Button").GetComponent<Button>();
		LeftArm_ChangeToLeftGripper_Button = GameObject.Find("Main System/Left Arm Canvas/Change to Left Gripper Button").GetComponent<Button>();
		LeftArm_ChangeToInfo_Button.onClick.AddListener(ChangeToInfo);
		LeftArm_ChangeToSettings_Button.onClick.AddListener(ChangeToSettings);
		LeftArm_ChengeToVehicle_Button.onClick.AddListener(ChangeToVehicle);
		LeftArm_ChengeToRightArm_Button.onClick.AddListener(ChangeToRightArm);
		LeftArm_ChangeToRightGripper_Button.onClick.AddListener(ChangeToRightGripper);
		LeftArm_ChangeToLeftGripper_Button.onClick.AddListener(ChangeToLeftGripper);

		RightGripper_ChangeToInfo_Button = GameObject.Find("Main System/Right Gripper Canvas/Change to Info Button").GetComponent<Button>();
		RightGripper_ChangeToSettings_Button = GameObject.Find("Main System/Right Gripper Canvas/Change to Settings Button").GetComponent<Button>();
		RightGripper_ChangeToVehicle_Button = GameObject.Find("Main System/Right Gripper Canvas/Change to Vehicle Button").GetComponent<Button>();
		RightGripper_ChangeToRightArm_Button = GameObject.Find("Main System/Right Gripper Canvas/Change to Right Arm Button").GetComponent<Button>();
		RightGripper_ChangeToLeftArm_Button = GameObject.Find("Main System/Right Gripper Canvas/Change to Left Arm Button").GetComponent<Button>();
		RightGripper_ChangeToLeftGripper_Button = GameObject.Find("Main System/Right Gripper Canvas/Change to Left Gripper Button").GetComponent<Button>();
		RightGripper_ChangeToInfo_Button.onClick.AddListener(ChangeToInfo);
		RightGripper_ChangeToSettings_Button.onClick.AddListener(ChangeToSettings);
		RightGripper_ChangeToVehicle_Button.onClick.AddListener(ChangeToVehicle);
		RightGripper_ChangeToRightArm_Button.onClick.AddListener(ChangeToRightArm);
		RightGripper_ChangeToLeftArm_Button.onClick.AddListener(ChangeToLeftArm);
		RightGripper_ChangeToLeftGripper_Button.onClick.AddListener(ChangeToLeftGripper);

		LeftGripper_ChangeToInfo_Button = GameObject.Find("Main System/Left Gripper Canvas/Change to Info Button").GetComponent<Button>();
		LeftGripper_ChangeToSettings_Button = GameObject.Find("Main System/Left Gripper Canvas/Change to Settings Button").GetComponent<Button>();
		LeftGripper_ChangeToVehicle_Button = GameObject.Find("Main System/Left Gripper Canvas/Change to Vehicle Button").GetComponent<Button>();
		LeftGripper_ChangeToRightArm_Button = GameObject.Find("Main System/Left Gripper Canvas/Change to Right Arm Button").GetComponent<Button>();
		LeftGripper_ChangeToLeftArm_Button = GameObject.Find("Main System/Left Gripper Canvas/Change to Left Arm Button").GetComponent<Button>();
		LeftGripper_ChangeToRightGripper_Button = GameObject.Find("Main System/Left Gripper Canvas/Change to Right Gripper Button").GetComponent<Button>();
		LeftGripper_ChangeToInfo_Button.onClick.AddListener(ChangeToInfo);
		LeftGripper_ChangeToSettings_Button.onClick.AddListener(ChangeToSettings);
		LeftGripper_ChangeToVehicle_Button.onClick.AddListener(ChangeToVehicle);
		LeftGripper_ChangeToRightArm_Button.onClick.AddListener(ChangeToRightArm);
		LeftGripper_ChangeToLeftArm_Button.onClick.AddListener(ChangeToLeftArm);
		LeftGripper_ChangeToRightGripper_Button.onClick.AddListener(ChangeToRightGripper);

		//コンフィグファイル読み込み
		config_filepath = Application.persistentDataPath + "/RC SmartPal Config.JSON";
		if (!File.Exists(config_filepath)) {
			using (File.Create(config_filepath)) { }
			config_data.right_arm_home_pos[1] = -5.0f;
			config_data.left_arm_home_pos[1] = -5.0f;
			string config_json = JsonUtility.ToJson(config_data);
			using (FileStream file = new FileStream(config_filepath, FileMode.Create, FileAccess.Write)) {
				using (StreamWriter writer = new StreamWriter(file)) {
					writer.Write(config_json);
				}
			}
		}
		using (FileStream file = new FileStream(config_filepath, FileMode.Open, FileAccess.Read)) {
			using (StreamReader reader = new StreamReader(file)) {
				string config_read = reader.ReadToEnd();
				Debug.Log(config_read);

				config_data = JsonUtility.FromJson<RCSmartPalConfig>(config_read);

				GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/ROS IP/Input_0").GetComponent<InputField>().text = config_data.ros_ip;
				for(int i = 0; i < 7; i++) {
					GameObject.Find(string.Format("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Right Arm Home Pos/Input_{0}", i)).GetComponent<InputField>().text = config_data.right_arm_home_pos[i].ToString();
					GameObject.Find(string.Format("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Left Arm Home Pos/Input_{0}", i)).GetComponent<InputField>().text = config_data.left_arm_home_pos[i].ToString();
				}
				GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Right Arm Move Speed/Input_0").GetComponent<InputField>().text = config_data.right_arm_move_speed.ToString();
				GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Left Arm Move Speed/Input_0").GetComponent<InputField>().text = config_data.left_arm_move_speed.ToString();
				GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Right Gripper Home Pos/Input_0").GetComponent<InputField>().text = config_data.right_gripper_home_pos.ToString();
				GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Right Gripper Move Speed/Input_0").GetComponent<InputField>().text = config_data.right_gripper_move_speed.ToString();
				GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Left Gripper Home Pos/Input_0").GetComponent<InputField>().text = config_data.left_gripper_home_pos.ToString();
				GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Left Gripper Move Speed/Input_0").GetComponent<InputField>().text = config_data.left_gripper_move_speed.ToString();

				finish_read_config = true;
			}
		}

		//値が変わったときの挙動
		GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/ROS IP/Input_0").GetComponent<InputField>().onValueChanged.AddListener(Config_Changed);
		for (int i = 0; i < 7; i++) {
			GameObject.Find(string.Format("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Right Arm Home Pos/Input_{0}", i)).GetComponent<InputField>().onValueChanged.AddListener(Config_Changed);
			GameObject.Find(string.Format("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Left Arm Home Pos/Input_{0}", i)).GetComponent<InputField>().onValueChanged.AddListener(Config_Changed);
		}
		GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Right Arm Move Speed/Input_0").GetComponent<InputField>().onValueChanged.AddListener(Config_Changed);
		GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Left Arm Move Speed/Input_0").GetComponent<InputField>().onValueChanged.AddListener(Config_Changed);
		GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Right Gripper Home Pos/Input_0").GetComponent<InputField>().onValueChanged.AddListener(Config_Changed);
		GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Right Gripper Move Speed/Input_0").GetComponent<InputField>().onValueChanged.AddListener(Config_Changed);
		GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Left Gripper Home Pos/Input_0").GetComponent<InputField>().onValueChanged.AddListener(Config_Changed);
		GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Left Gripper Move Speed/Input_0").GetComponent<InputField>().onValueChanged.AddListener(Config_Changed);

		//Settingsキャンバスのボタンの表示・非表示を初期化
		Settings_ChangeButtonActivate(false);

		//CanvasをInfoCanvasのみに設定
		CanvasState = 0;
		SettingsCanvas.SetActive(false);
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

	void ChangeToSettings() {
		IEnumerator coroutine = WaitForChangeToSettings();
		StartCoroutine(coroutine);
	}

	void RestartApp() {
		config_data.ros_ip = GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/ROS IP/Input_0").GetComponent<InputField>().text;
		for(int i = 0; i < 7; i++) {
			config_data.right_arm_home_pos[i] = float.Parse(GameObject.Find(string.Format("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Right Arm Home Pos/Input_{0}", i)).GetComponent<InputField>().text);
			config_data.left_arm_home_pos[i] = float.Parse(GameObject.Find(string.Format("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Left Arm Home Pos/Input_{0}", i)).GetComponent<InputField>().text);
		}
		config_data.right_arm_move_speed = float.Parse(GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Right Arm Move Speed/Input_0").GetComponent<InputField>().text);
		config_data.left_arm_move_speed = float.Parse(GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Left Arm Move Speed/Input_0").GetComponent<InputField>().text);
		config_data.right_gripper_home_pos = float.Parse(GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Right Gripper Home Pos/Input_0").GetComponent<InputField>().text);
		config_data.right_gripper_move_speed = float.Parse(GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Right Gripper Move Speed/Input_0").GetComponent<InputField>().text);
		config_data.left_gripper_home_pos = float.Parse(GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Left Gripper Home Pos/Input_0").GetComponent<InputField>().text);
		config_data.left_gripper_move_speed = float.Parse(GameObject.Find("Main System/Settings Canvas/Settings Area/Scroll View/Scroll Contents/Left Gripper Move Speed/Input_0").GetComponent<InputField>().text);

		if(config_data.right_arm_move_speed < 0.0f) {
			config_data.right_arm_move_speed = 1.0f;
		}
		else if(config_data.right_arm_move_speed > 30.0f) {
			config_data.right_arm_move_speed = 30.0f;
		}

		if(config_data.left_arm_move_speed < 0.0f) {
			config_data.left_arm_move_speed = 1.0f;
		}
		else if (config_data.left_arm_move_speed > 30.0f) {
			config_data.left_arm_move_speed = 30.0f;
		}

		if(config_data.right_gripper_move_speed < 0.0f) {
			config_data.right_gripper_move_speed = 1.0f;
		}
		else if (config_data.right_gripper_move_speed > 30.0f) {
			config_data.right_gripper_move_speed = 30.0f;
		}

		if (config_data.left_gripper_move_speed < 0.0f) {
			config_data.left_gripper_move_speed = 1.0f;
		}
		else if (config_data.left_gripper_move_speed > 30.0f) {
			config_data.left_gripper_move_speed = 30.0f;
		}

		string config_json = JsonUtility.ToJson(config_data);

		using (FileStream file = new FileStream(config_filepath, FileMode.Create, FileAccess.Write)) {
			using (StreamWriter writer = new StreamWriter(file)) {
				writer.Write(config_json);
			}
		}

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	void Config_Changed(string s) {
		Settings_ChangeButtonActivate(true);
	}

	void Settings_ChangeButtonActivate(bool only_restart) {
		Settings_ChangeToInfo_Button.gameObject.SetActive(!only_restart);
		Settings_ChangeToSettings_Button.SetActive(!only_restart);
		Settings_ChangeToVehicle_Button.gameObject.SetActive(!only_restart);
		Settings_ChangeToRightArm_Button.gameObject.SetActive(!only_restart);
		Settings_ChangeToLeftArm_Button.gameObject.SetActive(!only_restart);
		Settings_ChangeToRightGripper_Button.gameObject.SetActive(!only_restart);
		Settings_ChangeToLeftGripper_Button.gameObject.SetActive(!only_restart);

		Settings_RestartApp_Button.gameObject.SetActive(only_restart);
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

	IEnumerator WaitForChangeToSettings() {
		while (cm.CheckWaitAnything()) {
			yield return null;
		}
		CanvasDictionary[CanvasState].SetActive(false);
		SettingsCanvas.SetActive(true);
		CanvasState = -1;
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
