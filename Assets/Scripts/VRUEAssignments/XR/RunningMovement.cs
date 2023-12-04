using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace VRUEAssignments.XR
{
    public class RunningMovement : MonoBehaviour
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
        private float minVelocity = 0;
        private float maxVelocity = 0;

        private bool isPerformingRunning;

        float increaseTimeElapsed;
        float decreaseTimeElapsed;
        float lerpDuration = 1f;

        // Start is called before the first frame update
        void Start()
        {
            characterController = characterControllerGameObject.GetComponent<CharacterController>();
            characterController.detectCollisions = true;

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
            if (leftController.TryGetFeatureValue(CommonUsages.deviceVelocity, out var velocityL)
                && rightController.TryGetFeatureValue(CommonUsages.deviceVelocity, out var velocityR))
            {
                if (!IsBelowThreshold(velocityL, controllerVelocityLeft, 0.5f) && !IsBelowThreshold(velocityR, controllerVelocityRight, 0.5f))
                {
                    controllerVelocityRight = velocityR;
                    controllerVelocityLeft = velocityL;
                    if (IsRunning() && !isPerformingRunning)
                    {
                        Debug.Log("Start performing running");
                        isPerformingRunning = true;
                        var controllerAverageVelocity = (Math.Abs(controllerVelocityLeft.y) + Math.Abs(controllerVelocityRight.y)) / 2;
                        minVelocity = currentVelocity;
                        maxVelocity = controllerAverageVelocity * Time.deltaTime * 3;
                        lerpDuration = Math.Abs(minVelocity - maxVelocity) * 10;

                        Debug.Log("Max velocity is: " + maxVelocity);
                        Debug.Log("Min velocity is: " + minVelocity);
                        Debug.Log("Lerp duration is: " + lerpDuration);

                    }
                    else if (IsJumping())
                    {
                        isPerformingRunning = false;
                        Debug.Log("I am jumping!");
                        // TODO implement jumping
                    }
                    else
                    {
                        isPerformingRunning = false;
                    }

                }
            }

            if (hmd.TryGetFeatureValue(CommonUsages.deviceRotation, out var rotation))
            {
                hmdRotation = rotation;
            }
            currentVelocity = getUpdatedVelocityValue();
            updateCharacterMovement(currentVelocity);
        }

        private void updateCharacterMovement(float velocity)
        {
            Vector3 direction = hmdRotation * new Vector3(0.0f, 0.0f, velocity);
            direction.Set(direction.x, 0, direction.z);
            characterController.Move(direction);
        }

        private float getUpdatedVelocityValue()
        {
            if (!isPerformingRunning)
            {
                if (currentVelocity > 0 && decreaseTimeElapsed < lerpDuration)
                {
                    // smoothly decreasing speed
                    var newVelocity = Mathf.Lerp(maxVelocity, 0, decreaseTimeElapsed / lerpDuration);
                    //Debug.Log("Decreasing velocity with t value " + decreaseTimeElapsed / lerpDuration + " to " + currentVelocity);
                    decreaseTimeElapsed += Time.deltaTime;
                    return newVelocity;
                }
                else
                {
                    decreaseTimeElapsed = 0;
                    return 0;
                }
            }
            else {
                if (increaseTimeElapsed < lerpDuration)
                {
                    // smoothly increasing speed
                    // TODO instead of increasing from 0 to run velocity increase from old velocity to new velocity
                    var newVelocity = Mathf.Lerp(minVelocity, maxVelocity, increaseTimeElapsed / lerpDuration);
                    //Debug.Log("Increasing velocity with t value " + increaseTimeElapsed / lerpDuration + " to " + currentVelocity);
                    increaseTimeElapsed += Time.deltaTime;
                    return newVelocity;
                } else
                {
                    increaseTimeElapsed = 0;
                    isPerformingRunning = false;
                    Debug.Log("velocity reached " + currentVelocity);
                    Debug.Log("End performing acceleration.");
                    return currentVelocity;
                }
            } 
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
}

