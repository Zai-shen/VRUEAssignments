using System.Collections;
using System.Collections.Generic;
using Unity.Template.VR.VRUEAssignments.Structures;
using UnityEngine;
using VRUEAssignments.Managers;

public class UIManager : MonoBehaviour
{
    public GamingEnvironment AGamingEnvironment;

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
}
