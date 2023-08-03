using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.zibra.liquid.Manipulators;

public class ShutOffValveController : MonoBehaviour
{
    private PlayerController playerController;
    OperableComponentDescription operableComponentDescription;

    [SerializeField]
    GameObject playerManager;

    [SerializeField]
    public GameObject ShutOffValve1;

    [SerializeField]
    ZibraLiquidCollider supplyCollider;
    Vector3 supplyColliderPos;

    [SerializeField]
    GameObject ShutOffValve2;

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
                        ShutOffValve1.transform.eulerAngles =
                            playerController.OperableObjectRotation;
                    }
                    shutOffValveScaleFactor = (playerController.OperableObjectRotation.z * 0.1f);

                    volume = Mathf.Lerp(
                        supplyVolume,
                        0,
                        ShutOffValve1.transform.eulerAngles.z / 90f
                    );

                    mainSupplyEmitter.VolumePerSimTime = Mathf.SmoothStep(
                        mainSupplyEmitter.VolumePerSimTime,
                        volume,
                        1f
                    );

                    if (ShutOffValve1.transform.rotation.eulerAngles.z == 90)
                    {
                        _isSupplyOn = false;
                    }
                    else
                    {
                        _isSupplyOn = true;
                    }
                }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShutOffValveOperationCheck();
    }
}
