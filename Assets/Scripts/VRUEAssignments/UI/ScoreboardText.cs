using System.Collections;
using TMPro;
using UnityEngine;
using VRUEAssignments.Managers;

namespace VRUEAssignments.UI
{
    public class ScoreboardText : MonoBehaviour
    {

        public TextMeshPro readyText;
        public TextMeshPro timeText;
        private System.DateTime startTime;
        private bool isTimeRunning;

        // Start is called before the first frame update
        void Start()
        {
            isTimeRunning = false;
            timeText.SetText("Time: 0s");
        
        }

        public void StartDisplaying()
        {
            StartCoroutine(DisplayReadySetGo());
        }

        // Update is called once per frame
        void Update()
        {
            if (isTimeRunning)
            {
                var timeElapsed = System.DateTime.UtcNow - startTime;
                GameStatistics.TimeElapsed = (timeElapsed).Seconds;
                timeText.SetText("Time: " + timeElapsed.Seconds.ToString() + "s");
            }
        }

        private IEnumerator DisplayReadySetGo()
        {
            readyText.SetText("Ready");
            yield return new WaitForSeconds(1);
            readyText.SetText("Set");
            yield return new WaitForSeconds(1);
            readyText.SetText("Go!");
            startTime = System.DateTime.UtcNow;
            isTimeRunning = true;
       
        }

    
    }
}
