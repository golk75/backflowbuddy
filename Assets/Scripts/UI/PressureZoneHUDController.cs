
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    const string SupplyPressurePanelString = "SupplyPressure__panel";
    const string CheckSpring1ValueLabelString = "CheckSpring1_value_label";
    const string CheckSpring2ValueLabelString = "CheckSpring2_value_label";
    const string CheckSpring1AddButtonString = "CheckSpring1_add_button";
    const string CheckSpring2AddButtonString = "CheckSpring2_add_button";
    const string CheckSpring1SubtractButtonString = "CheckSpring1_subtract_button";
    const string CheckSpring2SubtractButtonString = "CheckSpring2_subtract_button";

    //visual elements
    public TextField m_SupplyPressureTextField;
    Label m_PressureZone2TextLabel;
    Label m_PressureZone3TextField;
    Label m_CheckSpring1Value;
    Label m_CheckSpring2Value;
    Button m_CheckSpring1AddButton;
    Button m_CheckSpring1SubtractButton;
    Button m_CheckSpring2AddButton;
    Button m_CheckSpring2SubtractButton;
    VisualElement m_SupplyPressurePanel;


    //slider elements
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
    public float maxSpringPressure = 20f;
    public float check1SpringPressure = 10;
    public float check2SpringPressure = 5;




    //coroutines
    Coroutine OnIncreaseValue;
    Coroutine OnDecreaseValue;
    Coroutine OnSupplyPanelMove;

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
        RegisterPanelCallBacks();

        m_SupplyPressureTextField.isDelayed = false;
        m_SupplyPressureTextField.value = waterController.supplyPsi.ToString();


    }


    void SetVisualElements()
    {
        root = GetComponent<UIDocument>();
        m_SupplyPressurePanel = root.rootVisualElement.Q<VisualElement>(SupplyPressurePanelString);
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



        //addition down and up
        m_CheckSpring1AddButton.RegisterCallback<PointerDownEvent>(SpringCheck1AdditionButton_down, TrickleDown.TrickleDown);
        m_CheckSpring1AddButton.RegisterCallback<PointerUpEvent>(SpringCheck1Addition_up);
        m_CheckSpring2AddButton.RegisterCallback<PointerDownEvent>(SpringCheck2AdditionButton_down, TrickleDown.TrickleDown);
        m_CheckSpring2AddButton.RegisterCallback<PointerUpEvent>(SpringCheck2AdditionButton_up);

        //subtract down and up
        m_CheckSpring1SubtractButton.RegisterCallback<PointerDownEvent>(SpringCheck1SubtractButton_down, TrickleDown.TrickleDown);
        m_CheckSpring1SubtractButton.RegisterCallback<PointerUpEvent>(SpringCheck1SubtractButton_up);
        m_CheckSpring2SubtractButton.RegisterCallback<PointerDownEvent>(SpringCheck2SubtractButton_down, TrickleDown.TrickleDown);
        m_CheckSpring2SubtractButton.RegisterCallback<PointerUpEvent>(SpringCheck2SubtractButton_up);

    }

    /// <summary>
    /// Check Spring #2 button events
    /// </summary>
    /// <param name="evt"></param>

    //add
    private void SpringCheck2AdditionButton_down(PointerDownEvent evt)
    {
        isPointerDown = true;

        OnIncreaseValue = StartCoroutine(IncreaseCheckSpring2Value());
    }

    private void SpringCheck2AdditionButton_up(PointerUpEvent evt)
    {
        isPointerDown = false;

    }

    //subtract
    private void SpringCheck2SubtractButton_down(PointerDownEvent evt)
    {
        isPointerDown = true;

        OnDecreaseValue = StartCoroutine(DecreaseCheckSpring2Value());
    }

    private void SpringCheck2SubtractButton_up(PointerUpEvent evt)
    {
        isPointerDown = false;
    }




    /// <summary>
    /// Check Spring #1 button events
    /// </summary>
    /// <param name="evt"></param>


    //add
    private void SpringCheck1AdditionButton_down(PointerDownEvent evt)
    {
        isPointerDown = true;

        OnIncreaseValue = StartCoroutine(IncreaseCheckSpring1Value());

    }

    private void SpringCheck1Addition_up(PointerUpEvent evt)
    {
        isPointerDown = false;
    }

    //subtract
    private void SpringCheck1SubtractButton_down(PointerDownEvent evt)
    {
        isPointerDown = true;

        OnDecreaseValue = StartCoroutine(DecreaseCheckSpring1Value());
        // waterController.check1SpringForce += 1;
    }

    private void SpringCheck1SubtractButton_up(PointerUpEvent evt)
    {
        isPointerDown = false;

    }




    //increase spring 1 pressure on "add" button click and/or hold
    IEnumerator IncreaseCheckSpring1Value()
    {
        check1SpringPressure += 1;
        yield return new WaitForSeconds(1.5f);

        while (isPointerDown == true)
        {
            check1SpringPressure += 1;

            yield return null;
        }


    }
    IEnumerator DecreaseCheckSpring1Value()
    {
        check1SpringPressure -= 1;
        yield return new WaitForSeconds(1.5f);

        while (isPointerDown == true && check1SpringPressure > 0)
        {
            check1SpringPressure -= 1;
            yield return null;
        }
        //

    }
    IEnumerator IncreaseCheckSpring2Value()
    {
        check2SpringPressure += 1;
        yield return new WaitForSeconds(1.5f);

        while (isPointerDown == true)
        {
            check2SpringPressure += 1;
            yield return null;
        }


    }
    IEnumerator DecreaseCheckSpring2Value()
    {
        check2SpringPressure -= 1;
        yield return new WaitForSeconds(1.5f);

        while (isPointerDown == true && check2SpringPressure > 0)
        {
            check2SpringPressure -= 1;
            yield return null;
        }


    }

    Vector3 GetPointerPos()
    {
        Vector3 screenPosition = Input.mousePosition;

        // If you're using a perspective camera for parallax,
        // be sure to assign a depth to this point.
        // screenPosition.z = 1f;

        return Camera.main.ScreenToWorldPoint(screenPosition);
    }
    void RegisterSliderCallBacks(VisualElement slider)
    {
        slider.RegisterCallback<ChangeEvent<float>>(SliderValueChanged);
        slider.RegisterCallback<GeometryChangedEvent>(SliderInitialPositioning);

    }
    void RegisterPanelCallBacks()
    {
        m_SupplyPressurePanel.RegisterCallback<PointerDownEvent>(PanelMoveEvent);
    }

    private void PanelMoveEvent(PointerDownEvent evt)
    {
        // Vector3 pointerOrigin = m_SupplyPressurePanel.parent.LocalToWorld(m_SupplyPressurePanel.transform.position);
        // Vector3 currentPointerPos = GetPointerPos();
        // // Vector3 targetPointerPosition = currentPointerPos - pointerOrigin;
        // m_SupplyPressurePanel.transform.position = m_SupplyPressurePanel.parent.WorldToLocal(pointerOrigin - currentPointerPos);
        OnSupplyPanelMove = StartCoroutine(MovePanel());

    }

    private IEnumerator MovePanel()
    {
        Vector3 pointerOrigin = m_SupplyPressurePanel.parent.LocalToWorld(m_SupplyPressurePanel.transform.position);
        Vector3 currentPointerPos = GetPointerPos();
        // Vector3 targetPointerPosition = currentPointerPos - pointerOrigin;
        while (playerController.primaryTouchStarted > 0)
        {
            m_SupplyPressurePanel.transform.position = m_SupplyPressurePanel.parent.WorldToLocal(pointerOrigin - currentPointerPos);
            yield return null;
        }

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
            case SupplyPressurePanelString:

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



    //regulate min-max values as well as setting text in ui label
    void CheckSpring1Regulate()
    {
        waterController.check1SpringForce = check1SpringPressure;
        if (check1SpringPressure >= maxSpringPressure)
        {


            check1SpringPressure = maxSpringPressure;
            m_CheckSpring1Value.text = maxSpringPressure.ToString();
        }
        else if (check1SpringPressure > 0 && check1SpringPressure < maxSpringPressure)
        {
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
        waterController.check2SpringForce = check2SpringPressure;
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


        m_PressureZone2TextLabel.text = waterController.zone2Pressure.ToString();
        m_PressureZone3TextField.text = waterController.zone3Pressure.ToString();




    }


}
