
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UIElements;

public class PressureZoneHUDController : MonoBehaviour
{

    //game objects
    public WaterController waterController;
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
    Label m_PressureZone2TextLabel;
    Label m_PressureZone3TextField;
    Label m_CheckSpring1Value;
    Label m_CheckSpring2Value;
    Label m_Zone2PressureSliderValue;
    Label m_Zone3PressureSliderValue;
    Button m_CheckSpring1AddButton;
    Button m_CheckSpring1SubtractButton;
    Button m_CheckSpring2AddButton;
    Button m_CheckSpring2SubtractButton;
    VisualElement m_SupplyPressurePanel;
    VisualElement m_PressureZone2Panel;
    VisualElement m_PressureZone3Panel;


    //slider elements

    public VisualElement m_DropAreaTopSlot;
    public VisualElement m_DropAreaMidSlot;
    public VisualElement m_DropAreaBotSlot;


    //booleans
    public bool isPointerDown = false;

    //root
    UIDocument root;


    //lists
    List<VisualElement> SlidersToResetList;
    List<VisualElement> SliderBarList;
    public List<VisualElement> DropAreaSlotList = new List<VisualElement>();


    //floats
    public float maxSpringPressure = 50f;
    public float check1SpringPressure;
    public float check2SpringPressure;
    public float check1SpringInitPressure;
    public float check2SpringInitPressure;


    void Start()
    {

        SetVisualElements();
        RegisterCallBacks();

        m_SupplyPressurePanel.pickingMode = PickingMode.Ignore;
        m_PressureZone2Panel.pickingMode = PickingMode.Ignore;
        m_PressureZone3Panel.pickingMode = PickingMode.Ignore;


    }


    void SetVisualElements()
    {
        root = GetComponent<UIDocument>();
        m_SupplyPressurePanel = root.rootVisualElement.Q<VisualElement>(SupplyPressurePanelTemplateString);


        m_PressureZone2Panel = root.rootVisualElement.Q<VisualElement>(PressureZone2PanelTemplateString);
        m_PressureZone3Panel = root.rootVisualElement.Q<VisualElement>(PressureZone3PanelTemplateString);
        // m_SupplyPressureTextField = root.rootVisualElement.Q<TextField>(SupplyPressureTextString);
        m_SupplyPressureTextField = root.rootVisualElement.Q<Label>(SupplyPressureTextString);
        m_PressureZone2TextLabel = root.rootVisualElement.Q<Label>(PressureZone2LabelString);
        m_PressureZone3TextField = m_PressureZone3Panel.Q<Label>(PressureZone3LabelString);

        SliderBarList = root.rootVisualElement.Query(className: "pressure-zone-slider").ToList();

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

        if (waterController.isActiveAndEnabled)
        {

            // m_SupplyPressureTextField.value = waterController.supplyPsi.ToString();
            m_SupplyPressureTextField.text = waterController.supplyPsi.ToString();


        }
        else
        {
            // m_SupplyPressureTextField.value = m_rpzWaterController.supplyPsi.ToString();
            m_SupplyPressureTextField.text = m_rpzWaterController.supplyPsi.ToString();
            check1SpringPressure = m_rpzWaterController.check1SpringForce;
            check2SpringPressure = m_rpzWaterController.check2SpringForce;

        }


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
        VisualElement currentSliderBar = (VisualElement)evt.target;
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

                waterController.zone2PsiChange = zonePressureSliderValue;
                m_rpzWaterController.zone2PsiChange = zonePressureSliderValue;
                if (zonePressureSliderValue >= 0)
                {
                    m_Zone2PressureSliderValue.text = "+" + zonePressureSliderValue.ToString();
                }
                else
                {
                    m_Zone2PressureSliderValue.text = zonePressureSliderValue.ToString();
                }
                break;

            case PressureZone3PanelTemplateString:
                waterController.zone3PsiChange = zonePressureSliderValue;
                m_rpzWaterController.zone3PsiChange = zonePressureSliderValue;
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
        if (waterController.isActiveAndEnabled)
        {
            waterController.check1SpringForce = check1SpringPressure;
        }
        else
        {

            m_rpzWaterController.check1SpringForce = check1SpringPressure;
        }

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
        if (waterController.isActiveAndEnabled)
        {
            waterController.check2SpringForce = check2SpringPressure;
        }
        else
        {

            m_rpzWaterController.check2SpringForce = check2SpringPressure;

        }

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


    // Update is called once per frame
    void Update()
    {
        CheckSpring1Regulate();
        CheckSpring2Regulate();

        if (waterController.isActiveAndEnabled)
        {
            m_PressureZone2TextLabel.text = waterController.zone2Pressure.ToString();
            m_PressureZone3TextField.text = waterController.zone3Pressure.ToString();
            m_SupplyPressureTextField.text = waterController.supplyPsi.ToString();

        }
        else
        {
            m_PressureZone2TextLabel.text = m_rpzWaterController.zone2Pressure.ToString();
            m_PressureZone3TextField.text = m_rpzWaterController.zone3Pressure.ToString();
            m_SupplyPressureTextField.text = m_rpzWaterController.zone1Pressure.ToString();
        }



    }


}
