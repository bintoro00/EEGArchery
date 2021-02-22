using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.IO;
public class PhotonManager : MonoBehaviourPunCallbacks
{
    #region MWMのコア
    private const int LABEL_WIDTH = 100;
	private const int VALUE_MIN_WIDTH = 70;
    [Header("References")]
    [SerializeField]
    private MindwaveController m_Controller = null; //パワスペとか取り出すやつ
    [SerializeField]
    private MindwaveCalibrator m_Calibrator = null;　//較正するやつ（使ってない）

    private MindwaveDataModel m_MindwaveData; //変数色々引っ張り込むやつ
    private int m_EEGValue = 0;　//rawデータ（高速）
    private int m_BlinkStrength = 0;　//まばたき
    #endregion

    #region Public 変数
    public Transform c1,c2,c3,c4,c5;　//コンプの座標
    public GameObject b1,b2,b3,b4;　//サブ亀隠すやつ
    public GameObject canvas, list, calib; //キャンバスのやつ（子にする用）
    public GameObject Conne,Disco,Cing; //接続ステータスのスプライト
    public CameraManager cam;   //サブカメラの管理
    public Camera mainCamera;   //カメラのポジション
    public GameObject playerPrefab, calibBtn, calibEndBtn;　//プレハブ色々
    public Slider attention;　//スライダー（メインコントローラー）
    public Text calibTime, calibInstruction, standardText, calibStartTxt, //テキスト色々
                Traw,Tblink,Tmed, Tdelta,Ttheta,TlowAlpha,ThighAlpha,TlowBeta,ThighBeta,TlowGamma,ThighGamma;　//パワスペ
    public bool shotOn, readyOn = false;　//ブールのやつ
    public int Svalue = 0;　//較正結果
    public string Amwm;

    public Text calib10;

    public TimeCounter tc;
    #endregion

    #region Private 変数
    [SerializeField]
    private Text joinedMembersText = default;　//プレイヤーリスト
    public GameObject player; //プレイヤー入れるやつ
    private float CalibrationTime;　//較正する時間
    private bool flag, timeOn, reload = false;　//ブールのやつ　その2
    private int Addvalue;　//加算させるやつ
    private float AddTime = 0f;　//1秒数えるやつ

    public int calibT = 10;

    bool k_flag = false;
    int cnt = 0;
    float Cnt = 0.0f;
    bool mCntFlag = false;
    private string raw,blink,Atten,Mediten,Delta,Theta,hAlpha,lAlpha,hBeta,lBeta,hGamma,lGamma;
    float yobidashi = 0f;
    int yobidashi2 = 0;
    float blinkTime = 0.0f;
    //private float c = 2.0f; //カウント
    //private float k = 0.0f; //仮数
    //private bool w = false; //条件達成の有無
    #endregion

    //デバック用
    private void Awake(){
        if(!PhotonNetwork.IsConnected){
            SceneManager.LoadScene("Login");
            return;
        }

        if(m_Controller == null){
            m_Controller = GetComponent<MindwaveController>();
        }
        if(m_Calibrator == null){
            m_Calibrator = GetComponent<MindwaveCalibrator>();
        }
        BindMindwaveControllerEvents();

    }
    void Start() {
        Settings();
        SetMember();
        UpdateMemberList();

        //記録初期化
        File.WriteAllText(Application.dataPath + @"/測定結果/RawData.txt",""); 
        File.WriteAllText(Application.dataPath + @"/測定結果/Blink.txt","");
        File.WriteAllText(Application.dataPath + @"/測定結果/EEGPOWER.txt","");
        File.WriteAllText(Application.dataPath + @"/測定結果/eSense.txt","");
        //スコア初期化
        GameManager.instance.score = 0;
    }

