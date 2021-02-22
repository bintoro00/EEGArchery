using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public GameObject fade;
    float t = 0.0f;
    void Start(){
        fade.SetActive(true);
    }
    void Update(){
        t += Time.deltaTime;
        if(t >= 1.2f){
            fade.SetActive(false);
        }
    }
}
