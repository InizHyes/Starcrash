using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timer_Text;
    [SerializeField] TextMeshProUGUI gameover;
    [SerializeField] float remainingTime;

    private void Start()
    {
        gameover.enabled = false;
    }
    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if(remainingTime < 0)
        {
            remainingTime = 0;
            gameover.enabled = true;
            timer_Text.enabled = false;
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timer_Text.text = string.Format("{0:00}:{1:00}",minutes, seconds);
    }
}
