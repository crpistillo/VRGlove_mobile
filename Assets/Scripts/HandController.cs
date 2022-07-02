using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class HandController : MonoBehaviour
{
    private KalmanFilterFloat kalmanFilterX;
    private KalmanFilterFloat kalmanFilterY;
    private KalmanFilterFloat kalmanFilterZ;
    
    public List<Finger> fingers;   
    public BLEController BLEController; 
    private const float MAX_ANGLE = 235.0f;
    private const float MIN_ANGLE = -15.0f;

    private Transform handTransform;
    private Quaternion rawQuat;
    //static MadgwickAHRS AHRS = new MadgwickAHRS(1f / 256f, 0.1f);
    public List<float> gyroXOffsets; 
    public List<float> gyroYOffsets; 
    public List<float> gyroZOffsets; 
    public List<float> gyroOffsets; 
    bool gyroCalibrated = false;

    float previousX = 0;
    float previousY = 0;
    float previousZ = 0;

    // original rotation of our object
    Quaternion initialObjRotation;
    
    // original rotation of the IMU
    Quaternion initialIMURotation;
    bool imuInitialized = false;
    bool handIsMoving = false;

    float initialX;
    float initialY;
    float initialZ;

    
    // Temporary quaternion (kept around just to for efficiency)
    Quaternion diff;

    void Start() {
        handTransform = GetComponent<Transform>();
        initialObjRotation = handTransform.rotation;
        diff = new Quaternion(); 
        kalmanFilterX = new KalmanFilterFloat(0.0001f, 0.01f);
        kalmanFilterY = new KalmanFilterFloat(0.0001f, 0.01f);
        kalmanFilterZ = new KalmanFilterFloat(0.0001f, 0.01f);
        initialX = handTransform.rotation.eulerAngles.x;
        initialY = handTransform.rotation.eulerAngles.y;
        initialZ = handTransform.rotation.eulerAngles.z;
    }

    void Update()
    {
        if(BLEController.anglesNotAvailable() || BLEController.mpuValuesNotAvailable()) {

            return;
        } else {
            /*if(!gyroCalibrated) {

                StartCoroutine(CalibrateGyro());
                return;
            }

            if(!handIsMoving) {
                StartCoroutine(RotateHand());
            }*/
            
            for(int i = 0 ; i < fingers.Count ; i++) {
                if(!fingers[i].isMoving()) {
                    if(invalidAngle(BLEController.getFingerAngle(i))) {
                        return;
                    }
                    StartCoroutine(fingers[i].updateAngle(BLEController.getFingerAngle(i)));   
                } 
            }
        }
    }

    public IEnumerator RotateHand() {
        handIsMoving = true;

        float x = BLEController.getMpuValue(2) - gyroOffsets[0]; //y

        float x_kalman = x - 45;

        if(x_kalman <= -180) {
            x_kalman = x_kalman + 360;
        } else if (x_kalman >= 180) {
            x_kalman = x_kalman - 360;
        }       

        print("x: " + x);
        if (x > 180 || x < -180) {
            print("DISMISSING X!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            x = previousX;
        }
        else {
            x = kalmanFilterX.Update(x_kalman) + 45;
        }

        float y = BLEController.getMpuValue(0) - gyroOffsets[1]; //x
        float z = BLEController.getMpuValue(1) - gyroOffsets[2];;

        if(z <= -180 || z >= 180) {
            print("DISMISSING Z!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            z = previousZ;
        }
        else {
            z = kalmanFilterZ.Update(z);
        }

        print("z: " + (BLEController.getMpuValue(1) - gyroOffsets[2]));
        print("corrected z: " + z);

        //corregir dy porque hace cosas raras
        handTransform.rotation = Quaternion.Euler(initialX/* - dy + 270*/, initialY - x, initialZ - z);

        previousX = x;
        previousZ = z;

        yield return null;
        //yield return new WaitForSeconds(BLEController.UPDATE_RATE);
        handIsMoving = false;
    }

    public void HandleIMURotation(Quaternion imuRot) {
        if (!imuInitialized) {
            // This is the first IMU reading; just store it as
            // the initial IMU rotation.
            initialIMURotation = imuRot;
            imuInitialized = true;
        } else {
            // This is a subsequent reading; find out how the
            // IMU has changed since the start, and apply that
            // same change to our object.
            diff = imuRot * Quaternion.Inverse(initialIMURotation);
            handTransform.rotation = diff * initialIMURotation;
        }
    }

    IEnumerator CalibrateGyro()
    {
        float timePassed = 0;
        while (timePassed < 10)
        {
            print("Calibrating");
            print("timePassed: " + timePassed);

            gyroXOffsets.Add(BLEController.getMpuValue(2));
            gyroYOffsets.Add(BLEController.getMpuValue(0));
            gyroZOffsets.Add(BLEController.getMpuValue(1));

            timePassed += Time.deltaTime;
            yield return null;
        }
        float xOffset = Queryable.Average(gyroXOffsets.AsQueryable());
        float yOffset = Queryable.Average(gyroYOffsets.AsQueryable());
        float zOffset = Queryable.Average(gyroZOffsets.AsQueryable());

        gyroOffsets = new List<float> { xOffset, yOffset, zOffset};


        gyroCalibrated = true;
    }

    bool invalidAngle(float angle) {
        return angle > MAX_ANGLE || angle < MIN_ANGLE;
    }

    static float deg2rad(float degrees)
    {
        return (float)(Math.PI / 180) * degrees;
    }
}
