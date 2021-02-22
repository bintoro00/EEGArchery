using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class Score : MonoBehaviourPunCallbacks
{
    public Shot s;
    Text scoreText;
    void Start(){
        scoreText = this.GetComponent<Text>();
    }
    void Update(){
        GameObject.Find("Score1").GetComponent<TextMesh>().text = s.nameL1.text;
        GameObject.Find("Score2").GetComponent<TextMesh>().text = s.nameL2.text;
        GameObject.Find("Score3").GetComponent<TextMesh>().text = s.nameL3.text;
        GameObject.Find("Score4").GetComponent<TextMesh>().text = s.nameL4.text;
        GameObject.Find("Score5").GetComponent<TextMesh>().text = s.nameL5.text;

        PhotonManager pm = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
        scoreText.text = "Attention Value："+pm.Amwm;//GameManager.instance.score.ToString();

        var hashtable = new Hashtable();
        if(PhotonNetwork.LocalPlayer.ActorNumber==1){
            hashtable["Score1"] = GameManager.instance.score;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        }
        if(PhotonNetwork.LocalPlayer.ActorNumber==2){
            hashtable["Score2"] = GameManager.instance.score;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        }
        if(PhotonNetwork.LocalPlayer.ActorNumber==3){
            hashtable["Score3"] = GameManager.instance.score;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        }
        if(PhotonNetwork.LocalPlayer.ActorNumber==4){
            hashtable["Score4"] = GameManager.instance.score;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        }
        if(PhotonNetwork.LocalPlayer.ActorNumber==5){
            hashtable["Score5"] = GameManager.instance.score;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        }
    }
}
