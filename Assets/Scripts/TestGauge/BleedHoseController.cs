using System.Collections;
using System.Collections.Generic;
using com.zibra.liquid.Manipulators;
using UnityEditor.Rendering;
using UnityEngine;
public class BleedHoseController : MonoBehaviour
{
    ZibraLiquidEmitter bleederHoseEmitter;
    float controlKnobRotation;
    public TestKitController testKitController;
    float currentFlow = 0;
    float appliedKnobRotation = 0;
    [SerializeField]
    GameObject highBleedKnob;





    private void OnEnable()
    {

        Actions.onHighBleedOperate += HighBleedKnobOperate;

    }
    private void OnDisable()
    {
        Actions.onHighBleedOperate -= HighBleedKnobOperate;

    }
    // Start is called before the first frame update
    void Start()
    {

        bleederHoseEmitter = GetComponentInChildren<ZibraLiquidEmitter>();

    }
    void HighBleedKnobOperate()
    {
        if (bleederHoseEmitter.VolumePerSimTime == 0)
        {
            bleederHoseEmitter.VolumePerSimTime = 1;
            Debug.Log($"High bleed opened");
        }
        else
        {

            bleederHoseEmitter.VolumePerSimTime = 0;
            Debug.Log($"High bleed closed");
        }
    }



    // Update is called once per frame
    void Update()
    {
        //reset cached knob rotation to not reset after rotating 180 degress--> zRot is rotating from 0 -> 180 -> -180 -> 0 -> 180..and so on
        // if (testKitController.currentKnob == highBleedKnob)
        // {
        //     appliedKnobRotation = testKitController.knobRotation;

        //     bleederHoseEmitter.VolumePerSimTime = appliedKnobRotation / 10000;

        // }





    }
}
