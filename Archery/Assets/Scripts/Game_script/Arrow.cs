using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class Arrow : MonoBehaviourPunCallbacks
{
    GameObject hit;
    Rigidbody rig;
    private void Start() {
        rig = this.GetComponent<Rigidbody>();
        hit = GameObject.Find("Hit");
        hit.SetActive(false);
    }
    public void Hit(bool hit){
        if(hit){
            FixedJoint fj = this.gameObject.AddComponent<FixedJoint>();
            fj.connectedBody = GameObject.Find("Target").GetComponent<Rigidbody>();
            rig.constraints = RigidbodyConstraints.FreezePositionY |
                              RigidbodyConstraints.FreezePositionZ;
            rig.freezeRotation = true;
        }
    }
    public void Init(Vector3 originPos, Quaternion originRot) {
        transform.position = originPos;
        transform.rotation = originRot;
    }
    void OnCollisionEnter(Collision col){
        if(photonView.IsMine){
            if(col.gameObject.tag == "Target"){
                hit.SetActive(true);
                Animator anim = hit.GetComponent<Animator>();
                Text judgeTxt = hit.GetComponent<Text>();
                anim.SetBool("hit",true);
                judgeTxt.text = "+1";
                judgeTxt.color = Colors.Yellow;
            }
            if(col.gameObject.tag == "Monolith"){
                hit.SetActive(true);
                Animator anim = hit.GetComponent<Animator>();
                Text judgeTxt = hit.GetComponent<Text>();
                anim.SetBool("miss",true);
                judgeTxt.text = "Miss";
                judgeTxt.color = Colors.Lightblue;
                rig.constraints = RigidbodyConstraints.FreezeAll;   
            }
        }
        else{
            if(col.gameObject.tag == "Target"){
                hit.SetActive(true);
                Animator anim = hit.GetComponent<Animator>();
                Text judgeTxt = hit.GetComponent<Text>();
                anim.SetBool("hit",true);
                judgeTxt.text = "Hit";
            }
            if(col.gameObject.tag == "Monolith"){
                hit.SetActive(true);
                Animator anim = hit.GetComponent<Animator>();
                Text judgeTxt = hit.GetComponent<Text>();
                anim.SetBool("miss",true);
                judgeTxt.text = "Miss";
                rig.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }
}