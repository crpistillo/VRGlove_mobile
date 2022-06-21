using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandController : MonoBehaviour
{
    public List<Finger> fingers;   
    public BLEController BLEController; 
    
    private float previousAngle = 0.221f;
    private bool isRunning = false;
    private KalmanFilterFloat kalmanFilter;
    private const float ADJUST_FACTOR = 0.6f;
    private const float MAX_ANGLE = 135.0f;
    private const float MIN_ANGLE = -15.0f;

    void Start() {
        kalmanFilter = new KalmanFilterFloat();
    }

    void Update()
    {
        if(BLEController.getStringAngle() == "No angle") {
            return;
        } else {
            if(!isRunning) {
                if(invalidAngle()) {
                    return;
                }
                StartCoroutine(updateFingerAngle());   
            }
        }
    }

    IEnumerator updateFingerAngle() {
        isRunning = true;
        float value = kalmanFilter.Update(BLEController.getAngle()) * ADJUST_FACTOR;
        fingers[1].flexBones(previousAngle, value);
        previousAngle = value;
        yield return new WaitForSeconds(BLEController.UPDATE_RATE);
        isRunning = false;
    }

    bool invalidAngle() {
        return BLEController.getAngle() > MAX_ANGLE || BLEController.getAngle() < MIN_ANGLE;
    }
}
