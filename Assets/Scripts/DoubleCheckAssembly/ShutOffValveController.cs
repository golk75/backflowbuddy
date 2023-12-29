using System.Collections;
using System.Collections.Generic;
using com.zibra.liquid;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.Solver;
using Unity.Mathematics;
using UnityEngine;

public class ShutOffValveController : MonoBehaviour
{
    private PlayerController playerController;
    OperableComponentDescription operableComponentDescription;
    public ZibraLiquid liquid;
    [SerializeField]
    GameObject playerManager;


    public GameObject ShutOffValve1;
    public GameObject ShutOffValve2;

    [SerializeField]
    ZibraLiquidCollider supplyCollider;
    Vector3 supplyColliderPos;



    [SerializeField]
    ZibraLiquidDetector zone1Detector;

    [SerializeField]
    ZibraLiquidForceField supplyFF;

    public float supplyVolume;

    [SerializeField]
    float supplyVelocity;

    public ZibraLiquidEmitter mainSupplyEmitter;

    public float volume;

    float shutOffValveScaleFactor;

    Vector3 VelocityRef = new Vector3(0, 10, 0);

    private bool _isSupplyOn;

    public bool IsSupplyOn
    {
        get { return _isSupplyOn; }
        set { value = _isSupplyOn; }
    }

    private bool _isSecondShutOffOpen;

    public bool IsSecondShutOffOpen
    {
        get { return _isSecondShutOffOpen; }
        set { value = _isSecondShutOffOpen; }
    }
    // Start is called before the first frame update
    void Start()
    {
        playerController = playerManager.GetComponent<PlayerController>();
    }

    private void ShutOffValveOperationCheck()
    {
        if (playerController.OperableObject != null)
        {
            if (
                playerController.OperableObject.TryGetComponent<OperableComponentDescription>(
                    out OperableComponentDescription component
                )
            )

                if (
                    playerController.operableComponentDescription.partsType
                    == OperableComponentDescription.PartsType.ShutOff
                )
                {
                    operableComponentDescription = playerController.operableComponentDescription;
                    if (
                        operableComponentDescription.componentId
                        == OperableComponentDescription.ComponentId.ShutOffValve1
                    )
                    {
                        mainSupplyEmitter.enabled = true;
                        if (!liquid.Initialized)
                        {

                            liquid.InitialState = ZibraLiquid.InitialStateType.NoParticles;
                            liquid.InitializeSimulation();
                            if (liquid.enabled != true)
                            {

                                liquid.enabled = true;
                            }
                        }
                        ShutOffValve1.transform.eulerAngles = playerController._operableObjectRotation;
                    }
                    else if (
                        operableComponentDescription.componentId
                        == OperableComponentDescription.ComponentId.ShutOffValve2
                    )
                    {
                        ShutOffValve2.transform.eulerAngles =
                            playerController.OperableObjectRotation;
                    }

                }
        }

        if (ShutOffValve1.transform.rotation.eulerAngles.z == 90)
        {
            _isSupplyOn = false;
        }
        else
        {
            _isSupplyOn = true;
        }
        if (ShutOffValve2.transform.rotation.eulerAngles.z == 90)
        {
            _isSecondShutOffOpen = false;
        }
        else
        {
            _isSecondShutOffOpen = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShutOffValveOperationCheck();

    }
}
