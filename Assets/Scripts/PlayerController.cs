using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerInputAction playerInput;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInputAction();
        Checkvalve1InitPos = CheckValve1.transform.localPosition;
        Checkvalve2InitPos = CheckValve2.transform.localPosition;
        testCockClosedScale = TestCockValve1.transform.localScale;
        testCockClosedYScale = testCockClosedScale.y;

        //touch input

        //Touch0
        playerInput.Touchscreen.Touch0Contact.started += Touch0Contact_started;
        playerInput.Touchscreen.Touch0Contact.canceled += Touch0Contact_canceled;
        playerInput.Touchscreen.Touch0Delta.started += Touch0Delta_started;
        playerInput.Touchscreen.Touch0Delta.canceled += Touch0Delta_canceled;
        //Touch1
        playerInput.Touchscreen.Touch1Contact.started += Zoom_started;
        playerInput.Touchscreen.Touch1Contact.canceled += Zoom_canceled;
    }

    void OnEnable()
    {
        playerInput.Enable();
    }

    void OnDisable()
    {
        playerInput.Disable();
    }

    void Start() { }

    // Update is called once per frame
    void Update() { }
}
