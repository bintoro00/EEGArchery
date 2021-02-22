using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Result : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text playerRanking = default;
    [SerializeField]
    private Text playerScore = default;
    public Shot shotP;
    private int maxP;
    public void OnClickRestart(){
        PhotonNetwork.Disconnect();
        SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Single);
    }
    public void OnClickEndGame(){
        PhotonNetwork.Disconnect();
        SceneManager.LoadSceneAsync("Login", LoadSceneMode.Single);
    }
    void Start(){
        if(PhotonNetwork.IsConnected){
            maxP = PhotonNetwork.CurrentRoom.MaxPlayers;
        }
    }
    public void ResultGame(){
        string r1 = shotP.nameL1.text;
        string r2 = shotP.nameL2.text;
        string r3 = shotP.nameL3.text;
        string r4 = shotP.nameL4.text;
        string r5 = shotP.nameL5.text;
        string[] r = {r1,r2,r3,r4,r5};

        for(int i=0;i<maxP;i++){
            playerRanking.text += PhotonNetwork.PlayerList[i].NickName+"\n";
            playerScore.text += r[i]+"\n";
        }
    }
} 
