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


    private void Start()
    {
        GameStatistics.StartTime = System.DateTime.UtcNow;
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
}
