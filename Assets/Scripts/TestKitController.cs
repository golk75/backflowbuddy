using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.zibra.liquid.Manipulators;

public class TestKitController : MonoBehaviour
{
    public WaterController waterController;
    public PlayerController playerController;
    public GameObject lowBleed;
    public GameObject lowControl;

    public GameObject highBleed;
    public GameObject highControl;

    public GameObject bypassControl;

    public GameObject needle;
    private GameObject currentKnob;

    [SerializeField]
    GameObject LowHose;

    [SerializeField]
    GameObject HighHose;

    [SerializeField]
    GameObject BypassHose;
    ZibraLiquidDetector LowHoseDetector;

    ZibraLiquidDetector HighHoseDetector;

    ZibraLiquidDetector BypassHoseDetector;

    private const float MinNeedle_rotation = 55;
    private const float MaxNeedle_rotation = -55;
    private const float MinKnob_rotation = 0;

    //limit knobs to 4 complete rotations (x1 rotation = 360;)->
    private const float MaxKnob_rotation = 1440;
    private float currentKnobRotCount;

    private float currentKnobRotation;
    private float maxKnobRotation;
    private float minKnobRotation;

    private float currentPSID;
    private float maxPSID;
    public bool isOperableObject;

    void OnEnable()
    {
        Actions.OnTestKitOperate += TestKitOperate;
    }

    void OnDisable()
    {
        Actions.OnTestKitOperate -= TestKitOperate;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentPSID = 0;
        maxPSID = 55;
        currentKnobRotation = 0;
        maxKnobRotation = 1440;
        minKnobRotation = 0;
    }

    private float GetPsidNeedleRotation()
    {
        float PsidDiff = MinNeedle_rotation - MaxNeedle_rotation;

        float normalizedPsid = currentPSID / maxPSID;

        return MinNeedle_rotation - normalizedPsid * PsidDiff;
    }

    private float GetKnobRotation()
    {
        // max - min to rotate left while increasing
        float rotationDiff = MaxKnob_rotation - MinKnob_rotation;

        float normalizedRotation = currentKnobRotation / maxKnobRotation;

        //return MinKnob_rotation + normalizedRotation * rotationDiff;
        return MinKnob_rotation + normalizedRotation * rotationDiff;
    }

    private void TestKitOperate(OperateTestKit testKit)
    {
        currentKnob = testKit.gameObject;

        //currentKnobRotation = testKit.gameObject.transform.eulerAngles.z;
    }

    /*
    private void OperateControls()
    {
        if (isOperableObject == true)
        {
            float counter = 0;
            currentKnobRotation = (
                playerController.touchStart.x
                - Camera.main.ScreenToWorldPoint(Input.mousePosition).x
            );
           
            if (currentKnob.transform.eulerAngles.z > maxKnobRotation)
            {
                currentKnobRotation = maxKnobRotation;
            }
            //currentKnob.transform.eulerAngles = new Vector3(0, 0, GetKnobRoation());
            currentKnob.transform.rotation = Quaternion.Euler(
                new Vector3(0, 0, currentKnob.transform.eulerAngles.z + GetKnobRotation() * 0.5f)
            );
            Debug.Log($"isOperableObject= {isOperableObject}; counter = {counter}");
        }
    }
    */
    private void OperateControls()
    {
        if (isOperableObject == true)
        {
            currentKnobRotation +=
                (
                    playerController.touchStart.x
                    - Camera.main.ScreenToWorldPoint(Input.mousePosition).x
                ) / 5;

            //currentKnobRotation += 1 * Time.deltaTime;

            if (currentKnobRotation > maxKnobRotation)
            {
                currentKnobRotation = maxKnobRotation;
            }
            if (currentKnobRotation < minKnobRotation)
            {
                currentKnobRotation = minKnobRotation;
            }

            currentKnob.transform.eulerAngles = new Vector3(0, 0, GetKnobRotation());
        }
    }

    private void NeedleControl()
    {
        float lowHosePressure;
        float highHosePressure;
        float bypasshosePressure;

        /*
        currentPSID += 1 * Time.deltaTime;
        if (currentPSID > maxPSID)
        {
            currentPSID = maxPSID;
        }
        needle.transform.eulerAngles = new Vector3(0, 0, GetPsidNeedleRotation());
        */
    }

    // Update is called once per frame
    void Update()
    {
        OperateControls();
        NeedleControl();
    }
}
