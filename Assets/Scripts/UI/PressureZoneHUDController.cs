
using System;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PressureZoneHUDController : MonoBehaviour
{
    public WaterController waterController;
    const string SupplyPressureTextString = "SupplyPressure__value";
    const string PressureZone2TextString = "PressureZone2__value";
    const string PressureZone3TextString = "PressureZone3__value";
    const string PressureZone2Slider = "PressureZone2__slider";


    TextField SupplyPressureTextField;
    TextField PressureZone2TextField;
    TextField PressureZone3TextField;
    bool zone2PressureFocused = false;
    float zone2PressureOnFocus;

    // Start is called before the first frame update
    void Start()
    {
        SetVisualElements();
        RegisterTextFieldCallBacks();
        SupplyPressureTextField.isDelayed = false;
        PressureZone2TextField.isDelayed = false;
        PressureZone3TextField.isDelayed = false;
    }


    void OnEnable()
    {

    }
    void OnDisable()
    {

    }


    void SetVisualElements()
    {
        var root = GetComponent<UIDocument>();
        SupplyPressureTextField = root.rootVisualElement.Q<TextField>(SupplyPressureTextString);
        PressureZone2TextField = root.rootVisualElement.Q<TextField>(PressureZone2TextString);
        PressureZone3TextField = root.rootVisualElement.Q<TextField>(PressureZone3TextString);

    }


    void RegisterTextFieldCallBacks()
    {
        SupplyPressureTextField.RegisterCallback<ChangeEvent<string>>(InputValueChanged);
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
        PressureZone2TextField.value = waterController.zone2Pressure.ToString();
        PressureZone3TextField.value = waterController.zone3Pressure.ToString();

    }
}
