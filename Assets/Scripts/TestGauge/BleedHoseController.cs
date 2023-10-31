using System.Collections;
using System.Collections.Generic;
using com.zibra.liquid.Manipulators;
using UnityEditor.Rendering;
using UnityEngine;
public class BleedHoseController : MonoBehaviour
{
    ZibraLiquidEmitter bleederHoseEmitter;
    float controlKnobRotation;
    float bleederHoseEmitterVolume;
    public TestKitController testKitController;
    float currentFlow = 0;
    float appliedKnobRotation = 0;



    // Start is called before the first frame update
    void Start()
    {
        bleederHoseEmitter = GetComponentInChildren<ZibraLiquidEmitter>();
        bleederHoseEmitterVolume = bleederHoseEmitter.VolumePerSimTime;



    }



    // Update is called once per frame
    void Update()
    {
        //reset cached knob rotation to not reset after rotating 180 degress--> zRot is rotating from 0 -> 180 -> -180 -> 0 -> 180..and so on

        appliedKnobRotation = testKitController.knobRotation;



        Debug.Log($"appliedKnobRotation = {appliedKnobRotation}; controlKnobRotation = {controlKnobRotation}");





    }
}
