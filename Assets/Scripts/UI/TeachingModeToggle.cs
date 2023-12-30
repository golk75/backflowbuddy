using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UIElements;

public class TeachingModeToggle : MonoBehaviour
{
    UIDocument root;

    public Toggle toggle;

    public WaterController waterController;
    const string PressureZone2PanelTemplateString = "PressureZone2__panel";
    const string PressureZone3PanelTemplateString = "PressureZone3__panel";
    const string SupplyPressurePanelTemplateString = "SupplyPressure__panelTemp";


    VisualElement m_SupplyPressurePanel;
    VisualElement m_PressureZone2Panel;
    VisualElement m_PressureZone3Panel;

    void Start()
    {
        root = GetComponent<UIDocument>();
        toggle = root.rootVisualElement.Q<Toggle>("TeachingMode_toggle");
        toggle.RegisterValueChangedCallback(TeachingModeToggled);
        m_SupplyPressurePanel = root.rootVisualElement.Q<VisualElement>(SupplyPressurePanelTemplateString);
        m_PressureZone2Panel = root.rootVisualElement.Q<VisualElement>(PressureZone2PanelTemplateString);
        m_PressureZone3Panel = root.rootVisualElement.Q<VisualElement>(PressureZone3PanelTemplateString);

    }

    private void TeachingModeToggled(ChangeEvent<bool> evt)
    {
        // if (waterController.isTeachingModeEnabled == false)
        // {
        //     waterController.isTeachingModeEnabled = true;
        // }
        // else
        // {
        //     waterController.isTeachingModeEnabled = false;
        // }
        if (evt.newValue == true)
        {
            m_SupplyPressurePanel.style.visibility = Visibility.Visible;
            m_PressureZone2Panel.style.visibility = Visibility.Visible;
            m_PressureZone3Panel.style.visibility = Visibility.Visible;
        }
        else
        {
            m_SupplyPressurePanel.style.visibility = Visibility.Hidden;
            m_PressureZone2Panel.style.visibility = Visibility.Hidden;
            m_PressureZone3Panel.style.visibility = Visibility.Hidden;
        }

    }



    void Update()
    {

    }
}
