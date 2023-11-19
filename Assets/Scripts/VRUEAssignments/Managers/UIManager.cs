using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Template.VR.VRUEAssignments.Structures;
using UnityEngine;
using UnityEngine.UI;
using VRUEAssignments.Managers;
using VRUEAssignments.Utils;

public class UIManager : UnitySingleton<UIManager>
{
    public GamingEnvironment AGamingEnvironment;
    public Slider GEScaleSlider;
    public TextMeshProUGUI HolesHitLabel;
    public TextMeshProUGUI CueHitsLabel;
    public TextMeshProUGUI TimeLabel;
    public GameObject LogTextPrefab;

    public Image LogPanel;
    private Coroutine _logPanelFade;
    private Color _logPanelShown = new Color(0, 0, 0, 0.4f);
    private Color _logPanelHidden = new Color(0, 0, 0, 0.0f);
    private float _fadeDuration = 5f;

    
    private void Start()
    {
        GameStatistics.StartTime = System.DateTime.UtcNow;

        SetLogPanelColor(_logPanelHidden);
    }

    private void SetLogPanelColor(Color color)
    {
        LogPanel.color = color;
    }

    public void SetCurrentThrowable(ThrowableType index)
    {
        ThrowableSpawner.Instance.SetCurrentThrowable(index);
    }
    
    public void SetCurrentThrowable(int index)
    {
        ThrowableSpawner.Instance.SetCurrentThrowable(index);
    }

    public void SpawnCurrentThrowable()
    {
        ThrowableSpawner.Instance.SpawnThrowable();
    }

    public void SetStructureAmount(float amount)
    {
        SetStructureAmount((int)amount);
    }
    
    public void SetStructureAmount(int amount)
    {
        StructureSpawner.Instance.SetAmount(amount);
    }
    
    public void SpawnStructures()
    {
        StructureSpawner.Instance.SpawnStructures();
    }

    public void Restart()
    {
        AGamingEnvironment.Restart();
    }

    public void AllowScaling(bool allowed)
    {
        AGamingEnvironment.AllowScaling(allowed);
    }

    public void UpdateGamingEnvironmentScale(float localScaleX)
    {
        GEScaleSlider.value = localScaleX;
    }

    public void UpdateHolesHit()
    {
        HolesHitLabel.SetText("Holes Hit: " + GameStatistics.HolesHit.ToString());
    }

    public void UpdateTime()
    {
        GameStatistics.TimeElapsed = (System.DateTime.UtcNow - GameStatistics.StartTime).Seconds;
        TimeLabel.SetText("Time: " + GameStatistics.TimeElapsed.ToString() + "s");
    }

    private void Update()
    {
        UpdateTime();
    }

    public void UpdateCueHits()
    {
        CueHitsLabel.SetText("Hits: " + GameStatistics.CueHits.ToString());
    }

    public void DisplayUIMessage(string message)
    {
        // Fade
        if (_logPanelFade != null)
        {
            StopCoroutine(_logPanelFade);
        }
        _logPanelFade = StartCoroutine(ChangeLogPanelColor(_logPanelShown, _logPanelHidden, _fadeDuration));

        // Create & Destroy
        StartCoroutine(CreateAndDestroyLogText(message, _fadeDuration));
    }

    private IEnumerator ChangeLogPanelColor(Color start, Color end, float duration) {
        for (float t = 0f; t < duration; t += Time.deltaTime) {
            float normalizedTime = t/duration;
            SetLogPanelColor(Color.Lerp(start, end, normalizedTime));
            
            yield return null;
        }
        SetLogPanelColor(end);
    }

    private IEnumerator CreateAndDestroyLogText(string message, float duration)
    {
        GameObject logText = Instantiate(LogTextPrefab, LogPanel.transform);
        logText.GetComponent<TextMeshProUGUI>().SetText(message);

        yield return new WaitForSeconds(duration);
        
        Destroy(logText);
    }
}
