
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


    TextField SupplyPressureTextField;
    TextField PressureZone2TextField;
    TextField PressureZone3TextField;
    bool zone2PressureFocused = false;
    float zone2PressureOnFocus;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>();
        SupplyPressureTextField = root.rootVisualElement.Q<TextField>(SupplyPressureTextString);
        SupplyPressureTextField.isDelayed = false;

        PressureZone2TextField = root.rootVisualElement.Q<TextField>(PressureZone2TextString);
        PressureZone2TextField.isDelayed = false;
        //Register callbacks
        // SupplyPressureTextField.RegisterCallback<ChangeEvent<string>>((evt) =>
        // {

        //     int result;
        //     bool isInt = Int32.TryParse(evt.newValue, out result);
        //     // Debug.Log($"evt: {evt.newValue.GetType()}");
        //     waterController.supplyPsi = result;
        //     // InputValueChanged(evt);
        //     //display and update Zones 2 & 3 pressures (maintained in waterController)
        //     PressureZone2TextField.value = waterController.zone2Pressure.ToString();
        // });


        SupplyPressureTextField.RegisterCallback<ChangeEvent<string>>(InputValueChanged);


    }



    private void InputValueChanged(ChangeEvent<string> evt)
    {

        int result;
        bool isInt = Int32.TryParse(evt.newValue, out result);
        // Debug.Log($"evt: {evt.newValue.GetType()}");
        waterController.supplyPsi = result;


    }

    void OnEnable()
    {

    }
    void OnDisable()
    {

    }



    // Update is called once per frame
    void Update()
    {
        PressureZone2TextField.value = waterController.zone2Pressure.ToString();

    }
}
