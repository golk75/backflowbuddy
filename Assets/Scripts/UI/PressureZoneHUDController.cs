
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
    //slider elementså
    VisualElement m_PressureZoneSliderBar;
    VisualElement m_PressureZoneSliderTracker;
    VisualElement m_PressureZoneSliderHandle;
    VisualElement m_SliderFillBar;
    VisualElement m_NewDragger;
    VisualElement m_CurrentSlider;


    //booleans


    //root
    UIDocument root;


    //lists

    List<VisualElement> SliderHandleList;
    List<VisualElement> SliderBarList;
    List<VisualElement> SliderTrackerList;

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
        m_PressureZoneSliderBar = root.rootVisualElement.Query(name: PressureZoneSliderBarString);
        m_PressureZoneSliderTracker = root.rootVisualElement.Q<VisualElement>(PressureZoneSliderTrackerString);
        SliderHandleList = root.rootVisualElement.Query(name: "unity-dragger").ToList();
        SliderBarList = root.rootVisualElement.Query(className: "pressure-zone-slider").ToList();
        SliderTrackerList = root.rootVisualElement.Query(name: "unity-tracker").ToList();

        foreach (var dragger in SliderHandleList)
        {
            AddFillBarElements(dragger);
        }


        foreach (var sliderBar in SliderBarList)
        {
            AddNewDraggerElements(sliderBar);
            RegisterSliderCallBacks(sliderBar);
        }
        // foreach (var tracker in SliderTrackerList)
        // {
        //     AddNewDraggerElements(tracker);
        // }


    }


    void AddFillBarElements(VisualElement sliderHandle)
    {
        m_SliderFillBar = new VisualElement();
        sliderHandle.Add(m_SliderFillBar);
        m_SliderFillBar.name = "SliderFillBar";
        m_SliderFillBar.AddToClassList("fill-bar");
    }
    void AddNewDraggerElements(VisualElement sliderBar)
    {

        //new dragger handle
        m_NewDragger = new VisualElement();
        sliderBar.Add(m_NewDragger);
        m_NewDragger.name = "NewDragger";
        m_NewDragger.AddToClassList("new-dragger");
        m_NewDragger.pickingMode = PickingMode.Ignore;



    }


    void RegisterTextFieldCallBacks()
    {
        m_SupplyPressureTextField.RegisterCallback<ChangeEvent<string>>(InputValueChanged);
    }
    void RegisterSliderCallBacks(VisualElement slider)
    {
        slider.RegisterCallback<ChangeEvent<float>>(SliderValueChanged);
        slider.RegisterCallback<GeometryChangedEvent>(SliderInitialPositioning);


    }

    private void SliderInitialPositioning(GeometryChangedEvent evt)
    {
        VisualElement currentSliderBar = (VisualElement)evt.target;
        VisualElement currentDragger = currentSliderBar.Query(name: "unity-dragger");
        VisualElement currentNewDragger = currentSliderBar.Query(name: "NewDragger");

        Vector2 offset = new Vector2((currentNewDragger.layout.width - currentDragger.layout.width) / 2, (currentNewDragger.layout.height - currentDragger.layout.height) / 2);
        Vector2 position = currentDragger.parent.LocalToWorld(currentDragger.transform.position);

        currentNewDragger.transform.position = currentNewDragger.parent.WorldToLocal(position - offset);
    }

    private void SliderValueChanged(ChangeEvent<float> evt)
    {

        VisualElement currentSliderBar = (VisualElement)evt.target;
        VisualElement currentDragger = currentSliderBar.Query(name: "unity-dragger");
        VisualElement currentNewDragger = currentSliderBar.Query(name: "NewDragger");

        Vector2 offset = new Vector2((currentNewDragger.layout.width - currentDragger.layout.width) / 2, (currentNewDragger.layout.height - currentDragger.layout.height) / 2);
        Vector2 position = currentDragger.parent.LocalToWorld(currentDragger.transform.position);

        currentNewDragger.transform.position = currentNewDragger.parent.WorldToLocal(position - offset);


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
