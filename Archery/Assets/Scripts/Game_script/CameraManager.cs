using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class CameraManager : MonoBehaviourPunCallbacks
{
    public Camera subCam1,subCam2,subCam3,subCam4;
    bool Out;
    Animator cameraAnim;
    
    void Start(){
        cameraAnim = this.GetComponent<Animator>();
        cameraAnim.SetBool("zoom", false);
    }
    public void Camera2(){
        subCam1.transform.position = new Vector3(0f,2f,-10f);
    }
    public void Camera3(){
        subCam1.transform.position = new Vector3(0f,2f,-10f);
        subCam2.transform.position = new Vector3(15f,2f,-10f);
    }
    public void Camera4(){
        subCam1.transform.position = new Vector3(0f,2f,-10f);
        subCam2.transform.position = new Vector3(15f,2f,-10f);
        subCam3.transform.position = new Vector3(30f,2f,-10f);
    }
    public void Camera5(){
        subCam1.transform.position = new Vector3(0f,2f,-10f);
        subCam2.transform.position = new Vector3(150f,2f,-10f);
        subCam3.transform.position = new Vector3(30f,2f,-10f);
        subCam4.transform.position = new Vector3(45f,2f,-10f);
    }
    public void mainCamera(){
        if(!Out){
            cameraAnim.SetBool("zoom", true);
            Out = true;
        }
        else{
            cameraAnim.SetBool("zoom", false);
            Out = false; 
        }
    }
}
