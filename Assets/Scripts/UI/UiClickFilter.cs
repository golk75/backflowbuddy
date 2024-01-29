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

    //visual elements
    VisualElement m_GameMenuScreen;
    VisualElement m_GameMenuOptionsScreen;
    VisualElement m_SupplyPressurePanel;
    VisualElement m_PressureZone2Panel;
    VisualElement m_PressureZone3Panel;
    VisualElement m_Slider_PressureZone2;
    VisualElement m_Slider_PressureZone3;

    //lists
    List<VisualElement> SliderList;

    public bool isUiClicked = false;
    public bool isUiHovered = false;
    // Start is called before the first frame update
    void Start()
    {
        //set visual elements
        var root = GetComponent<UIDocument>();
        SliderList = root.rootVisualElement.Query(className: "unity-base-slider__dragger").ToList();
        m_SupplyPressurePanel = root.rootVisualElement.Q<VisualElement>(SupplyPressurePanelName);
        m_PressureZone2Panel = root.rootVisualElement.Q<VisualElement>(PressureZone2PanelName);
        m_PressureZone3Panel = root.rootVisualElement.Q<VisualElement>(PressureZone3PanelName);
        m_GameMenuScreen = root.rootVisualElement.Q<VisualElement>(GameMenuScreenName);
        m_GameMenuOptionsScreen = root.rootVisualElement.Q<VisualElement>(GameMenuOptionsScreenName);



        //Register callbacks
        //MouseDown
        m_SupplyPressurePanel.RegisterCallback<PointerDownEvent>(PointerDown, TrickleDown.TrickleDown);
        m_PressureZone2Panel.RegisterCallback<PointerDownEvent>(PointerDown, TrickleDown.TrickleDown);
        m_PressureZone3Panel.RegisterCallback<PointerDownEvent>(PointerDown, TrickleDown.TrickleDown);
        m_GameMenuScreen.RegisterCallback<PointerDownEvent>(PointerDown, TrickleDown.TrickleDown);
        m_GameMenuOptionsScreen.RegisterCallback<PointerDownEvent>(PointerDown, TrickleDown.TrickleDown);



        //MouseUp
        m_SupplyPressurePanel.RegisterCallback<PointerUpEvent>(PointerUp, TrickleDown.TrickleDown);
        m_PressureZone2Panel.RegisterCallback<PointerUpEvent>(PointerUp, TrickleDown.TrickleDown);
        m_PressureZone3Panel.RegisterCallback<PointerUpEvent>(PointerUp, TrickleDown.TrickleDown);
        m_GameMenuScreen.RegisterCallback<PointerUpEvent>(PointerUp, TrickleDown.TrickleDown);
        m_GameMenuOptionsScreen.RegisterCallback<PointerUpEvent>(PointerUp, TrickleDown.TrickleDown);


        foreach (var slider in SliderList)
        {
            Debug.Log($"slider: {slider.name}");
            slider.RegisterCallback<PointerDownEvent>(PointerDown, TrickleDown.TrickleDown);
            slider.RegisterCallback<PointerUpEvent>(PointerUp, TrickleDown.TrickleDown);
        }

    }

    private void PointerUp(PointerUpEvent evt)
    {
        isUiClicked = false;
    }

    private void PointerDown(PointerDownEvent evt)
    {
        isUiClicked = true;
        playerController.isOperableObject = false;
        playerController.operableObject = null;
    }

}
