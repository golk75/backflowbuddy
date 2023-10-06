using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TooltipTrigger : MonoBehaviour
{
    private Button fillButton;
    private Button menuButton;
    public string content;
    public string header;
    public ToolTipScriptableObject fillButtonTooltip;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement.Q<Button>("FillButton");
        fillButton = root.Q<Button>("FillButton");
        menuButton = root.Q<Button>("MenuButton");

        fillButton.RegisterCallback<MouseEnterEvent>(MouseIn);
        fillButton.RegisterCallback<MouseOutEvent>(MouseOut);
    }

    private void Start()
    {

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


}