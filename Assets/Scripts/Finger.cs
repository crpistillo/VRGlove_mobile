using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Finger : MonoBehaviour {
    public List<FingerBone> fingerBones;
    private float previousAngle = 0;
    private bool _isMoving = false;
    public float ADJUST_FACTOR = 0.6f;
    private KalmanFilterFloat kalmanFilter;

    void Start() {
        kalmanFilter = new KalmanFilterFloat();
    }

    public bool isMoving() {
        return _isMoving;
    }

    public IEnumerator updateAngle(float angle) {
        _isMoving = true;

        float value = kalmanFilter.Update(angle) * ADJUST_FACTOR;
        print("corrected value: " + value);

        if (Math.Abs(value - angle) >= 10) 

        /*if( value >= 185) {
            value = 185;
        }*/
        
        flexBones(previousAngle, value);
        
        previousAngle = value;

        yield return new WaitForSeconds(BLEController.UPDATE_RATE);
        _isMoving = false;
    }

    public void flexBones() {
        if(!flexed()) {
            foreach(FingerBone fingerBone in fingerBones) {
                fingerBone.flex();
            }
        }
    }

    public void flexBones(float startingAngle, float endAngle) {
        if(flexed()) {
            if(endAngle < startingAngle) {
                foreach(FingerBone fingerBone in fingerBones) {
                    StartCoroutine(fingerBone.Flex(startingAngle, endAngle));
                }
            }
            return;
        }
        if(streched()) {
            if(endAngle > startingAngle) {
                foreach(FingerBone fingerBone in fingerBones) {
                    StartCoroutine(fingerBone.Flex(startingAngle, endAngle));
                }
            }
            return;
        }
        float diff = endAngle - startingAngle;
        if(fingerBones[0].gameObject.transform.localRotation.eulerAngles.z - diff - 360 <= fingerBones[0].maxZAngle) {
            diff = fingerBones[0].gameObject.transform.localRotation.eulerAngles.z - fingerBones[0].maxZAngle;
             foreach(FingerBone fingerBone in fingerBones) {
                StartCoroutine(fingerBone.Flex(0, diff));
            }
            return;
        }
        if(fingerBones[0].gameObject.transform.localRotation.eulerAngles.z - diff - 360 >= fingerBones[0].minZAngle) {
            diff = fingerBones[0].minZAngle - fingerBones[0].gameObject.transform.localRotation.eulerAngles.z;
             foreach(FingerBone fingerBone in fingerBones) {
                StartCoroutine(fingerBone.Flex(0, diff));
            }
            return;
        }
        
        foreach(FingerBone fingerBone in fingerBones) {
            StartCoroutine(fingerBone.Flex(startingAngle, endAngle));
        }
    }

    public bool flexed() {
        return fingerBones[0].flexed();
    }

    public void strechBones() {
        if(!streched()) {
            foreach(FingerBone fingerBone in fingerBones) {
                fingerBone.strech();
            }
        }
    }

    public bool streched() {
        return fingerBones[0].streched();
    }
}