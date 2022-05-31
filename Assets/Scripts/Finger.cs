using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Finger : MonoBehaviour {
    public List<FingerBone> fingerBones;

    public IEnumerator flex() {
        foreach(FingerBone fingerBone in fingerBones) {
           fingerBone.flex();
        }
        yield return null;
    }

    public bool flexed() {
        return fingerBones[0].finishedRotation();
    }
}