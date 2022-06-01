using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandController : MonoBehaviour
{
    public List<Finger> fingers;    

    void Update()
    {
        if(Input.GetKey(KeyCode.Keypad0)) {
            fingers[0].flexBones();
        }
        if(Input.GetKey(KeyCode.Keypad1)) {
            fingers[0].strechBones();
        }

        if(Input.GetKey(KeyCode.Keypad2)) {
            fingers[1].flexBones();
        }
        if(Input.GetKey(KeyCode.Keypad3)) {
            fingers[1].strechBones();
        }

        if(Input.GetKey(KeyCode.Keypad4)) {
            fingers[2].flexBones();
        }
        if(Input.GetKey(KeyCode.Keypad5)) {
            fingers[2].strechBones();
        }

        if(Input.GetKey(KeyCode.Keypad6)) {
            fingers[3].flexBones();
        }
        if(Input.GetKey(KeyCode.Keypad7)) {
            fingers[3].strechBones();
        }

        if(Input.GetKey(KeyCode.Keypad8)) {
            fingers[4].flexBones();
        }
        if(Input.GetKey(KeyCode.Keypad9)) {
            fingers[4].strechBones();
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            foreach (Finger finger in fingers)
            {
                finger.flexBones();
            }
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            foreach (Finger finger in fingers)
            {
                finger.strechBones();
            }
        }
    }
    
}
