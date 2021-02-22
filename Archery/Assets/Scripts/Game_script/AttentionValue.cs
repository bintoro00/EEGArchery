using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttentionValue : MonoBehaviour
{
    public Slider attention;
    public Button atnBtn;
    public PhotonManager pm;
    private Color32 white_color = new Color32(255, 255, 255, 255);
    Text atnTxt;
    void Start(){
        atnTxt = this.GetComponent<Text>();
    }
    void Update(){
        atnTxt.text = attention.value.ToString();
        if(attention.value>=attention.maxValue){
            atnBtn.GetComponent<Image>().color = Colors.White;
        }
        else{
            atnBtn.GetComponent<Image>().color = Colors.Darkgray;
        }
    }
}
