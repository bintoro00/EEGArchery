using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Ready : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject readyPrefab = default;
    public Transform ok1,ok2,ok3,ok4,ok5;
    public GameObject StartBtn;
    public GameObject list; //リスト子オブジェクト
    private bool ready = false;

    public void ReadyButton(){
        ready = true;
        StartBtn.SetActive(false);
    }
    void Update(){
        if(ready){
            AreYouReady();
        }
    }
    void AreYouReady(){
        if(PhotonNetwork.PlayerList.Length==1){
            GameObject _ready = PhotonNetwork.Instantiate(this.readyPrefab.name, ok1.position, ok1.rotation);
            _ready.transform.SetParent(list.transform);
            ready = false;
        }
        if(PhotonNetwork.PlayerList.Length==2){
            GameObject _ready = PhotonNetwork.Instantiate(this.readyPrefab.name, ok2.position, ok2.rotation);
            _ready.transform.SetParent(list.transform);
            ready = false;
        }
        if(PhotonNetwork.PlayerList.Length==3){
            GameObject _ready = PhotonNetwork.Instantiate(this.readyPrefab.name, ok3.position, ok3.rotation);
            _ready.transform.SetParent(list.transform);
            ready = false;
        }
        if(PhotonNetwork.PlayerList.Length==4){
            GameObject _ready = PhotonNetwork.Instantiate(this.readyPrefab.name, ok4.position, ok4.rotation);
            _ready.transform.SetParent(list.transform);
            ready = false;
        }
        if(PhotonNetwork.PlayerList.Length==5){
            GameObject _ready = PhotonNetwork.Instantiate(this.readyPrefab.name, ok5.position, ok5.rotation);
            _ready.transform.SetParent(list.transform);
            ready = false;
        }
    }
}
