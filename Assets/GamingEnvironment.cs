using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class GamingEnvironment : MonoBehaviour
{
    public GameObject PoolBallSetPrefab;
    public GameObject PoolSpace;
    
    public float Scale = 1f;

    private XRGeneralGrabTransformer _xrGeneralGrabTransformer;
    private bool _scalingAllowed = false;
    
    void Start()
    {
        _xrGeneralGrabTransformer = GetComponent<XRGeneralGrabTransformer>();
    }
    
    
    public void AllowScaling(bool allowed)
    {
        _scalingAllowed = allowed;
        _xrGeneralGrabTransformer.allowTwoHandedScaling = _scalingAllowed;
    }

    [ContextMenu("Restart")]
    public void Restart()
    {
        ResetBallPositions();
    }

    private void ResetBallPositions()
    {
        StartCoroutine(ResetRoutine());
    }

    private IEnumerator ResetRoutine()
    {
        Destroy(PoolSpace.transform.GetChild(0).gameObject);

        yield return new WaitForSeconds(0.1f);
        
        Instantiate(PoolBallSetPrefab, PoolSpace.transform);

        yield return null;
    }
}
