
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UIElements;

public class RPZPressureZoneHUDController : MonoBehaviour
{

    //game objects
    public DCWaterController waterController;
    public RPZWaterController m_rpzWaterController;
    public PlayerController playerController;
    public UiClickFilter uiClickFilter;
    public GameObject ActiveWaterController;


    //string ids
    const string SupplyPressureTextString = "SupplyPressureZone_value_label";
    const string PressureZone2LabelString = "PressureZone2_value_label";
    const string PressureZone3LabelString = "PressureZone3_value_label";


    const string PressureZone2PanelTemplateString = "PressureZone2__panel";
    const string PressureZone3PanelTemplateString = "PressureZone3__panel";
    // const string SupplyPressurePanelTemplateString = "SupplyPressure__panelTemp";
    const string SupplyPressurePanelTemplateString = "SupplyPressureZone__panel";
    const string CheckSpring1ValueLabelString = "CheckSpring1_value_label";
    const string CheckSpring2ValueLabelString = "CheckSpring2_value_label";
    const string CheckSpring1AddButtonString = "CheckSpring1_add_button";
    const string CheckSpring2AddButtonString = "CheckSpring2_add_button";
    const string CheckSpring1SubtractButtonString = "CheckSpring1_subtract_button";
    const string CheckSpring2SubtractButtonString = "CheckSpring2_subtract_button";
    const string DropAreaTopSlotString = "PressurePanel_slot_top";
    const string DropAreaMidSlotString = "PressurePanel_slot_middle";
    const string DropAreaBotSlotString = "PressurePanel_slot_bottom";

    //visual elements
    // public TextField m_SupplyPressureTextField;
    public Label m_SupplyPressureTextField;
    public Label m_PressureZone2TextLabel;
    public Label m_PressureZone3TextField;
    Label m_CheckSpring1Value;
    Label m_CheckSpring2Value;
    public Label m_Zone2PressureSliderValue;
    public Label m_Zone3PressureSliderValue;
    Button m_CheckSpring1AddButton;
    Button m_CheckSpring1SubtractButton;
    Button m_CheckSpring2AddButton;
    Button m_CheckSpring2SubtractButton;
    public VisualElement m_SupplyPressurePanel;
    public VisualElement m_PressureZone2Panel;
    public VisualElement m_PressureZone3Panel;
    public Slider m_SupplyPressurePanelSlider;
    public Slider m_PressureZone2PanelSlider;
    public Slider m_PressureZone3PanelSlider;

    //slider elements

    public VisualElement m_DropAreaTopSlot;
    public VisualElement m_DropAreaMidSlot;
    public VisualElement m_DropAreaBotSlot;


    //booleans
    public bool isPointerDown = false;

    //root
    UIDocument root;


    //lists
    List<VisualElement> DraggersToReset;
    List<VisualElement> DummyDraggersToReset;
    List<VisualElement> SliderBarList;
    public List<VisualElement> DropAreaSlotList = new List<VisualElement>();

