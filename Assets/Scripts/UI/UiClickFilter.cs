using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UiClickFilter : MonoBehaviour
{

    public UIDocument _uiDocument;
    public PlayerController playerController;

    const string SupplyPressureTextFieldString = "SupplyPressure__value";
    const string PressureZone2TextFieldString = "PressureZone2__value";
    const string PressureZone3TextFieldString = "PressureZone3__value";
    const string PressureZone2SliderString = "PressureZone2__slider";
    TextField SupplyPressureTextField;
    TextField PressureZone2TextField;
    TextField PressureZone3TextField;
    SliderInt PressureZone2Slider;
    public bool isUiClicked = false;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>();
        SupplyPressureTextField = root.rootVisualElement.Q<TextField>(SupplyPressureTextFieldString);
        PressureZone2TextField = root.rootVisualElement.Q<TextField>(PressureZone2TextFieldString);
        PressureZone3TextField = root.rootVisualElement.Q<TextField>(PressureZone3TextFieldString);
        PressureZone2Slider = root.rootVisualElement.Q<SliderInt>(PressureZone2SliderString);

        //Register callbacks

        SupplyPressureTextField.RegisterCallback<MouseDownEvent>(MouseDown);
        PressureZone2TextField.RegisterCallback<MouseDownEvent>(MouseDown);
        PressureZone3TextField.RegisterCallback<MouseDownEvent>(MouseDown);
        PressureZone2Slider.RegisterCallback<MouseDownEvent>(MouseDown);


        SupplyPressureTextField.RegisterCallback<MouseUpEvent>(MouseUp);
        PressureZone2TextField.RegisterCallback<MouseUpEvent>(MouseUp);
        PressureZone3TextField.RegisterCallback<MouseUpEvent>(MouseUp);
        PressureZone2Slider.Q("unity-drag-container").RegisterCallback<MouseUpEvent>(MouseUp);
    }



    public bool IsPointerOverUI(Vector2 screenPos)
    {

        Vector2 pointerUiPos = new Vector2 { x = screenPos.x, y = Screen.height - screenPos.y };
        List<VisualElement> picked = new List<VisualElement>();
        _uiDocument.rootVisualElement.panel.PickAll(pointerUiPos, picked);

        if (picked.Count <= 0)
        {
            return false;

        }
        else
        {
            return true;
        }
    }


    private void MouseDown(MouseDownEvent evt)
    {
        Debug.Log($"mouse down");
        isUiClicked = true;
        Vector2 pointerScreenPos = Pointer.current.position.ReadValue();
        if (IsPointerOverUI(pointerScreenPos))
        {
            playerController.isOperableObject = false;
            playerController.operableObject = null;

        }



        // Vector2 pointerScreenPos = Pointer.current.position.ReadValue();
        // if (!uiClickFilter.IsPointerOverUI(pointerScreenPos))
        // {
        //     Debug.Log($"Ui Element clicked");
        // }
        // else
        // {
        //     Debug.Log($"Ui Element not clicked");
        // }

    }
    private void MouseUp(MouseUpEvent evt)
    {
        Debug.Log($"mouse up");
        isUiClicked = false;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