    void Update(){
        Amwm = m_MindwaveData.eSense.attention.ToString();
        //繋がってれば～云々かんぬん 
        if (m_Controller.IsConnecting){
            Cing.SetActive(true);
            Conne.SetActive(false);
            Disco.SetActive(false);
        }
        else if (m_Controller.IsConnected){
            Conne.SetActive(true);
            Cing.SetActive(false);
            Disco.SetActive(false);
            if(!flag){
                //calib.SetActive(true);
                calibEndBtn.SetActive(false);
                flag = true;
            }
            SetEEG();
        }
        else{
            flag = false;
            Disco.SetActive(true);
            Cing.SetActive(false);
            Conne.SetActive(false);
        }
        //接続後のトリガー開けるやつ
        if(timeOn){
            CalibrationSet();
        }
        //ショットの処理
        if(attention.value==attention.maxValue && shotOn && !reload){
            player.GetComponent<Shot>().OnBtn = true;
            reload = true;
        }
        //リロード
        if(player.GetComponent<Shot>().OnBtn){
            attention.value = attention.maxValue;
            Invoke("Reload",3.0f);
        }
        else if(shotOn){
            //2秒足し込みのテスト
            /*c -= Time.deltaTime;
            if(c>0.0f){
                if(!w){if(Svalue<=m_MindwaveData.eSense.attention){k += Svalue * 0.2f; w = true;}
                    else{k -= Svalue * 0.2f; w = true;}}}
            else if(w){c = 2.0f; w = false; attention.value = k;}*/

            //通常こっち
            attention.value = m_MindwaveData.eSense.attention;

            blinkTime += Time.deltaTime;
            Cnt += Time.deltaTime;
            if(Cnt>=0f&&!mCntFlag){
                Atten += m_MindwaveData.eSense.attention.ToString()+",";
                Mediten += m_MindwaveData.eSense.meditation.ToString()+",";
                Delta += m_MindwaveData.eegPower.delta.ToString()+",";
                Theta += m_MindwaveData.eegPower.theta.ToString()+",";
                hAlpha += m_MindwaveData.eegPower.highAlpha.ToString()+",";
                lAlpha += m_MindwaveData.eegPower.lowAlpha.ToString()+",";
                hBeta += m_MindwaveData.eegPower.highBeta.ToString()+",";
                lBeta += m_MindwaveData.eegPower.lowBeta.ToString()+",";
                hGamma += m_MindwaveData.eegPower.highGamma.ToString()+",";
                lGamma += m_MindwaveData.eegPower.lowGamma.ToString()+",";
                mCntFlag = true;
            }
            if(Cnt>=1f){
                Atten += m_MindwaveData.eSense.attention.ToString()+",";
                Mediten += m_MindwaveData.eSense.meditation.ToString()+",";
                Delta += m_MindwaveData.eegPower.delta.ToString()+",";
                Theta += m_MindwaveData.eegPower.theta.ToString()+",";
                hAlpha += m_MindwaveData.eegPower.highAlpha.ToString()+",";
                lAlpha += m_MindwaveData.eegPower.lowAlpha.ToString()+",";
                hBeta += m_MindwaveData.eegPower.highBeta.ToString()+",";
                lBeta += m_MindwaveData.eegPower.lowBeta.ToString()+",";
                hGamma += m_MindwaveData.eegPower.highGamma.ToString()+",";
                lGamma += m_MindwaveData.eegPower.lowGamma.ToString()+",";
                Cnt = 0f;
            }

        }
        //記録結果の出力
        if(tc.flag&&cnt==0){
            k_flag = true;
            cnt = 1;
        }
        if(k_flag){            
            File.AppendAllText(Application.dataPath + @"/測定結果/EEGPOWER.txt","デルタ：" + Delta +"\n");
            File.AppendAllText(Application.dataPath + @"/測定結果/EEGPOWER.txt","シータ：" + Theta +" \n");
            File.AppendAllText(Application.dataPath + @"/測定結果/EEGPOWER.txt","高アルファ：" + hAlpha+"\n");
            File.AppendAllText(Application.dataPath + @"/測定結果/EEGPOWER.txt","低アルファ：" +lAlpha+"\n");
            File.AppendAllText(Application.dataPath + @"/測定結果/EEGPOWER.txt","高ベータ：" + hBeta+"\n");
            File.AppendAllText(Application.dataPath + @"/測定結果/EEGPOWER.txt","低ベータ："+lBeta+"\n");
            File.AppendAllText(Application.dataPath + @"/測定結果/EEGPOWER.txt","高ガンマ："+hGamma+"\n");
            File.AppendAllText(Application.dataPath + @"/測定結果/EEGPOWER.txt","低ガンマ："+lGamma);
            File.AppendAllText(Application.dataPath + @"/測定結果/eSense.txt","リラックス度："+Mediten+"\n");
            File.AppendAllText(Application.dataPath + @"/測定結果/eSense.txt","集中度："+Atten);
            File.AppendAllText(Application.dataPath + @"/測定結果/Blink.txt",blink);
            k_flag = false;
        }
    }
    #region Private Method
    //矢の補充
    void Reload(){
        if(reload){
            attention.value = 0.0f;
            reload = false;
        }
    }
    //ログイン状態の出力・その他GUI
    void OnGUI(){
      GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }
    //PUN2の設定
    void Settings(){
        PhotonNetwork.IsMessageQueueRunning = true;
        PhotonNetwork.SendRate = 20; // 1秒間にメッセージ送信を行う回数
        PhotonNetwork.SerializationRate = 10; // 1秒間にオブジェクト同期を行う回数
    }
    //パワスペ表示させるやつ
    void SetEEG(){
        Traw.text = "Raw："+m_EEGValue.ToString();
        Tblink.text = "Blink："+m_BlinkStrength.ToString();
        Tmed.text = "Meditation："+m_MindwaveData.eSense.meditation;

        Tdelta.text = "Delta："+m_MindwaveData.eegPower.delta.ToString();
        Ttheta.text = "Theta："+m_MindwaveData.eegPower.theta.ToString();
        TlowAlpha.text = "Low Alpha："+m_MindwaveData.eegPower.lowAlpha.ToString();
        ThighAlpha.text = "High Alpha："+m_MindwaveData.eegPower.highAlpha.ToString();
        TlowBeta.text = "Low Beta："+m_MindwaveData.eegPower.lowBeta.ToString();
        ThighBeta.text = "High Beta："+m_MindwaveData.eegPower.highBeta.ToString();
        TlowGamma.text = "Low Gamma："+m_MindwaveData.eegPower.lowGamma.ToString();
        ThighGamma.text = "High Gamma："+m_MindwaveData.eegPower.highGamma.ToString();
    }
    //較正のあれこれ
    void CalibrationSet(){
        CalibrationTime -= Time.deltaTime; //引いて
        AddTime += Time.deltaTime;         //足して
        for(int i=calibT;i>0;--i){            
            if(CalibrationTime > i){
                calibTime.text = CalibrationTime.ToString("f0");
            }
        }
        if(AddTime > 0.99f){ //1秒後に
            Addvalue += m_MindwaveData.eSense.attention; //足して
            calib10.text += m_MindwaveData.eSense.attention.ToString() + ", ";
            AddTime = 0f; //もっかい
        }
        //0で計測終了
        if(CalibrationTime < 0){
            Svalue = Addvalue / calibT; //較正完了
            if(Svalue >= 60){
                Svalue = 60;
            }
            //ショットのテスト
            attention.maxValue = Svalue; //最大値の設定
            calibInstruction.text = "結果："+Svalue.ToString();
            
            calibEndBtn.SetActive(true);
            calibBtn.SetActive(true);
            calibStartTxt.text = "もう一度";
            timeOn = false; //トリガー閉じ
        }
    }
    #endregion

