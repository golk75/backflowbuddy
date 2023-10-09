using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TooltipTrigger : MonoBehaviour
// , IPointerEnterHandler, IPointerExitHandler
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



        fillButton.RegisterCallback<MouseEnterEvent>(MouseIn);
        fillButton.RegisterCallback<MouseOutEvent>(MouseOut);
        menuButton.RegisterCallback<MouseEnterEvent>(MouseIn);
        menuButton.RegisterCallback<MouseOutEvent>(MouseOut);
        resetButton.RegisterCallback<MouseEnterEvent>(MouseIn);
        resetButton.RegisterCallback<MouseOutEvent>(MouseOut);


        // fillButton.RegisterCallback<MouseDownEvent>(MouseDown);

    }



    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
        Debug.Log($"IPOINT OUT");

    }

    public void OnPointerEnter(PointerEventData eventData)
    {


        TooltipSystem.Show(content, header);
        tooltip.transform.position = Input.mousePosition;
        Debug.Log($"IPOINT IN");
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