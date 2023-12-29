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
    VisualElement m_Slider;


    public bool isUiClicked = false;
    public bool isUiHovered = false;
    // Start is called before the first frame update
    void Start()
    {
        //set visual elements
        var root = GetComponent<UIDocument>();

        m_SupplyPressurePanel = root.rootVisualElement.Q<VisualElement>(SupplyPressurePanelName);
        m_PressureZone2Panel = root.rootVisualElement.Q<VisualElement>(PressureZone2PanelName);
        m_PressureZone3Panel = root.rootVisualElement.Q<VisualElement>(PressureZone3PanelName);
        m_GameMenuScreen = root.rootVisualElement.Q<VisualElement>(GameMenuScreenName);
        m_GameMenuOptionsScreen = root.rootVisualElement.Q<VisualElement>(GameMenuOptionsScreenName);


        //Register callbacks
        //MouseDown
        m_SupplyPressurePanel.RegisterCallback<MouseDownEvent>(MouseDown, TrickleDown.TrickleDown);
        m_PressureZone2Panel.RegisterCallback<MouseDownEvent>(MouseDown, TrickleDown.TrickleDown);
        m_PressureZone3Panel.RegisterCallback<MouseDownEvent>(MouseDown, TrickleDown.TrickleDown);
        m_GameMenuScreen.RegisterCallback<MouseDownEvent>(MouseDown, TrickleDown.TrickleDown);
        m_GameMenuOptionsScreen.RegisterCallback<MouseDownEvent>(MouseDown, TrickleDown.TrickleDown);



        //MouseUp
        m_SupplyPressurePanel.RegisterCallback<MouseUpEvent>(MouseUp, TrickleDown.TrickleDown);
        m_PressureZone2Panel.RegisterCallback<MouseUpEvent>(MouseUp, TrickleDown.TrickleDown);
        m_PressureZone3Panel.RegisterCallback<MouseUpEvent>(MouseUp, TrickleDown.TrickleDown);
        m_GameMenuScreen.RegisterCallback<MouseUpEvent>(MouseUp, TrickleDown.TrickleDown);
        m_GameMenuOptionsScreen.RegisterCallback<MouseUpEvent>(MouseUp, TrickleDown.TrickleDown);



        // //MouseOut
        m_SupplyPressurePanel.RegisterCallback<MouseOutEvent>(MouseOut, TrickleDown.TrickleDown);
        m_PressureZone2Panel.RegisterCallback<MouseOutEvent>(MouseOut, TrickleDown.TrickleDown);
        m_PressureZone3Panel.RegisterCallback<MouseOutEvent>(MouseOut, TrickleDown.TrickleDown);


        //MouseOver
        m_SupplyPressurePanel.RegisterCallback<MouseOverEvent>(MouseOver, TrickleDown.TrickleDown);
        m_PressureZone2Panel.RegisterCallback<MouseOverEvent>(MouseOver, TrickleDown.TrickleDown);
        m_PressureZone3Panel.RegisterCallback<MouseOverEvent>(MouseOver, TrickleDown.TrickleDown);

    }
    void OnEnable()
    {

    }
    private void MouseOver(MouseOverEvent evt)
    {

        isUiHovered = true;

    }
    private void MouseEnter(MouseEnterEvent evt)
    {

        isUiHovered = true;
    }
    private void MouseOut(MouseOutEvent evt)
    {

        isUiHovered = false;

    }
    private void MouseDown(MouseDownEvent evt)
    {

        isUiClicked = true;
        playerController.isOperableObject = false;
        playerController.operableObject = null;


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
