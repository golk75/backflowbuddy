using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UIElements;

public class ClickEnableButton : MonoBehaviour
{
    UIDocument root;

    public Toggle toggle;

    public PlayerController playerController;



    void Start()
    {
        root = GetComponent<UIDocument>();
        toggle = root.rootVisualElement.Q<Toggle>("ClickEnable_toggle");
        toggle.RegisterValueChangedCallback(ClickOperationToggled);

    }

    private void ClickOperationToggled(ChangeEvent<bool> evt)
    {
        Debug.Log($"Toggle value = {evt.newValue}");
        playerController.ClickOperationEnabled = evt.newValue;
    }



    void Update()
    {

    }
}
