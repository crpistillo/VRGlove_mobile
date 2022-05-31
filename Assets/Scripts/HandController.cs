using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandController : MonoBehaviour
{
    public List<Finger> fingers;    

    void Start()
    {
        
    }
    /*
    IEnumerator RotateIndex()
    {
        indexBones[0].transform.Rotate(0, 0, -index1Velocity);
        indexBones[1].transform.Rotate(0, 0, -index2Velocity);
        indexBones[2].transform.Rotate(0, 0, -index3Velocity);
        yield return null;
    }*/
 
    // Update is called once per frame
    void Update()
    {
        foreach (Finger finger in fingers)
        {
            if(!finger.flexed()) {
                StartCoroutine(finger.flex());
            }
        }
        /*if(indexBones[0].transform.localRotation.eulerAngles.z >= INDEX_1_MAX + 360) {
            StartCoroutine(RotateIndex());
        }*/
        
    }
    
}
