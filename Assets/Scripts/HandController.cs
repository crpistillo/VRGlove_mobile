using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandController : MonoBehaviour
{
    public GameObject index1;
    public GameObject index2;
    public GameObject index3;

    private static float INDEX_1_MIN = -4.9f;
    private static float INDEX_1_MAX = -75f;
    private float index1Velocity = Math.Abs(INDEX_1_MAX - INDEX_1_MIN) / (60 * 5);

    private static float INDEX_2_MIN = -0.4f;
    private static float INDEX_2_MAX = -105f;
    private float index2Velocity = Math.Abs(INDEX_2_MAX - INDEX_2_MIN) / (60 * 5);

    private static float INDEX_3_MIN = 8.21f;
    private static float INDEX_3_MAX = -70f;
    private float index3Velocity = Math.Abs(INDEX_3_MAX - INDEX_3_MIN) / (60 * 5);

    void Start()
    {
        
    }
    IEnumerator RotateIndex()
    {
        index1.transform.Rotate(0, 0, -index1Velocity);
        index2.transform.Rotate(0, 0, -index2Velocity);
        index3.transform.Rotate(0, 0, -index3Velocity);
        yield return null;
    }
 
    // Update is called once per frame
    void Update()
    {
        if(index1.transform.localRotation.eulerAngles.z >= INDEX_1_MAX + 360) {
            StartCoroutine(RotateIndex());
        }
        
    }
    
}
