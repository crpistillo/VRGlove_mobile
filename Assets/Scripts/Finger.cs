using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Finger : MonoBehaviour {
    public List<FingerBone> fingerBones;

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