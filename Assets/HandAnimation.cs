using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class HandAnimation : MonoBehaviour
{
    public InputActionReference GripInputActionReference;
    public InputActionReference TriggerInputActionReference;

    private Animator _animator;
    private float _gripValue;
    private float _triggerValue;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimateGrip();
        AnimateTrigger();
    }
    
    private void AnimateGrip()
    {
        _gripValue = GripInputActionReference.action.ReadValue<float>();
        _animator.SetFloat("Grip", _gripValue);
    }
    
    private void AnimateTrigger()
    {
        _triggerValue = TriggerInputActionReference.action.ReadValue<float>();
        _animator.SetFloat("Trigger", _triggerValue);    }
}
