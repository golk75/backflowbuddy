
using System;
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
    const string PressureZone2SliderString = "PressureZone2__slider";


    //visual elements
    TextField m_SupplyPressureTextField;
    TextField m_PressureZone2TextField;
    TextField m_PressureZone3TextField;
    Slider m_PressureZone2Slider;


    //booleans


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
        var root = GetComponent<UIDocument>();
        m_SupplyPressureTextField = root.rootVisualElement.Q<TextField>(SupplyPressureTextString);
        m_PressureZone2TextField = root.rootVisualElement.Q<TextField>(PressureZone2TextString);
        m_PressureZone3TextField = root.rootVisualElement.Q<TextField>(PressureZone3TextString);
        m_PressureZone2Slider = root.rootVisualElement.Q<Slider>(PressureZone2SliderString);

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
