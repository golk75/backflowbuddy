
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
    const string PressureZone2TextString = "SupplyPressure__value";
    const string PressureZone3TextString = "SupplyPressure__value";


    TextField SupplyPressureTextField;
    TextField PressureZone2Text;
    TextField PressureZone3Text;


    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>();
        SupplyPressureTextField = root.rootVisualElement.Q<TextField>(SupplyPressureTextString);


        //Register callbacks
        SupplyPressureTextField.RegisterValueChangedCallback(evt =>
        {
            InputValueChanged(evt);
        });

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

    }
}
