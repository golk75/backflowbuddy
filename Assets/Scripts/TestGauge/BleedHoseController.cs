using System;
using System.Collections;
using System.Collections.Generic;
using com.zibra.liquid.Manipulators;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
public class BleedHoseController : MonoBehaviour
{
    ZibraLiquidEmitter bleederHoseEmitter;
    float controlKnobRotation;
    public RpzTestKitController rpzTestKitController;
    float currentFlow = 0;
    float appliedKnobRotation = 0;
    [SerializeField]
    GameObject highBleedKnob;
    public bool isHighBleedOpen;
    public bool isLowBleedOpen;

    private void OnEnable()
    {

        Actions.onHighBleedOperate += HighBleedKnobOperate;
        Actions.onLowBleedOperate += LowBleedKnobOperate;

    }



    private void OnDisable()
    {
        Actions.onHighBleedOperate -= HighBleedKnobOperate;
        Actions.onLowBleedOperate -= LowBleedKnobOperate;


    }
    // Start is called before the first frame update
    void Start()
    {

        bleederHoseEmitter = GetComponentInChildren<ZibraLiquidEmitter>();

    }
    void HighBleedKnobOperate()
    {

        if (isHighBleedOpen == false)
        {
            isHighBleedOpen = true;
        }
        else
        {
            isHighBleedOpen = false;
        }

    }
    private void LowBleedKnobOperate()
    {
        if (isLowBleedOpen == false)
        {
            isLowBleedOpen = true;
        }
        else
        {
            isLowBleedOpen = false;
        }

    }
    // Update is called once per frame
    void Update()
    {
        // if (isLowBleedOpen && isHighBleedOpen)
        // {
        //     if (rpzTestKitController.isHighHoseEngaged && rpzTestKitController.isLowHoseEngaged)
        //         bleederHoseEmitter.VolumePerSimTime = 1;
        // }
        // else if (!isLowBleedOpen && isHighBleedOpen)
        // {
        //     if (rpzTestKitController.isHighHoseEngaged)
        //         bleederHoseEmitter.VolumePerSimTime = 1;
        // }
        // else if (isLowBleedOpen && !isHighBleedOpen)
        // {
        //     if (rpzTestKitController.isLowHoseEngaged)
        //         bleederHoseEmitter.VolumePerSimTime = 1;
        // }
        // else
        // {
        //     bleederHoseEmitter.VolumePerSimTime = 0;
        // }
    }
}
