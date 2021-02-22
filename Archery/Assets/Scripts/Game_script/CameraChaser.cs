using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChaser : MonoBehaviour
{
    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag=="Finish"){
            this.gameObject.transform.parent = null;
        }
    }
}
