using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.zibra.liquid.Manipulators;
using System;

public class TestCockPressure : MonoBehaviour
{
    ZibraLiquidDetector detector;

    [SerializeField]
    ZibraLiquidForceField forceField;

    public bool isTcPrimed;

    // Start is called before the first frame update
    void Start()
    {
        detector = GetComponent<ZibraLiquidDetector>();
        forceField = GetComponentInChildren<ZibraLiquidForceField>();
        EventManager.TestcockEvent += CheckTcPsi;
    }

    private void CheckTcPsi()
    {
        Debug.Log($"detect.ParticlesInside = {detector.ParticlesInside}");
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        EventManager.TestcockEvent -= CheckTcPsi;
    }

    // Update is called once per frame
    void Update() { }
}
