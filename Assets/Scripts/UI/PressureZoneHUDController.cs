
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PressureZoneHUDController : MonoBehaviour
{
    //game objects
    public WaterController waterController;

    //string ids
    const string SupplyPressureTextString = "SupplyPressure__value";
    const string PressureZone2TextString = "PressureZone2__value";
    const string PressureZone3TextString = "PressureZone3__value";
    const string PressureZoneSliderBarString = "PressureZoneSlider";
    const string PressureZoneSliderTrackerString = "unity-tracker";
    const string PressureZoneSliderHandleString = "unity-dragger";


    //visual elements
    TextField m_SupplyPressureTextField;
    TextField m_PressureZone2TextField;
    TextField m_PressureZone3TextField;

    VisualElement m_PressureZoneSliderBar;
    VisualElement m_PressureZoneSliderTracker;
    VisualElement m_PressureZoneSliderHandle;
    VisualElement m_SliderFillBar;


    //booleans


    //root
    UIDocument root;


    //lists

    List<VisualElement> SliderHandleList;

    // Start is called before the first frame update
    void Start()
    {
        SetVisualElements();
        RegisterTextFieldCallBacks();
        m_SupplyPressureTextField.isDelayed = false;
        m_PressureZone2TextField.isDelayed = false;
        m_PressureZone3TextField.isDelayed = false;
    }

    void SetVisualElements()
    {
        root = GetComponent<UIDocument>();
        m_SupplyPressureTextField = root.rootVisualElement.Q<TextField>(SupplyPressureTextString);
        m_PressureZone2TextField = root.rootVisualElement.Q<TextField>(PressureZone2TextString);
        m_PressureZone3TextField = root.rootVisualElement.Q<TextField>(PressureZone3TextString);
        m_PressureZoneSliderBar = root.rootVisualElement.Query<VisualElement>(PressureZoneSliderBarString);
        m_PressureZoneSliderTracker = root.rootVisualElement.Q<VisualElement>(PressureZoneSliderTrackerString);
        SliderHandleList = root.rootVisualElement.Query(name: "unity-dragger").ToList();


        foreach (var dragger in SliderHandleList)
        {
            AddElements(dragger);
        }


    }
    void AddElements(VisualElement sliderHandle)
    {
        m_SliderFillBar = new VisualElement();
        sliderHandle.Add(m_SliderFillBar);
        m_SliderFillBar.name = "SliderFillBar";
        m_SliderFillBar.AddToClassList("fill-bar");



    }


    void RegisterTextFieldCallBacks()
    {
        m_SupplyPressureTextField.RegisterCallback<ChangeEvent<string>>(InputValueChanged);
    }


    private void InputValueChanged(ChangeEvent<string> evt)
    {
        int result;
        bool isInt = Int32.TryParse(evt.newValue, out result);
        waterController.supplyPsi = result;
    }


    // Update is called once per frame
    void Update()
    {
        m_PressureZone2TextField.value = waterController.zone2Pressure.ToString();
        m_PressureZone3TextField.value = waterController.zone3Pressure.ToString();

    }
}
