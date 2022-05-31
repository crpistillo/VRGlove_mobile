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

    public void flex()
    {
        gameObject.transform.Rotate(0, 0, -speed());
    }

    public bool finishedRotation() {
        return gameObject.transform.localRotation.eulerAngles.z <= maxZAngle + 360;
    }
}