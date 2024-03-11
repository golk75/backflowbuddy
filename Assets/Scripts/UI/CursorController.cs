using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.WSA;


public class CursorController : MonoBehaviour
{

    private const string SupplyPressureZonePanelString = "PressureZone__supply_panel";
    private const string PressureZone2PanelString = "PressureZone2__panel";
    private const string PressureZone3PanelString = "PressureZone3__panel";

    private VisualElement buttonWrapper;
    private VisualElement SupplyPressureZonePanel;
    private VisualElement PressureZone2Panel;
    private VisualElement PressureZone3Panel;

    public List<VisualElement> SliderDraggers { get; private set; }

    private VisualElement sliderDragger;

    public Texture2D handGrab;
    public Texture2D handOpen;
    public Texture2D handPoint;
    VisualElement sceneContainer;
    UIDocument root;
    UnityEngine.UIElements.Cursor cursor_default;
    UnityEngine.UIElements.Cursor cursor_grab;


    private void Awake()
    {
        root = GetComponent<UIDocument>();
        SupplyPressureZonePanel = root.rootVisualElement.Q<VisualElement>(SupplyPressureZonePanelString);
        PressureZone2Panel = root.rootVisualElement.Q<VisualElement>(PressureZone2PanelString);
        PressureZone3Panel = root.rootVisualElement.Q<VisualElement>(PressureZone3PanelString);
        SliderDraggers = root.rootVisualElement.Query("unity-dragger").ToList();
        buttonWrapper = root.rootVisualElement.Q<VisualElement>("ButtonWrapper");
        sceneContainer = root.rootVisualElement.Q<VisualElement>("SceneContainer");

        cursor_grab = new()
        {
            texture = Resources.Load<Texture2D>("UI/Textures/icons8-hand-34"),
            hotspot = new Vector2(15, 12)
        };
        cursor_default = new()
        {
            texture = Resources.Load<Texture2D>("UI/Textures/icons8-hand-32"),
            hotspot = new Vector2(15, 12)
        };

        sceneContainer.RegisterCallback<MouseDownEvent>(MouseDown);
        sceneContainer.RegisterCallback<MouseUpEvent>(MouseUp);
        buttonWrapper.RegisterCallback<MouseDownEvent>(MouseDown);
        buttonWrapper.RegisterCallback<MouseUpEvent>(MouseUp);
        // SupplyPressureZonePanel.RegisterCallback<PointerDownEvent>(SupplyPanelPointerDown);
        // SupplyPressureZonePanel.RegisterCallback<PointerUpEvent>(SupplyPanelPointerUp);
        SupplyPressureZonePanel.RegisterCallback<MouseDownEvent>(MouseDown);
        SupplyPressureZonePanel.RegisterCallback<MouseUpEvent>(MouseUp);
        PressureZone2Panel.RegisterCallback<MouseDownEvent>(MouseDown);
        PressureZone2Panel.RegisterCallback<MouseUpEvent>(MouseUp);
        PressureZone3Panel.RegisterCallback<MouseDownEvent>(MouseDown);
        PressureZone3Panel.RegisterCallback<MouseUpEvent>(MouseUp);
        // foreach (var dragger in SliderDraggers)
        // {
        //     dragger.RegisterCallback<MouseDownEvent>(MouseDown);
        //     dragger.RegisterCallback<MouseUpEvent>(MouseUp);
        // }
        // SupplyPressureZonePanel.RegisterCallback<PointerEnterEvent>(PointerEnter, TrickleDown.TrickleDown);
        // SupplyPressureZonePanel.RegisterCallback<PointerOutEvent>(PointerOut);
        // PressureZone2Panel.RegisterCallback<PointerEnterEvent>(PointerEnter, TrickleDown.TrickleDown);
        // PressureZone2Panel.RegisterCallback<PointerOutEvent>(PointerOut);
        // PressureZone3Panel.RegisterCallback<PointerEnterEvent>(PointerEnter, TrickleDown.TrickleDown);
        // PressureZone3Panel.RegisterCallback<PointerOutEvent>(PointerOut);

        // SupplyPressureZonePanel.RegisterCallback<PointerDownEvent>(PointerDown, TrickleDown.TrickleDown);
        // SupplyPressureZonePanel.RegisterCallback<PointerUpEvent>(PointerUp);
        // PressureZone2Panel.RegisterCallback<PointerDownEvent>(PointerDown, TrickleDown.TrickleDown);
        // PressureZone2Panel.RegisterCallback<PointerUpEvent>(PointerUp);
        // PressureZone3Panel.RegisterCallback<PointerDownEvent>(PointerDown, TrickleDown.TrickleDown);
        // PressureZone3Panel.RegisterCallback<PointerUpEvent>(PointerUp);



    }

