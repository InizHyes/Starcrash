using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer_UI : MonoBehaviour
{
    // Variables
    public Image uifil;

    public int TotalTime;

    private int TimeRemaining;

    // Start is called before the first frame update
    private void Start()
    {
        // Sets the start time
        being(TotalTime);
    }

    private void being(int Second)
    {
        // Starts the timer
        TimeRemaining = Second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        // Updates the timer every second
        while(TimeRemaining >= 0)
        {
            uifil.fillAmount = Mathf.InverseLerp(0, TotalTime, TimeRemaining);
            TimeRemaining--;
            yield return new WaitForSeconds(1f);
        }
        OnEnd();
    }

    private void OnEnd()
    {
        // Prints out text to say timer is ended
        print("End");
    }
}
