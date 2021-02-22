using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelStart : MonoBehaviour
{
    public GameObject p;
    void Start(){
        p.SetActive(false);
    }
    public void StartPanel(){
        p.SetActive(true);
    }
}
