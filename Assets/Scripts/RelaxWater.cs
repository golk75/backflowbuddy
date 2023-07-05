using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.DataStructures;

public class RelaxWater : MonoBehaviour
{
    [SerializeField]
    AssemblyController assemblyController;

    [SerializeField]
    AssemblyController_Touch assemblyController_Touch;

    [SerializeField]
    ZibraLiquidForceField checkValve1ForceField;

    [SerializeField]
    ZibraLiquidForceField checkValve2ForceField;

    [SerializeField]
    ZibraLiquidSolverParameters liquidSolverParameters;

    [SerializeField]
    ZibraLiquidForceField testCock1FF;

    [SerializeField]
    ZibraLiquidForceField testCock2FF;

    [SerializeField]
    ZibraLiquidForceField testCock3FF;

    [SerializeField]
    ZibraLiquidForceField testCock4FF;

    [SerializeField]
    ZibraLiquidDetector testCock1Detector;

    [SerializeField]
    ZibraLiquidDetector testCock2Detector;

    [SerializeField]
    ZibraLiquidDetector testCock3Detector;

    [SerializeField]
    ZibraLiquidDetector testCock4Detector;

    [SerializeField]
    ZibraLiquidDetector zone1Detector;

    [SerializeField]
    ZibraLiquidDetector zone2Detector;

    [SerializeField]
    ZibraLiquidDetector zone3Detector;

    AssemblyController_Touch controller;

    float currentVelocity;

    private void checkRelax()
    {
        if (assemblyController_Touch.IsSupplyOn == false)
        {
            //relax water in check housing
            checkValve1ForceField.enabled = false;
            checkValve2ForceField.enabled = false;
        }
        else
        {
            checkValve1ForceField.enabled = true;
            checkValve2ForceField.enabled = true;
        }

        //relax water @ test cocks
    }

    public void EventResponseTest()
    {
        Debug.Log($"Responding");
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<AssemblyController_Touch>();
    }

    // Update is called once per frame
    void Update()
    {
        checkRelax();
    }
}
