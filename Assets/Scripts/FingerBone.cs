using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FingerBone : MonoBehaviour {
    public float minZAngle;
    public float maxZAngle;

    private float speed() {
        return Math.Abs(maxZAngle - minZAngle) / (60 * FingerConstants.TIME_TO_FLEX);
    }

    public void flex() {
        gameObject.transform.Rotate(0, 0, -speed());
    }

    public IEnumerator Flex(float startingAngle, float endAngle) {
        gameObject.transform.Rotate(0, 0, -speed(startingAngle, endAngle));
        yield return new WaitForEndOfFrame();
    }

    private float speed(float startingAngle, float endAngle) {
        return (endAngle - startingAngle);
    }

    public bool flexed() {
        return gameObject.transform.localRotation.eulerAngles.z - 360 <= maxZAngle;
    }

    public void strech() {
        gameObject.transform.Rotate(0, 0, speed());
    }

    public bool streched() {
        return gameObject.transform.localRotation.eulerAngles.z - 360 >= minZAngle;
    }
}