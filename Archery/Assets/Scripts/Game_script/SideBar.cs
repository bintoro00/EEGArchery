using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideBar : MonoBehaviour
{
    Animator anim;
    bool reverse = false;

    void Start(){
        anim = this.GetComponent<Animator>();
    }
    public void SideBarBtn(){
        if(!reverse){
            anim.SetBool("off",false);
            anim.SetBool("on",true);
            reverse = true;
        }
        else{
            anim.SetBool("on",false);
            anim.SetBool("off",true);
            reverse = false;
        }
    }
}
