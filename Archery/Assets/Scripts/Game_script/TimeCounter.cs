using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class TimeCounter : MonoBehaviourPunCallbacks
{
    public float countdown = 5.0f;
    public GameObject progress, result, okPrefab, list;
    public GameObject Pad, readyBtn, pm;
    public Text progressText;
    public Text timeText;

    private GameObject[] tagObjects, tagObjectsReady;
    private GameObject _ready, resultCanvas;
    private int number;
    private float currentTime = 0f;

    public bool flag = false;

    private Vector3 Posnow = Vector3.zero;
    private Quaternion Rotnow;

    void Start(){
        Pad.SetActive(false);
        progress.SetActive(false);
        readyBtn.SetActive(false);
        resultCanvas = GameObject.Find("Result");

        timeText.text = countdown.ToString("f0");
        if(PhotonNetwork.IsConnected){
            number = PhotonNetwork.CurrentRoom.MaxPlayers;
        }
        //ショットのテスト
        pm.GetComponent<PhotonManager>().readyOn = true;

        var hashtable = new Hashtable();
        hashtable["Timer"] = countdown;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }
    void Update()
    {
        tagObjects = GameObject.FindGameObjectsWithTag("Player"); //監視用に走らせる
        tagObjectsReady = GameObject.FindGameObjectsWithTag("Check");

        if(pm.GetComponent<PhotonManager>().readyOn){
            readyBtn.SetActive(true);
        }
        if(number == tagObjects.Length && number == tagObjectsReady.Length){
            currentTime += Time.deltaTime;
            if(currentTime > 0.5){
                progress.SetActive(true);
                progressText.text = "3";
            if(currentTime > 1.5){
                progressText.text = "2";
            if(currentTime > 2.5){
                progressText.text = "1";
            if(currentTime > 3.5){
                progressText.text = "START";
            if(currentTime > 4){
                Pad.SetActive(true);
                progress.SetActive(false);
                countdown -= Time.deltaTime;
                timeText.text = countdown.ToString("f0");
                pm.GetComponent<PhotonManager>().shotOn = true;

            if(countdown<=30.0f){
                timeText.color = Colors.Yellow;
            }
            if(countdown<=10.0f){
                timeText.color = Colors.Red;
            }
            if(countdown <= 0.0f){
                timeText.text = "0";
                progress.SetActive(true);
                progressText.text = "Time Up";
                pm.GetComponent<PhotonManager>().shotOn = false;
                Invoke("OnResult", 3.0f);
            }
            }}}}}}
    }
    public void OnClickReady(){
        pm.GetComponent<PhotonManager>().readyOn = false;
        foreach (var p in PhotonNetwork.PlayerList){
            Debug.Log(p.NickName+"= 準備完了");
        }
        if(PhotonNetwork.LocalPlayer.ActorNumber==1){
            Posnow = new Vector3(160,70,0);
            Rotnow = Quaternion.identity;
        }
        if(PhotonNetwork.LocalPlayer.ActorNumber==2){
            Posnow = new Vector3(160,24,0);
            Rotnow = Quaternion.identity;
        }
        if(PhotonNetwork.LocalPlayer.ActorNumber==3){
            Posnow = new Vector3(160,-20,0);
            Rotnow = Quaternion.identity;
        }
        if(PhotonNetwork.LocalPlayer.ActorNumber==4){
            Posnow = new Vector3(160,-65,0);
            Rotnow = Quaternion.identity;
        }
        if(PhotonNetwork.LocalPlayer.ActorNumber==5){
            Posnow = new Vector3(160,-110,0);
            Rotnow = Quaternion.identity;
        }
        readyBtn.SetActive(false);
        photonView.RPC(nameof(SetComp), RpcTarget.All, Posnow, Rotnow);
    }
    void OnResult(){
        result.SetActive(true);
        if(!flag){
            resultCanvas.GetComponent<Result>().ResultGame();
            flag = true;
        }
    }
    [PunRPC]
    private void SetComp(Vector3 pos, Quaternion rot){
        _ready = Instantiate(okPrefab, pos, rot);
        _ready.transform.SetParent(list.transform, false);
    }
}