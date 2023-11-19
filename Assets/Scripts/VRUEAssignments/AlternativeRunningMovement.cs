using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.Rendering.DebugUI;

public class AlternativeRunningMovement: MonoBehaviour
{

    public GameObject characterControllerGameObject;
    private CharacterController characterController;

    private InputDevice leftController;
    private InputDevice rightController;
    private InputDevice hmd;

    private Quaternion hmdRotation;
    private Vector3 controllerVelocityLeft = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 controllerVelocityRight = new Vector3(0.0f, 0.0f, 0.0f);

    private float currentVelocity = 0;
    private float jumpVelocity = 0;
    private Queue<float> movingAverageVelocityQueue = new Queue<float>();
    private int queueSize = 50;
    private float controllerAverageVelocity;

    private float gravity = -1f;


    // Start is called before the first frame update
    void Start()
    {
        characterController = characterControllerGameObject.GetComponent<CharacterController>();
        characterController.detectCollisions = true;
        InitVRControllers();

        // init queue
        for (int i = 0; i < queueSize; i++)
        {
            movingAverageVelocityQueue.Enqueue(0f);
        }
    }

    private void InitVRControllers()
    {
        // find left controller
        var desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InitializeInputDeviceCharacteristics(desiredCharacteristics, ref leftController);

        // find right controller
        desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InitializeInputDeviceCharacteristics(desiredCharacteristics, ref rightController);


        // find hmd
        desiredCharacteristics = InputDeviceCharacteristics.HeadMounted;
        InitializeInputDeviceCharacteristics(desiredCharacteristics, ref hmd);
    }

    private void InitializeInputDeviceCharacteristics(InputDeviceCharacteristics inputDeviceCharacteristics, ref InputDevice device)
    {
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(inputDeviceCharacteristics, inputDevices);
        if (inputDevices.Count > 0)
        {
            device = inputDevices[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!leftController.isValid || !rightController.isValid)
        {
            InitVRControllers();
        }
        if (leftController.TryGetFeatureValue(CommonUsages.deviceVelocity, out var velocityL)
            && rightController.TryGetFeatureValue(CommonUsages.deviceVelocity, out var velocityR))
        {
            if (!IsBelowThreshold(velocityL, controllerVelocityLeft, 0.5f) && !IsBelowThreshold(velocityR, controllerVelocityRight, 0.5f))
            {
                controllerVelocityRight = velocityR;
                controllerVelocityLeft = velocityL;
                controllerAverageVelocity = (Math.Abs(controllerVelocityLeft.y) + Math.Abs(controllerVelocityRight.y)) / 2 * Time.deltaTime * 3;
                if (IsRunning())
                {
                    Debug.Log("I am running");
                    WriteToQueue(controllerAverageVelocity);

                }
                else if (IsJumping() && characterController.isGrounded)
                {
                    
                    Debug.Log("I am jumping!");
                    Debug.Log("Setting velocity to " + jumpVelocity);
                    jumpVelocity += Mathf.Sqrt(-0.05f * gravity);
                }
            } else
            {
                StartCoroutine(WaitAndWriteToQueue());
            }
        }

        if (hmd.TryGetFeatureValue(CommonUsages.deviceRotation, out var rotation))
        {
            hmdRotation = rotation;
        }

        StartCoroutine(calcVelocityFromMovingAvg());
        //Debug.Log("Current velocity moving average: " + currentVelocity.ToString());
        updateCharacterMovement(currentVelocity);
    }

    private void updateCharacterMovement(float velocity)
    {
        Vector3 direction = hmdRotation * new Vector3(0.0f, 0.0f, velocity);
        if (!characterController.isGrounded)
        {
            jumpVelocity += gravity * Time.deltaTime;
        }
        else if (jumpVelocity < 0.0f)
        {
            jumpVelocity = 0;
        }
        direction.Set(direction.x, jumpVelocity, direction.z);
        // Debug.Log("Velocity is " + direction);
        characterController.Move(direction);
    }

    private void WriteToQueue(float value)
    {
        movingAverageVelocityQueue.Dequeue();
        movingAverageVelocityQueue.Enqueue(value);
    }
    private IEnumerator WaitAndWriteToQueue()
    {
        controllerAverageVelocity -= 0.001f;

        if (controllerAverageVelocity < 0)
        {
            controllerAverageVelocity = 0;
        }
        WriteToQueue(controllerAverageVelocity);
        yield return new WaitForSeconds(1f);
    }
    private IEnumerator calcVelocityFromMovingAvg()
    {
        var sum = 0f;
        foreach (var value in movingAverageVelocityQueue)
        {
            sum += value;
        }
        currentVelocity = sum / movingAverageVelocityQueue.Count;
        yield return currentVelocity;
    }

    private bool IsJumping()
    {
        var yLeft = controllerVelocityLeft.y;
        var yRight = controllerVelocityRight.y;

        // check if both controllers are moving up
        return yLeft > 0.9 && yRight > 0.9;
    }

    private bool IsRunning()
    {
        var yLeft = controllerVelocityLeft.y;
        var yRight = controllerVelocityRight.y;

        // check if the controllers are moving in opposite direction e.g. one is moving up, the other down
        return (yLeft > 0 && yRight < 0) || (yLeft < 0 && yRight > 0);
    }

    private bool IsBelowThreshold(Vector3 a, Vector3 b, float threshold)
    {
        return Vector3.SqrMagnitude(a - b) < threshold;
    }
}

