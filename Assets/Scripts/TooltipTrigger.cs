

using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UIElements;


public class TooltipTrigger : MonoBehaviour

{
    public Button fillButton;
    private Button menuButton;
    private Button resetButton;
    private VisualElement clickEnableToggle;
    public string content;
    public string header;
    public VisualElement tooltip;
    public ToolTipScriptableObject fillButtonTooltip;
    public ToolTipScriptableObject menuButtonTooltip;
    public ToolTipScriptableObject resetButtonTooltip;
    public ToolTipScriptableObject clickEnableTooltip;
    UIDocument root;
    [SerializeField]
    private Texture2D[] cursorTextureArray;
    UnityEngine.UIElements.Cursor cursor_default;
    UnityEngine.UIElements.Cursor cursor_grab;

    public VisualElement buttonWrapper;
    public VisualElement sceneContainer;



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
        clickEnableToggle = root.rootVisualElement.Q<VisualElement>("ClickEnable_toggle");

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
        clickEnableToggle.RegisterCallback<MouseEnterEvent>(MouseIn);
        clickEnableToggle.RegisterCallback<MouseOutEvent>(MouseOut);




    }



    private void Start()
    {
    }


    private void MouseOut(MouseOutEvent evt)
    {


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
        else if (evt.target == clickEnableToggle)
        {
            TooltipSystem.Show(clickEnableTooltip.content, clickEnableTooltip.header);
        }


    }

    void Update()
    {

    }



}