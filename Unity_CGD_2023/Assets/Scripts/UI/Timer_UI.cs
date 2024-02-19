using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer_UI : MonoBehaviour
{
    public Image timer_bar;
    public float maxTime = 10f;
    float timeleft;
    //public TextMeshProUGUI gameovertext;

    private void Start()
    {
        //gameovertext.enabled = false;
        timer_bar = GameObject.Find("Image (1)").GetComponent<Image>();
        timeleft = maxTime;
    }

    private void Update()
    {
        if (timeleft > 0)
        {
             timeleft -= Time.deltaTime;
             timer_bar.fillAmount = timeleft / maxTime;
        }
        else if(timeleft <= 0)
        {
            //gameovertext.enabled = true;
            print("End");
       
        }
    }
}
