
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UIElements;

public class PressureZoneHUDController : MonoBehaviour
{

    //game objects
    public WaterController waterController;
    public PlayerController playerController;

    //string ids
    const string SupplyPressureTextString = "SupplyPressure__value";
    const string PressureZone2LabelString = "PressureZone2_value_label";
    const string PressureZone3LabelString = "PressureZone3_value_label";
    const string PressureZoneSliderBarString = "PressureZoneSlider";
    const string PressureZoneSliderTrackerString = "unity-tracker";
    const string PressureZoneSliderHandleString = "unity-dragger";
    const string PressureZone2Panel = "PressureZone2__panel";
    const string PressureZone3Panel = "PressureZone3__panel";
    const string SupplyPressurePanel = "SupplyPressure__panel";
    const string CheckSpring1ValueLabelString = "CheckSpring1_value_label";
    const string CheckSpring2ValueLabelString = "CheckSpring2_value_label";
    const string CheckSpring1AddButtonString = "CheckSpring1_add_button";
    const string CheckSpring2AddButtonString = "CheckSpring2_add_button";
    const string CheckSpring1SubtractButtonString = "CheckSpring1_subtract_button";
    const string CheckSpring2SubtractButtonString = "CheckSpring2_subtract_button";

    //visual elements
    TextField m_SupplyPressureTextField;
    Label m_PressureZone2TextLabel;
    Label m_PressureZone3TextField;
    Label m_CheckSpring1Value;
    Label m_CheckSpring2Value;
    Button m_CheckSpring1AddButton;
    Button m_CheckSpring1SubtractButton;
    Button m_CheckSpring2AddButton;
    Button m_CheckSpring2SubtractButton;


    //slider elementså
    VisualElement m_PressureZoneSliderBar;
    VisualElement m_PressureZoneSliderTracker;
    VisualElement m_PressureZoneSliderHandle;
    VisualElement m_SliderFillBar;
    VisualElement m_NewDragger;
    VisualElement m_CurrentSlider;


    //booleans
    public bool isPointerDown = false;

    //root
    UIDocument root;


    //lists

    List<VisualElement> SliderHandleList;
    List<VisualElement> SliderBarList;
    List<VisualElement> SliderTrackerList;


    //floats



    //coroutines
    Coroutine OnIncreaseValue;
    Coroutine OnDecreaseValue;

