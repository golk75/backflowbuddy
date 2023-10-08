using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // private Button fillButton;
    // private Button menuButton;
    // private VisualElement buttonWrapper;
    public string content;
    public string header;

    public ToolTipScriptableObject fillButtonTooltip;
    // UIDocument root;
    public List<ScriptableObject> tooltips;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        // root = GetComponent<UIDocument>();

        // fillButton = root.rootVisualElement.Q<Button>("FillButton");
        // menuButton = root.rootVisualElement.Q<Button>("MenuButton");
        // buttonWrapper = root.rootVisualElement.Q<VisualElement>("ButtonWrapper");
        // fillButton.AddManipulator(new UIManipulatorTest());



        // fillButton.RegisterCallback<MouseEnterEvent>(MouseIn);
        // fillButton.RegisterCallback<MouseOutEvent>(MouseOut);
    }

    private void Start()
    {
        // buttonWrapper.Query(className: "button")
        //               .ForEach((element) =>
        //                {
        //                    Debug.Log($"{element.name}");
        //                    element.RegisterCallback<MouseEnterEvent>(MouseIn);
        //                    element.RegisterCallback<MouseOutEvent>(MouseOut);
        //                });

    }


    // private void MouseOut(MouseOutEvent evt)
    // {
    //     // Debug.Log($"MouseOut");
    //     TooltipSystem.Hide();
    // }

    // private void MouseIn(MouseEnterEvent evt)
    // {
    //     buttonWrapper.Query(className: "button")
    //                  .ForEach((element) =>
    //                   {
    //                       //   Debug.Log($"{element.GetType().sta}");

    //                   });

    //     TooltipSystem.Show(fillButtonTooltip.content, fillButtonTooltip.header);
    //     // Debug.Log($"MouseIn");
    // }

    void Update()
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Show(content, header);
        Debug.Log($"enter");
    }
}