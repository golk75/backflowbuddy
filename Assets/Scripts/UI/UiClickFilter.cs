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
    const string QuickTourScreenName = "QuickTourContainer";
    const string DCTestContainerName = "DCTestContainer";
    const string PopUpContainerName = "PopUpContainer";
    const string GameMenuScreenName = "GameMenuScreen_anchor";
    const string GameMenuOptionsScreenName = "GameMenuOptionsScreen";
    const string SupplyPressurePanelName = "SupplyPressure__panel";
    const string PressureZone2PanelName = "PressureZone2__panel";
    const string PressureZone3PanelName = "PressureZone3__panel";

    //visual elements
    VisualElement m_QuickTourScreen;
    VisualElement m_PopUpContainer;
    VisualElement m_GameMenuScreen;
    VisualElement m_DCTestContainer;
    VisualElement m_GameMenuOptionsScreen;
    VisualElement m_SupplyPressurePanel;
    VisualElement m_PressureZone2Panel;
    VisualElement m_PressureZone3Panel;
    VisualElement m_Slider_PressureZone2;
    VisualElement m_Slider_PressureZone3;

    //lists
    List<VisualElement> FloatingElements;

    public bool isUiClicked = false;
    public bool isUiHovered = false;
    // Start is called before the first frame update
    void Start()
    {
        //set visual elements
        var root = GetComponent<UIDocument>();
        FloatingElements = root.rootVisualElement.Query(className: "floating").ToList();

        foreach (var ele in FloatingElements)
        {


            ele.RegisterCallback<PointerDownEvent>(PointerDown, TrickleDown.TrickleDown);

            ele.RegisterCallback<PointerUpEvent>(PointerUp, TrickleDown.TrickleDown);



        }


    }

    private void PointerUp(PointerUpEvent evt)
    {
        isUiClicked = false;
    }

    private void PointerDown(PointerDownEvent evt)
    {
        Debug.Log($"{evt.target}");
        isUiClicked = true;
        playerController.isOperableObject = false;
        playerController.operableObject = null;
    }

}
