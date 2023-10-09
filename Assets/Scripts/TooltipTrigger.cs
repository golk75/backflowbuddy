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
    private VisualElement buttonWrapper;
    public string content;
    public string header;
    public VisualElement tooltip;

    public ToolTipScriptableObject fillButtonTooltip;
    public ToolTipScriptableObject menuButtonTooltip;
    UIDocument root;
    // public List<ScriptableObject> tooltips;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    /// 
    private void Awake()
    {
        root = GetComponent<UIDocument>();

        fillButton = root.rootVisualElement.Q<Button>("FillButton");
        menuButton = root.rootVisualElement.Q<Button>("MenuButton");
        buttonWrapper = root.rootVisualElement.Q<VisualElement>("ButtonWrapper");
        tooltip = root.rootVisualElement.Q<VisualElement>("Tooltip");



        fillButton.RegisterCallback<MouseEnterEvent>(MouseIn);
        fillButton.RegisterCallback<MouseOutEvent>(MouseOut);
        menuButton.RegisterCallback<MouseEnterEvent>(MouseIn);
        menuButton.RegisterCallback<MouseOutEvent>(MouseOut);
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

        // TooltipSystem.Show(content, header);
        Debug.Log($"MouseEvent In");
        // Debug.Log($"{evt.target == fillButton}");
    }

    void Update()
    {

    }


}