    #region Public Method
    //接続ボタンのイベント
    public void ConnectBtn(){
        if(m_Controller != null){
            m_Controller.Connect();
        }
    }
    //較正開始ボタンのイベント
    public void CalibStart(){
        calibBtn.SetActive(false);
        calibEndBtn.SetActive(false);
        timeOn = true;
        calibInstruction.text = "測定中 ..";
        calib10.text = "測定値：";
        CalibrationTime = calibT;
        Addvalue = 0;
    }
    //較正終りボタンのイベント
    public void CalibEndBtn(){
        calib.SetActive(false);　//較正画面閉じ
        readyOn = true; //Readyボタン表示
        standardText.text = "Calibration Value：" + Svalue.ToString();
    }
    //参加時のコールバック
    public override void OnPlayerEnteredRoom(Player player) {
        Debug.Log(player.NickName + "が参加しました");
        UpdateMemberList();
        int num = PhotonNetwork.CurrentRoom.PlayerCount;
        if(num==2){
          b1.SetActive(false);
        }
        if(num==3){
          b2.SetActive(false);
        }
        if(num==4){
          b3.SetActive(false);
        }
        if(num==5){
          b4.SetActive(false);
        }
    }
    //退出時のコールバック
    public override void OnPlayerLeftRoom(Player player) {
        Debug.Log(player.NickName + "が退出しました");
        UpdateMemberList();
    }
    //メンバー更新
    public void UpdateMemberList(){
        joinedMembersText.text = "";
        foreach (var p in PhotonNetwork.PlayerList){
            joinedMembersText.text += " " + p.NickName + "\n";
        }
    }
    //プレイヤーの生成
    public void SetMember(){
        if(PhotonNetwork.LocalPlayer.ActorNumber==1){
            player = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 1f, 5f), playerPrefab.transform.rotation);
            mainCamera.transform.position = new Vector3(0f,2f,-10f);
            //TextMesh n = GameObject.Find("name1").GetComponent<TextMesh>();
            //n.text = PhotonNetwork.NickName;
        }
        if(PhotonNetwork.LocalPlayer.ActorNumber==2){
            player = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(15f, 1f, 5f), playerPrefab.transform.rotation);
            mainCamera.transform.position = new Vector3(15f,2f,-10f);
            cam.Camera2();
            b1.SetActive(false);
            //TextMesh n = GameObject.Find("name2").GetComponent<TextMesh>();
            //n.text = PhotonNetwork.NickName;
        }
        if(PhotonNetwork.LocalPlayer.ActorNumber==3){
            player = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(30f, 1f, 5f), playerPrefab.transform.rotation);
            mainCamera.transform.position = new Vector3(30f,2f,-10f);
            cam.Camera3();
            b1.SetActive(false);
            b2.SetActive(false);
            //TextMesh n = GameObject.Find("name3").GetComponent<TextMesh>();
            //n.text = PhotonNetwork.NickName;
        }
        if(PhotonNetwork.LocalPlayer.ActorNumber==4){
            player = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(45f, 1f, 5f), playerPrefab.transform.rotation);
            mainCamera.transform.position = new Vector3(45f,2f,-10f);
            cam.Camera4();
            b1.SetActive(false);
            b2.SetActive(false);
            b3.SetActive(false);
            //TextMesh n = GameObject.Find("name4").GetComponent<TextMesh>();
            //n.text = PhotonNetwork.NickName;
        }
        if(PhotonNetwork.LocalPlayer.ActorNumber==5){
            player = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(60f, 1f, 5f), playerPrefab.transform.rotation);
            mainCamera.transform.position = new Vector3(60f,2f,-10f);
            cam.Camera5();
            b1.SetActive(false);
            b2.SetActive(false);
            b3.SetActive(false);
            b4.SetActive(false);
            //TextMesh n = GameObject.Find("name5").GetComponent<TextMesh>();
            //n.text = PhotonNetwork.NickName;
        }
    }
    #endregion

    //MindWaveのあれこれ
    public void OnUpdateMindwaveData(MindwaveDataModel _Data){
        m_MindwaveData = _Data;
        if(shotOn){
            yobidashi2 += 1;
            
            Debug.Log(yobidashi2);
        }
    }
    public void OnUpdateRawEEG(int _EEGValue){
        m_EEGValue = _EEGValue;

        //rawの記録
        if(shotOn){
            yobidashi += 1f;
            raw += _EEGValue.ToString()+",";
            if(yobidashi>=512f){
                File.AppendAllText(Application.dataPath + @"/測定結果/RawData.txt",raw);
                yobidashi = 0f;
                raw = "";
            }
        }
    }
    public void OnUpdateBlink(int _BlinkStrength){
        m_BlinkStrength = _BlinkStrength;

        if(shotOn){
            blink += blinkTime.ToString("f2") + "秒："+_BlinkStrength.ToString()+"\n";
        }
    }
    private void BindMindwaveControllerEvents(){
        m_Controller.OnUpdateMindwaveData += OnUpdateMindwaveData;
        m_Controller.OnUpdateRawEEG += OnUpdateRawEEG;
        m_Controller.OnUpdateBlink += OnUpdateBlink;
    }
}