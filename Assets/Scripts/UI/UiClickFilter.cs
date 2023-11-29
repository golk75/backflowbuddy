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
    //string ids
    const string GameMenuScreenName = "GameMenuScreen";
    const string GameMenuOptionsScreenName = "GameMenuOptionsScreen";
    const string SupplyPressureTextFieldName = "SupplyPressure__value";
    const string PressureZone2TextFieldName = "PressureZone2__value";
    const string PressureZone3TextFieldName = "PressureZone3__value";
    const string PressureZone2SliderName = "PressureZone2__slider";

    TextField SupplyPressureTextField;
    TextField PressureZone2TextField;
    TextField PressureZone3TextField;
    SliderInt PressureZone2Slider;
    VisualElement m_GameMenuScreen;
    VisualElement m_GameMenuOptionsScreen;

    public bool isUiClicked = false;
    // Start is called before the first frame update
    void Start()
    {
        //set visual elements
        var root = GetComponent<UIDocument>();
        SupplyPressureTextField = root.rootVisualElement.Q<TextField>(SupplyPressureTextFieldName);
        PressureZone2TextField = root.rootVisualElement.Q<TextField>(PressureZone2TextFieldName);
        PressureZone3TextField = root.rootVisualElement.Q<TextField>(PressureZone3TextFieldName);
        PressureZone2Slider = root.rootVisualElement.Q<SliderInt>(PressureZone2SliderName);
        m_GameMenuScreen = root.rootVisualElement.Q<VisualElement>(GameMenuScreenName);
        m_GameMenuOptionsScreen = root.rootVisualElement.Q<VisualElement>(GameMenuOptionsScreenName);


        //Register callbacks
        SupplyPressureTextField.RegisterCallback<MouseDownEvent>(MouseDown);
        PressureZone2TextField.RegisterCallback<MouseDownEvent>(MouseDown);
        PressureZone3TextField.RegisterCallback<MouseDownEvent>(MouseDown);
        PressureZone2Slider.RegisterCallback<MouseDownEvent>(MouseDown);
        m_GameMenuScreen.Q("GameMenuScreen_anchor").RegisterCallback<MouseDownEvent>(MouseDown);
        m_GameMenuOptionsScreen.RegisterCallback<MouseDownEvent>(MouseDown);


        SupplyPressureTextField.RegisterCallback<MouseUpEvent>(MouseUp);
        PressureZone2TextField.RegisterCallback<MouseUpEvent>(MouseUp);
        PressureZone3TextField.RegisterCallback<MouseUpEvent>(MouseUp);
        m_GameMenuScreen.Q("GameMenuScreen_anchor").RegisterCallback<MouseUpEvent>(MouseUp);
        m_GameMenuOptionsScreen.RegisterCallback<MouseUpEvent>(MouseUp);
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

        isUiClicked = true;
        Vector2 pointerScreenPos = Pointer.current.position.ReadValue();
        if (IsPointerOverUI(pointerScreenPos))
        {
            playerController.isOperableObject = false;
            playerController.operableObject = null;

        }


    }
    private void MouseUp(MouseUpEvent evt)
    {
        isUiClicked = false;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