    void OnEnable()
    {

    }
    void OnDisable()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        SetVisualElements();
        RegisterTextFieldCallBacks();
        RegisterButtonCallBacks();
        m_SupplyPressureTextField.isDelayed = false;
        // m_PressureZone2TextLabel.isDelayed = false;
        // m_PressureZone3TextField.isDelayed = false;
    }


    void SetVisualElements()
    {
        root = GetComponent<UIDocument>();
        m_SupplyPressureTextField = root.rootVisualElement.Q<TextField>(SupplyPressureTextString);
        m_PressureZone2TextLabel = root.rootVisualElement.Q<Label>(PressureZone2LabelString);
        m_PressureZone3TextField = root.rootVisualElement.Q<Label>(PressureZone3LabelString);
        m_PressureZoneSliderBar = root.rootVisualElement.Query(name: PressureZoneSliderBarString);
        m_PressureZoneSliderTracker = root.rootVisualElement.Q<VisualElement>(PressureZoneSliderTrackerString);
        SliderHandleList = root.rootVisualElement.Query(name: "unity-dragger").ToList();
        SliderBarList = root.rootVisualElement.Query(className: "pressure-zone-slider").ToList();
        SliderTrackerList = root.rootVisualElement.Query(name: "unity-tracker").ToList();
        m_CheckSpring1Value = root.rootVisualElement.Q<Label>(CheckSpring1ValueLabelString);
        m_CheckSpring2Value = root.rootVisualElement.Q<Label>(CheckSpring2ValueLabelString);
        m_CheckSpring1AddButton = root.rootVisualElement.Q<Button>(CheckSpring1AddButtonString);
        m_CheckSpring1SubtractButton = root.rootVisualElement.Q<Button>(CheckSpring1SubtractButtonString);
        m_CheckSpring2AddButton = root.rootVisualElement.Q<Button>(CheckSpring2AddButtonString);
        m_CheckSpring2SubtractButton = root.rootVisualElement.Q<Button>(CheckSpring2SubtractButtonString);




        foreach (var dragger in SliderHandleList)
        {
            AddFillBarElements(dragger);
        }
        //

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

    void RegisterButtonCallBacks()
    {

        m_CheckSpring1AddButton.RegisterCallback<PointerDownEvent>(SpringButtonAddition_down, TrickleDown.TrickleDown);
        m_CheckSpring1AddButton.RegisterCallback<PointerUpEvent>(SpringButtonAddition_up);

        m_CheckSpring1AddButton.RegisterCallback<ClickEvent>(IncreaseSpring1Pressure, TrickleDown.TrickleDown);
        m_CheckSpring1SubtractButton.RegisterCallback<ClickEvent>(DecreaseSpring1Pressure);
        m_CheckSpring2AddButton.RegisterCallback<ClickEvent>(IncreaseSpring2Pressure);
        m_CheckSpring2SubtractButton.RegisterCallback<ClickEvent>(DecreaseSpring2Pressure);




    }

    private void SpringButtonAddition_up(PointerUpEvent evt)
    {
        isPointerDown = false;

    }

    private void SpringButtonAddition_down(PointerDownEvent evt)
    {
        isPointerDown = true;
        OnIncreaseValue = StartCoroutine(IncreaseValue());
        // waterController.check1SpringForce += 1;
    }

    IEnumerator IncreaseValue()
    {
        while (isPointerDown == true)
        {
            waterController.check1SpringForce += 1;
        }
        yield return null;

    }



    //increase individual spring pressures
    private void IncreaseSpring1Pressure(ClickEvent evt)
    {


    }

    private void DecreaseSpring1Pressure(ClickEvent evt)
    {
        // Debug.Log($"evt: {evt.target}");
        waterController.check1SpringForce -= 1;
    }



    private void IncreaseSpring2Pressure(ClickEvent evt)
    {
        // Debug.Log($"evt: {evt.target}");
        waterController.check2SpringForce += 1;
    }

    private void DecreaseSpring2Pressure(ClickEvent evt)
    {
        // Debug.Log($"evt: {evt.target}");
        waterController.check2SpringForce -= 1;
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
        // Debug.Log($"evt: {evt.newValue}");
        ZonePressureOperations(evt.newValue, currentSliderBar.parent.parent);

    }


    private void InputValueChanged(ChangeEvent<string> evt)
    {

        bool isInt = Int32.TryParse(evt.newValue, out int result);


        waterController.supplyPsi = result;
    }


    void ZonePressureOperations(float zonePressureSliderValue, VisualElement zonePressureSlider)
    {

        switch (zonePressureSlider.name)
        {
            case SupplyPressurePanel:

                break;
            case PressureZone2Panel:
                waterController.zone2PsiChange = zonePressureSliderValue;
                // Debug.Log($"Zone2 slider operated");
                break;
            case PressureZone3Panel:
                waterController.zone3PsiChange = zonePressureSliderValue;
                // Debug.Log($"Zone3 slider operated");
                break;
            default:
                throw new Exception($"{zonePressureSlider.name} does not match the name of slider being used");

        }
        // Debug.Log($"zonePressureValue: {zonePressureValue} ; zonePressureSlider: {zonePressureSlider.name}");
    }


    // Update is called once per frame
    void Update()
    {

        m_PressureZone2TextLabel.text = waterController.zone2Pressure.ToString();
        m_PressureZone3TextField.text = waterController.zone3Pressure.ToString();
        m_CheckSpring1Value.text = waterController.check1SpringForce.ToString();
        m_CheckSpring2Value.text = waterController.check2SpringForce.ToString();

    }


}
