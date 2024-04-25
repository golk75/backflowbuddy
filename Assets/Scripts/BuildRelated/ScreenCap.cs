using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCap : MonoBehaviour
{

    void Update()
    {
        int count = 1;

        if (Input.GetKeyDown(KeyCode.Space))
        { ScreenCapture.CaptureScreenshot($"/Users/GregP/Desktop/screenshot{count++}.png"); }

    }
}
