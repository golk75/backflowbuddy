using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UIElements;

public class TeachingModeToggle : MonoBehaviour
{
    UIDocument root;

    public Toggle toggle;

    public WaterController waterController;



    void Start()
    {
        root = GetComponent<UIDocument>();
        toggle = root.rootVisualElement.Q<Toggle>("TeachingMode_toggle");
        toggle.RegisterValueChangedCallback(TeachingModeToggled);

    }

    private void TeachingModeToggled(ChangeEvent<bool> evt)
    {
        if (waterController.isTeachingModeEnabled == false)
        {
            waterController.isTeachingModeEnabled = true;
        }
        else
        {
            waterController.isTeachingModeEnabled = false;
        }


    }



    void Update()
    {

    }
}
