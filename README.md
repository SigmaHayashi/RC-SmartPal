# RC SmartPal


# 概要
ROS-TMSで制御するSmartPal Vをスマートフォン経由で手動で操縦するAndroidアプリケーション


# 必要な環境
PC1 : Windows10 64bit（アプリケーションビルド用）  
PC2 : Ubuntu 16（Smart Previewed Reality実行用）  
※PC1とPC2は同時に起動する必要なし，デュアルブートでOK

Androidスマートフォン

ROS kinetic (Ubuntuにインストールしておく)


# 開発環境
PC : Windows 10 64bit  
* Unity 2018.4.1f1  
* Visual Studio 2017  
* Android Studio 3.5.1  

Android（動作確認済み） : Pixel 3 XL, Pixel 4 XL


# アプリケーションをビルドするためのPCの準備
1. Unityのインストール  
    URL : https://unity3d.com/jp/get-unity/download

1. Visual Studioのインストール  
    ※VS Codeではない  
    ※Unityのインストール中にインストールされるものでOK  
    URL : https://visualstudio.microsoft.com/ja/downloads/

1. Android Studioのインストール  
    ※Android SDKが必要  
    URL : https://developer.android.com/studio


# アプリケーションのインストール方法

1. GitHubから任意の場所にダウンロード

1. Unityでプロジェクトを開く

1. "Main Scene"のSceneを開く

1. File > Build Settingsからビルド環境の設定を開く

1. Androidを選択し，Switch Platformを選択

1. Android端末をPCに接続し，Build & Run


# 使い方

## ROS-TMS for Smart Previewed Realityの実行

実行前に，ROSをインストールしたUbuntuでROS-TMS for Smart Previewed Realityをcatkin_makeしておく必要がある．

ROS-TMS for Smart Previewed Reality : https://github.com/SigmaHayashi/ros_tms  

このアプリケーションをフルに利用するためには，B-sen，SmartPal V，Viconが必要である．
また，データベースを利用するため，mongodbをインストールする必要がある．その他依存関係はROS-TMSのWikiを参照．

Wiki : https://github.com/irvs/ros_tms/wiki


### 実行手順

```
$ roscore
$ roslaunch rosbridge_server rosbridge_websocket.launch
$ rosrun tms_ss_vicon vicon_stream
$ roslaunch tms_db_manager tms_db_manager.launch
```

以下をSmartPal搭載NUCにSSHでアクセスして実行する

※以下のコンソールの数だけSSHでSmartPal搭載NUCにアクセスする．  
※SmartPal搭載NUCにもROS-TMSをインストールする必要あり
```
$ ./start_omniNames

// ここでSmartPalのスイッチパネルのVehicle/Armsスイッチを入れる
// 15秒待ってから

$ rosrun tms_rc_smartpal_control smartpal5_control
```


## アプリの操作

1. アプリケーションを起動

    ※初回起動時は，Settingsボタンを押してROS-TMSを実行しているUbuntu PCのIPアドレスを指定する必要あり（うまく起動しない場合はWi-Fiを一度オフにしてからアプリを起動するとスムーズに起動するかも）

1. 各部分の操縦画面に移動し，ボタンで操作する

1. エラーが出た場合はエラー解除操作を行う

1. 通信エラーが正しく行いないときはアプリの再起動を行う
