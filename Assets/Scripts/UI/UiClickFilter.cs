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
    List<Button> FloatingButtons;

    public List<VisualElement> SliderDraggers { get; private set; }


    public Texture2D handGrab;
    public Texture2D handOpen;
    public Texture2D handPoint;
    public bool isUiClicked = false;
    public bool isUiHovered = false;
    // Start is called before the first frame update
    void Awake()
    {
        //set visual elements
        var root = GetComponent<UIDocument>();
        FloatingElements = root.rootVisualElement.Query(className: "floating").ToList();
        FloatingButtons = root.rootVisualElement.Query<Button>(className: "unity-button").ToList();
        SliderDraggers = root.rootVisualElement.Query<VisualElement>("unity-drag-container").ToList();
        foreach (var button in FloatingButtons)
        {


            button.RegisterCallback<PointerDownEvent>(
                e =>
                {
                    isUiClicked = true;
                    // UnityEngine.Cursor.SetCursor(handPoint, new Vector2(0, 0), CursorMode.Auto);
                },
                TrickleDown.TrickleDown);
            button.RegisterCallback<PointerUpEvent>(
                e =>
                {
                    isUiClicked = false;

                },
                TrickleDown.TrickleDown);

            button.RegisterCallback<PointerEnterEvent>(
                e =>
                {
                    // isUiClicked = true;
                    UnityEngine.Cursor.SetCursor(handPoint, new Vector2(handPoint.width / 2, 0), CursorMode.Auto);

                });
            button.RegisterCallback<PointerLeaveEvent>(
                e =>
                {
                    // isUiClicked = false;
                    UnityEngine.Cursor.SetCursor(handOpen, new Vector2(handOpen.width / 2, handOpen.height / 2), CursorMode.Auto);


                });

        }
        foreach (var ele in FloatingElements)
        {

            ele.RegisterCallback<PointerUpEvent>(
                e =>
                {
                    isUiClicked = false;
                    UnityEngine.Cursor.SetCursor(handOpen, new Vector2(handOpen.width / 2, handOpen.height / 2), CursorMode.Auto);
                }, TrickleDown.TrickleDown);
            ele.RegisterCallback<PointerDownEvent>(
            e =>
            {
                UnityEngine.Cursor.SetCursor(handGrab, new Vector2(handGrab.width / 2, handGrab.height / 2), CursorMode.Auto);
                isUiClicked = true;
                playerController.isOperableObject = false;
                playerController.operableObject = null;



            }, TrickleDown.TrickleDown);
            // ele.RegisterCallback<PointerDownEvent>(PointerDown);

            // ele.RegisterCallback<PointerUpEvent>(PointerUp);
        }

        foreach (var dragger in SliderDraggers)
        {
            dragger.RegisterCallback<PointerUpEvent>(
               e =>
               {
                   isUiClicked = false;
                   UnityEngine.Cursor.SetCursor(handOpen, new Vector2(handOpen.width / 2, handOpen.height / 2), CursorMode.Auto);
               });
            dragger.RegisterCallback<PointerDownEvent>(
            e =>
            {
                isUiClicked = true;
                playerController.isOperableObject = false;
                playerController.operableObject = null;
                UnityEngine.Cursor.SetCursor(handGrab, new Vector2(handGrab.width / 2, handGrab.height / 2), CursorMode.Auto);


            }, TrickleDown.TrickleDown);
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
