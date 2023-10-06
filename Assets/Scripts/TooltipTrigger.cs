using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TooltipTrigger : MonoBehaviour
{
    private Button fillButton;
    private Button menuButton;
    private VisualElement buttonWrapper;
    public string content;
    public string header;
    public ToolTipScriptableObject fillButtonTooltip;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        var root = GetComponent<UIDocument>();

        fillButton = root.rootVisualElement.Q<Button>("FillButton");
        menuButton = root.rootVisualElement.Q<Button>("MenuButton");
        buttonWrapper = root.rootVisualElement.Q<VisualElement>("ButtonWrapper");


        fillButton.RegisterCallback<MouseEnterEvent>(MouseIn);
        fillButton.RegisterCallback<MouseOutEvent>(MouseOut);
    }

    private void Start()
    {
        Debug.Log(buttonWrapper.childCount);
    }


    private void MouseOut(MouseOutEvent evt)
    {
        // Debug.Log($"MouseOut");
        TooltipSystem.Hide();
    }

    private void MouseIn(MouseEnterEvent evt)
    {

        TooltipSystem.Show(fillButtonTooltip.content, fillButtonTooltip.header);
        // Debug.Log($"MouseIn");
    }
    void Update()
    {

    }

}