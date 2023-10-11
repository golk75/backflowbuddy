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
    public string content;
    public string header;
    public VisualElement tooltip;
    public ToolTipScriptableObject fillButtonTooltip;
    public ToolTipScriptableObject menuButtonTooltip;
    public ToolTipScriptableObject resetButtonTooltip;
    UIDocument root;
    OperableComponentDescription ShutOff1OperableDescription;
    [SerializeField]
    private Texture2D[] cursorTextureArray;
    UnityEngine.UIElements.Cursor cursor_default;
    UnityEngine.UIElements.Cursor cursor_grab;

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
        tooltip = root.rootVisualElement.Q<VisualElement>("Tooltip");

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


    }


    private void MouseOut(MouseOutEvent evt)
    {
        Debug.Log($"MouseEvent Out");

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