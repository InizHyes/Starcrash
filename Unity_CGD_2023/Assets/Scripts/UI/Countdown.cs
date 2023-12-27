using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    // Variables
    public Image uiFill;

    public Image uiBackground;

    public int totalTime;

    private int timeRemaining;

    public TextMeshProUGUI text;

    public TextMeshProUGUI go_text;

    // Start is called before the first frame update
    private void Start()
    {
        // Sets the start time
        begin(totalTime);
        go_text.enabled = false;
    }

    private void begin(int Second)
    {
        // Starts the timer
        timeRemaining = Second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        // Updates the timer every second
        while (timeRemaining >= 0)
        {
            uiFill.fillAmount = Mathf.InverseLerp(0, totalTime, timeRemaining);
            timeRemaining--;
            yield return new WaitForSeconds(1f);
        }
        OnEnd();
    }

    private void OnEnd()
    {
        // Prints out text to say timer is ended
        print("Go");
        text.enabled = false;
        go_text.enabled = true;
        uiFill.enabled = false;
        uiBackground.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            go_text.enabled = false;
        }
    }
}
