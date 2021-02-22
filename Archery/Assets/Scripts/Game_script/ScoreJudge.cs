using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScoreJudge : MonoBehaviour
{
    Animator anim;
    TextMesh txtM;
    Color color;
    void Start(){
        anim = this.gameObject.GetComponent<Animator>();
        txtM = this.gameObject.GetComponent<TextMesh>();
        color = txtM.color;
    }
    public void SetAnim(string judge){
        switch(judge){
            case "Hit":
                anim.SetBool("hit",true);
                txtM.text = "+10";
                color = Colors.Yellow;
            break;
            case "Miss":
                anim.SetBool("miss",true);
                txtM.text = "Miss";
                color = Colors.Lightblue;
            break;
            case "Reset":
                anim.SetBool("hit",false);
                anim.SetBool("miss",false);
            break;
        }
    }
}