    private void SupplyPanelPointerDown(PointerDownEvent evt)
    {
        // SupplyPressureZonePanel.style.cursor = new StyleCursor(cursor_grab);
        UnityEngine.Cursor.SetCursor(handGrab, new Vector2(0, 0), CursorMode.Auto);

    }

    private void SupplyPanelPointerUp(PointerUpEvent evt)
    {
        // SupplyPressureZonePanel.style.cursor = new StyleCursor(cursor_grab);
        UnityEngine.Cursor.SetCursor(handOpen, new Vector2(0, 0), CursorMode.Auto);

    }

    private void PointerUp(PointerUpEvent evt)
    {
        // SupplyPressureZonePanel.style.cursor = new StyleCursor(cursor_default);
        // PressureZone2Panel.style.cursor = new StyleCursor(cursor_default);
        // PressureZone3Panel.style.cursor = new StyleCursor(cursor_default);
    }

    private void PointerDown(PointerDownEvent evt)
    {
        // SupplyPressureZonePanel.style.cursor = new StyleCursor(cursor_grab);
        // PressureZone2Panel.style.cursor = new StyleCursor(cursor_grab);
        // PressureZone3Panel.style.cursor = new StyleCursor(cursor_grab);
    }

    private void PointerOut(PointerOutEvent evt)
    {
        // SupplyPressureZonePanel.style.cursor = new StyleCursor(cursor_default);
        // PressureZone2Panel.style.cursor = new StyleCursor(cursor_default);
        // PressureZone3Panel.style.cursor = new StyleCursor(cursor_default);
    }

    private void PointerEnter(PointerEnterEvent evt)
    {
        // SupplyPressureZonePanel.style.cursor = new StyleCursor(cursor_default);
        // PressureZone2Panel.style.cursor = new StyleCursor(cursor_default);
        // PressureZone3Panel.style.cursor = new StyleCursor(cursor_default);
    }

    private void MouseUp(MouseUpEvent evt)
    {
        sceneContainer.style.cursor = new StyleCursor(cursor_default);
        buttonWrapper.style.cursor = new StyleCursor(cursor_default);
        UnityEngine.Cursor.SetCursor(handOpen, new Vector2(1, 1), CursorMode.Auto);

        Debug.Log($"evt.currentTarget: {evt.target}");
        // SupplyPressureZonePanel.style.cursor = new StyleCursor(cursor_default);
        // PressureZone2Panel.style.cursor = new StyleCursor(cursor_default);
        // PressureZone3Panel.style.cursor = new StyleCursor(cursor_default);

    }

    private void MouseDown(MouseDownEvent evt)
    {

        sceneContainer.style.cursor = new StyleCursor(cursor_grab);
        buttonWrapper.style.cursor = new StyleCursor(cursor_grab);
        UnityEngine.Cursor.SetCursor(handGrab, new Vector2(1, 1), CursorMode.Auto);
        // SupplyPressureZonePanel.style.cursor = new StyleCursor(cursor_grab);
        // PressureZone2Panel.style.cursor = new StyleCursor(cursor_grab);
        // PressureZone3Panel.style.cursor = new StyleCursor(cursor_grab);

    }
    private void Start()
    {


    }
    void Update()
    {

    }

}