using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VRUEAssignments.Managers;

public class ScoreboardText : MonoBehaviour
{

    public TextMeshPro readyText;
    public TextMeshPro timeText;
    private System.DateTime startTime;
    private bool isTimeRunning;

    // Start is called before the first frame update
    void Start()
    {
        GameStatistics.StartTime = startTime;
        isTimeRunning = false;
        timeText.SetText("Time: 0s");
        StartCoroutine(DisplayReadySetGo());
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimeRunning)
        {
            var timeElapsed = System.DateTime.UtcNow - startTime;
            GameStatistics.TimeElapsed = (timeElapsed).Seconds;
            timeText.SetText("Time: " + timeElapsed + "s");
        }
    }

    private IEnumerator DisplayReadySetGo()
    {
        readyText.SetText("Ready");
        yield return new WaitForSeconds(1);
        readyText.SetText("Set");
        yield return new WaitForSeconds(1);
        readyText.SetText("Go!");
        isTimeRunning = true;
       
    }

    
}