    VisualElement currentSliderBar;
    //floats
    public float maxSpringPressure = 50f;
    public float check1SpringPressure;
    public float check2SpringPressure;
    public float check1SpringInitPressure;
    public float check2SpringInitPressure;
    public GameObject reliefSpringFrontSeat;
    public ConfigurableJoint reliefSpring;
    public float reliefSpringXDrive;
    public float reliefSpringXDamper;
    public float sliderToReliefSpringFactor;
    public float springDamperFactor;
    void Start()
    {

        SetVisualElements();
        RegisterCallBacks();

        m_SupplyPressurePanel.pickingMode = PickingMode.Ignore;
        m_PressureZone2Panel.pickingMode = PickingMode.Ignore;
        m_PressureZone3Panel.pickingMode = PickingMode.Ignore;


    }
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        // Actions.onHighControlOperate += HighControlKnobOperate;
        // Actions.onLowControlOperate += LowControlKnobOperate;
        // Actions.onBypassControlOperate += BypassControlOperate;
    }



    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        // Actions.onHighControlOperate -= HighControlKnobOperate;
        // Actions.onLowControlOperate -= LowControlKnobOperate;
        // Actions.onBypassControlOperate -= BypassControlOperate;
    }

    void SetVisualElements()
    {
        root = GetComponent<UIDocument>();
        m_SupplyPressurePanel = root.rootVisualElement.Q<VisualElement>(SupplyPressurePanelTemplateString);
        m_PressureZone2Panel = root.rootVisualElement.Q<VisualElement>(PressureZone2PanelTemplateString);
        m_PressureZone3Panel = root.rootVisualElement.Q<VisualElement>(PressureZone3PanelTemplateString);
        m_SupplyPressurePanelSlider = m_SupplyPressurePanel.Q<Slider>(className: "pressure-zone-slider");
        m_PressureZone2PanelSlider = m_PressureZone2Panel.Q<Slider>(className: "pressure-zone-slider");
        m_PressureZone3PanelSlider = m_PressureZone3Panel.Q<Slider>(className: "pressure-zone-slider");

        // m_SupplyPressureTextField = root.rootVisualElement.Q<TextField>(SupplyPressureTextString);
        m_SupplyPressureTextField = root.rootVisualElement.Q<Label>(SupplyPressureTextString);
        m_PressureZone2TextLabel = root.rootVisualElement.Q<Label>(PressureZone2LabelString);
        m_PressureZone3TextField = m_PressureZone3Panel.Q<Label>(PressureZone3LabelString);

        SliderBarList = root.rootVisualElement.Query(className: "pressure-zone-slider").ToList();
        // DraggersToReset = root.rootVisualElement.Query(className: "unity-base-slider__dragger").ToList();
        DraggersToReset = root.rootVisualElement.Query("unity-dragger").ToList();
        DummyDraggersToReset = root.rootVisualElement.Query(className: "dummy-dragger").ToList();

        m_CheckSpring1Value = m_SupplyPressurePanel.Q<Label>(CheckSpring1ValueLabelString);
        m_CheckSpring2Value = m_SupplyPressurePanel.Q<Label>(CheckSpring2ValueLabelString);
        m_CheckSpring1AddButton = m_SupplyPressurePanel.Q<Button>(CheckSpring1AddButtonString);
        m_CheckSpring1SubtractButton = m_SupplyPressurePanel.Q<Button>(CheckSpring1SubtractButtonString);
        m_CheckSpring2AddButton = m_SupplyPressurePanel.Q<Button>(CheckSpring2AddButtonString);
        m_CheckSpring2SubtractButton = m_SupplyPressurePanel.Q<Button>(CheckSpring2SubtractButtonString);
        m_DropAreaTopSlot = root.rootVisualElement.Q<VisualElement>(DropAreaTopSlotString);
        m_DropAreaMidSlot = root.rootVisualElement.Q<VisualElement>(DropAreaMidSlotString);
        m_DropAreaBotSlot = root.rootVisualElement.Q<VisualElement>(DropAreaBotSlotString);

        m_Zone2PressureSliderValue = m_PressureZone2Panel.Q<Label>("slider-value-label");
        m_Zone3PressureSliderValue = m_PressureZone3Panel.Q<Label>("slider-value-label");



        DropAreaSlotList.Add(m_DropAreaTopSlot);
        DropAreaSlotList.Add(m_DropAreaMidSlot);
        DropAreaSlotList.Add(m_DropAreaBotSlot);

        //add dragger manipulator
        m_SupplyPressurePanel.AddManipulator(new PanelDragger(m_SupplyPressurePanel));
        m_PressureZone2Panel.AddManipulator(new PanelDragger(m_PressureZone2Panel));
        m_PressureZone3Panel.AddManipulator(new PanelDragger(m_PressureZone3Panel));




        /// <summary>
        /// This if statement changes the active water controller being refernced. This is neccessary because, at this time, there
        ///  are seperate water controllers for each backflow assembly type
        /// </summary>

        // if (waterController.isActiveAndEnabled)
        // {

        //     // m_SupplyPressureTextField.value = waterController.supplyPsi.ToString();
        //     m_SupplyPressureTextField.text = waterController.supplyPsi.ToString();


        // }
        // else
        // {
        //     // m_SupplyPressureTextField.value = m_rpzWaterController.supplyPsi.ToString();


        // }
        m_SupplyPressureTextField.text = m_rpzWaterController.supplyPsi.ToString();
        check1SpringPressure = m_rpzWaterController.check1SpringForce;
        check2SpringPressure = m_rpzWaterController.check2SpringForce;
        // foreach (var dummyDragger in DummyDraggersToReset)
        // {
        //     // dummyDragger.AddManipulator(new DraggerHandle(dummyDragger));
        //     // var target = m_SupplyPressurePanel.Q<VisualElement>("unity-dragger");



        // }
        // foreach (var dragger in DraggersToReset)
        // {
        //     dragger.AddManipulator(new DraggerHandle(dragger));
        //     // var target = m_SupplyPressurePanel.Q<VisualElement>("unity-dragger");



        // }


    }


    void RegisterCallBacks()
    {

        m_CheckSpring1AddButton.RegisterCallback<ClickEvent>(evt => Check1SpringAdd());
        m_CheckSpring1SubtractButton.RegisterCallback<ClickEvent>(evt => Check1SpringSubtract());
        m_CheckSpring2AddButton.RegisterCallback<ClickEvent>(evt => Check2SpringAdd());
        m_CheckSpring2SubtractButton.RegisterCallback<ClickEvent>(evt => Check2SpringSubtract());

        foreach (var sliderBar in SliderBarList)
        {
            RegisterSliderCallBacks(sliderBar);
        }


    }




    private void Check2SpringSubtract()
    {
        check2SpringPressure -= 1;
    }

    private void Check2SpringAdd()
    {
        check2SpringPressure += 1;
    }

    private void Check1SpringSubtract()
    {
        check1SpringPressure -= 1;
    }

    private void Check1SpringAdd()
    {
        check1SpringPressure += 1;
    }


    void RegisterSliderCallBacks(VisualElement slider)
    {
        slider.RegisterCallback<ChangeEvent<float>>(SliderValueChanged);
    }



    private void SliderValueChanged(ChangeEvent<float> evt)
    {


        uiClickFilter.isUiClicked = true;
        currentSliderBar = (VisualElement)evt.target;
        VisualElement currentDragger = currentSliderBar.Query(name: "unity-dragger");

        ZonePressureOperations(evt.newValue, SearchHiearchy.GetFirstAncestorWithClass(currentSliderBar, "floating"));

    }

    void ZonePressureOperations(float zonePressureSliderValue, VisualElement zonePressureSlider)
    {

        switch (zonePressureSlider.name)
        {

            case SupplyPressurePanelTemplateString:

                m_rpzWaterController.supplyPsi = zonePressureSliderValue;
                break;

            case PressureZone2PanelTemplateString:


                // m_rpzWaterController.zone2PsiChange = (int)zonePressureSliderValue;
                m_rpzWaterController.zone2PsiChange = zonePressureSliderValue;
                // ReliefValveControl(zonePressureSliderValue * sliderToReliefSpringFactor);
                // reliefSpringXDrive = zonePressureSliderValue / sliderToReliefSpringFactor;
                if (zonePressureSliderValue >= 0)
                {
                    m_Zone2PressureSliderValue.text = "+" + ((int)zonePressureSliderValue).ToString();
                }
                else
                {
                    m_Zone2PressureSliderValue.text = ((int)zonePressureSliderValue).ToString();
                }
                break;

            case PressureZone3PanelTemplateString:

                if (m_rpzWaterController.zone3PsiChange >= 0)
                {
                    m_rpzWaterController.zone3PsiChange = (int)zonePressureSliderValue;
                }
                else
                {
                    m_rpzWaterController.zone3PsiChange = 0 - (int)zonePressureSliderValue;
                }


                if (zonePressureSliderValue >= 0)
                {
                    m_Zone3PressureSliderValue.text = "+" + zonePressureSliderValue.ToString();
                }
                else
                {
                    m_Zone3PressureSliderValue.text = zonePressureSliderValue.ToString();
                }
                break;

            default:
                throw new Exception($"{zonePressureSlider.name} does not match the name of slider being used");

        }



    }



    //regulate min-max values as well as setting text in ui label
    void CheckSpring1Regulate()
    {





        m_rpzWaterController.check1SpringForce = check1SpringPressure;


        if (check1SpringPressure >= maxSpringPressure)
        {

            m_CheckSpring1Value.text = maxSpringPressure.ToString();
        }
        else if (check1SpringPressure > 0 && check1SpringPressure < maxSpringPressure)
        {
            // Debug.Log($"m_CheckSpring1Value.text: {m_CheckSpring1Value.text}");
            m_CheckSpring1Value.text = ((short)check1SpringPressure).ToString();
        }
        else
        {
            check1SpringPressure = 0;
            m_CheckSpring1Value.text = check1SpringPressure.ToString();
        }
    }


    void CheckSpring2Regulate()
    {



        m_rpzWaterController.check2SpringForce = check2SpringPressure;


        if (check2SpringPressure >= maxSpringPressure)
        {

            check2SpringPressure = maxSpringPressure;
            m_CheckSpring2Value.text = maxSpringPressure.ToString();
        }
        else if (check2SpringPressure > 0 && check2SpringPressure < maxSpringPressure)
        {

            m_CheckSpring2Value.text = ((short)check2SpringPressure).ToString();

        }
        else
        {

            check2SpringPressure = 0;
            m_CheckSpring2Value.text = check2SpringPressure.ToString();
        }
    }
    private void ReliefValveControl(float sliderValue)
    {


        // JointDrive drive = reliefSpring.xDrive;
        // drive.positionSpring = sliderValue;
        // drive.positionDamper = sliderValue * springDamperFactor;
        // reliefSpring.xDrive = drive;
        // var currentPos = reliefSpringFrontSeat.transform.localPosition;
        // currentPos.x -= -sliderValue;
        // if (currentPos.x > -0.0861f)
        // {
        //     currentPos.x -= sliderValue;
        // }

        // reliefSpringFrontSeat.transform.localPosition = currentPos;




    }


    // Update is called once per frame
    void Update()
    {
        CheckSpring1Regulate();
        CheckSpring2Regulate();

        m_PressureZone2TextLabel.text = (Mathf.Round(m_rpzWaterController.zone2Pressure * 10) * 0.1f).ToString();
        m_PressureZone3TextField.text = (Mathf.Round(m_rpzWaterController.zone3Pressure * 10) * 0.1f).ToString();
        m_SupplyPressureTextField.text = (Mathf.Round(m_rpzWaterController.zone1Pressure * 10) * 0.1f).ToString();








    }


}
