using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class Target : MonoBehaviourPunCallbacks
{  
    PhotonManager pm;
    Animator anim;
    void Start(){
        pm = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
        anim = this.GetComponent<Animator>();
        anim.SetBool("moveTarget", false);
    }
    void Update(){
        if(pm.shotOn){
            //anim.SetBool("moveTarget", true);
        }
    }
    private void OnCollisionEnter(Collision col){
        if(photonView.IsMine){
            if(col.gameObject.GetComponent<Arrow>()){
                col.gameObject.GetComponent<Arrow>().Hit(true);
                GameManager.instance.score += 1;
            }
        }
    }
}
