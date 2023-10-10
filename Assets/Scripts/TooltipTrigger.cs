using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.AI.Navigation.Editor.Converter;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class TooltipTrigger : MonoBehaviour

{
    private Button fillButton;
    private Button menuButton;
    private Button resetButton;
    private VisualElement buttonWrapper;
    public string content;
    public string header;
    public VisualElement tooltip;
    public PlayerController playerController;
    public GameObject SO1;

    public ToolTipScriptableObject fillButtonTooltip;
    public ToolTipScriptableObject menuButtonTooltip;
    public ToolTipScriptableObject resetButtonTooltip;
    UIDocument root;
    // public List<ScriptableObject> tooltips;
    OperableComponentDescription ShutOff1OperableDescription;
    [SerializeField]
    private Texture2D[] cursorTextureArray;
    UnityEngine.UIElements.Cursor cursor_default;
    UnityEngine.UIElements.Cursor cursor_grab;
    VisualElement sceneContainer;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    /// 
    private void Awake()
    {
        root = GetComponent<UIDocument>();

        fillButton = root.rootVisualElement.Q<Button>("FillButton");
        menuButton = root.rootVisualElement.Q<Button>("MenuButton");
        resetButton = root.rootVisualElement.Q<Button>("ResetButton");
        buttonWrapper = root.rootVisualElement.Q<VisualElement>("ButtonWrapper");
        tooltip = root.rootVisualElement.Q<VisualElement>("Tooltip");
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


        fillButton.RegisterCallback<MouseEnterEvent>(MouseIn);
        fillButton.RegisterCallback<MouseOutEvent>(MouseOut);
        menuButton.RegisterCallback<MouseEnterEvent>(MouseIn);
        menuButton.RegisterCallback<MouseOutEvent>(MouseOut);
        resetButton.RegisterCallback<MouseEnterEvent>(MouseIn);
        resetButton.RegisterCallback<MouseOutEvent>(MouseOut);

        // resetButton.clicked += MouseDown;
        resetButton.RegisterCallback<MouseDownEvent>(MouseDown);
        sceneContainer.RegisterCallback<MouseDownEvent>(MouseDown);
        sceneContainer.RegisterCallback<MouseUpEvent>(MouseUp);


    }

    private void MouseUp(MouseUpEvent evt)
    {
        sceneContainer.style.cursor = new StyleCursor(cursor_default);
    }

    private void MouseDown(MouseDownEvent evt)
    {

        sceneContainer.style.cursor = new StyleCursor(cursor_grab);


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();


    }

    public void OnPointerEnter(PointerEventData eventData)
    {


        TooltipSystem.Show(content, header);
        tooltip.transform.position = Input.mousePosition;

    }
    private void Start()
    {
        ShutOff1OperableDescription = SO1.GetComponent<OperableComponentDescription>();

    }


    private void MouseOut(MouseOutEvent evt)
    {
        Debug.Log($"MouseEvent Out");
        playerController.operableObject = null;
        TooltipSystem.Hide();
    }

    private void MouseIn(MouseEnterEvent evt)
    {
        if (evt.target == fillButton)
        {
            TooltipSystem.Show(fillButtonTooltip.content, fillButtonTooltip.header);

        }
        else if (evt.target == menuButton)
        {
            TooltipSystem.Show(menuButtonTooltip.content, menuButtonTooltip.header);
        }
        else if (evt.target == resetButton)
        {
            TooltipSystem.Show(resetButtonTooltip.content, resetButtonTooltip.header);
        }

        // TooltipSystem.Show(content, header);
        Debug.Log($"{evt.target == resetButton}");
    }

    void Update()
    {

    }


}