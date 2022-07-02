using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FingerBone : MonoBehaviour {
    public float minZAngle;
    public float maxZAngle;

    public float realMinZAngle;
    public float realMaxZAngle;

    private float speed() {
        return Math.Abs(maxZAngle - minZAngle) / (60 * FingerConstants.TIME_TO_FLEX);
    }

    public void flex() {
        gameObject.transform.Rotate(0, 0, -speed());
    }

    public IEnumerator Flex(float angle) {
        float computedAngle;

        print("minAngle: " + minZAngle);
        print("maxAngle: " + maxZAngle);
        print("realAngle: "+ gameObject.transform.rotation.eulerAngles.z);

        if(angle >= 170) {
            computedAngle = realMaxZAngle;
        } else if (angle >= -15) {
            computedAngle = (realMaxZAngle - realMinZAngle) * angle / 170 + realMinZAngle;
        } else {
            computedAngle = (realMaxZAngle - realMinZAngle) * 15 / 170 + realMinZAngle;
        }

        float previousX = gameObject.transform.rotation.eulerAngles.x;
        float previousY = gameObject.transform.rotation.eulerAngles.y;

        //print("computed angle: " + computedAngle);

        gameObject.transform.rotation = Quaternion.Euler(previousX, previousY, computedAngle);

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