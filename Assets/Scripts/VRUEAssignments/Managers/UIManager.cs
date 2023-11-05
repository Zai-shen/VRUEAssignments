using System.Collections;
using System.Collections.Generic;
using Unity.Template.VR.VRUEAssignments.Structures;
using UnityEngine;
using UnityEngine.UI;
using VRUEAssignments.Managers;
using VRUEAssignments.Utils;

public class UIManager : UnitySingleton<UIManager>
{
    public GamingEnvironment AGamingEnvironment;
    public Slider GEScaleSlider;
    
    private void Start()
    {
        //SpawnStructures();
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
}
