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
    const string SupplyPressurePanelName = "SupplyPressure__panel";
    const string PressureZone2PanelName = "PressureZone2__panel";
    const string PressureZone3PanelName = "PressureZone3__panel";
    const string PressureZone2SliderName = "PressureZone2__slider";



    SliderInt PressureZone2Slider;

    VisualElement m_GameMenuScreen;
    VisualElement m_GameMenuOptionsScreen;
    VisualElement m_SupplyPressurePanel;
    VisualElement m_PressureZone2Panel;
    VisualElement m_PressureZone3Panel;

    public bool isUiClicked = false;
    // Start is called before the first frame update
    void Start()
    {
        //set visual elements
        var root = GetComponent<UIDocument>();
        m_SupplyPressurePanel = root.rootVisualElement.Q<VisualElement>(SupplyPressurePanelName);
        m_PressureZone2Panel = root.rootVisualElement.Q<VisualElement>(PressureZone2PanelName);
        m_PressureZone3Panel = root.rootVisualElement.Q<VisualElement>(PressureZone3PanelName);
        PressureZone2Slider = root.rootVisualElement.Q<SliderInt>(PressureZone2SliderName);
        m_GameMenuScreen = root.rootVisualElement.Q<VisualElement>(GameMenuScreenName);
        m_GameMenuOptionsScreen = root.rootVisualElement.Q<VisualElement>(GameMenuOptionsScreenName);


        //Register callbacks
        m_SupplyPressurePanel.RegisterCallback<MouseDownEvent>(MouseDown, TrickleDown.TrickleDown);
        m_PressureZone2Panel.RegisterCallback<MouseDownEvent>(MouseDown, TrickleDown.TrickleDown);
        m_PressureZone3Panel.RegisterCallback<MouseDownEvent>(MouseDown, TrickleDown.TrickleDown);
        PressureZone2Slider.RegisterCallback<MouseDownEvent>(MouseDown);
        m_GameMenuScreen.RegisterCallback<MouseDownEvent>(MouseDown, TrickleDown.TrickleDown);
        m_GameMenuOptionsScreen.RegisterCallback<MouseDownEvent>(MouseDown);


        // SupplyPressureTextField.RegisterCallback<MouseUpEvent>(MouseUp);
        // PressureZone2TextField.RegisterCallback<MouseUpEvent>(MouseUp);
        // PressureZone3TextField.RegisterCallback<MouseUpEvent>(MouseUp);
        m_SupplyPressurePanel.RegisterCallback<MouseUpEvent>(MouseUp, TrickleDown.TrickleDown);
        m_PressureZone2Panel.RegisterCallback<MouseUpEvent>(MouseUp, TrickleDown.TrickleDown);
        m_PressureZone3Panel.RegisterCallback<MouseUpEvent>(MouseUp, TrickleDown.TrickleDown);
        m_GameMenuScreen.RegisterCallback<MouseUpEvent>(MouseUp, TrickleDown.TrickleDown);
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
        playerController.isOperableObject = false;
        playerController.operableObject = null;
        // Vector2 pointerScreenPos = Pointer.current.position.ReadValue();
        // if (IsPointerOverUI(pointerScreenPos))
        // {

        //     playerController.isOperableObject = false;
        //     playerController.operableObject = null;

        // }


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
