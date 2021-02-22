using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSlider : MonoBehaviour
{
    public TimeCounter timer;
    void Start(){
        Slider s = this.GetComponent<Slider>();
        s.maxValue = timer.countdown;
    }
    void Update(){
        Slider s = this.GetComponent<Slider>();
        s.value = timer.countdown;
    }
}
