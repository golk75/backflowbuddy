using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.zibra.liquid.Manipulators;

public class ShutOffValveController : MonoBehaviour
{
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

    private PlayerController playerController;

    [SerializeField]
    GameObject playerManager;

    public ZibraLiquidEmitter mainSupplyEmitter;

    public float volume;

    float shutOffValveScaleFactor;

    Vector3 VelocityRef = Vector3.zero;

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
        supplyColliderPos = supplyCollider.transform.position;
    }

    private void DeviceOperationCheck()
    {
        shutOffValveScaleFactor = (playerController.OperableObjectRotation.z * 0.1f);
        //Debug.Log(playerController.OperableObject);
        //Debug.Log(playerController.OperableObjectRotation.z / 90);
        if (playerController.OperableObject == ShutOffValve1)
        {
            //mainSupplyEmitter.VolumePerSimTime = Mathf.SmoothDamp();
            volume = Mathf.Lerp(supplyVolume, 0, playerController.OperableObjectRotation.z / 90);

            //supplyFF.Strength = Mathf.SmoothDamp(4, 0, ref VelocityRef.y, 1);
        }
        mainSupplyEmitter.VolumePerSimTime = volume;

        //mainSupplyEmitter.InitialVelocity.y = Mathf.SmoothDamp(0, 2, ref VelocityRef.y, 0.05f);
        /*
        mainSupplyEmitter.VolumePerSimTime = Mathf.SmoothDamp(
            mainSupplyEmitter.VolumePerSimTime,
            volume,
            ref VelocityRef.y,
            5f
        );
        

        /*
    // tie test cock emitter volumes to Assembly volume filled (using detectors), so that if the supply is off the test cocks can not output and visa versa
    if (supply.VolumePerSimTime > 0)
    {
        IsSupplyOn = true;
    }
    else if (supply.VolumePerSimTime <= 0)
    {
        IsSupplyOn = false;
    }
}
*/
        if (mainSupplyEmitter.VolumePerSimTime > 0)
        {
            _isSupplyOn = true;
        }
        else
        {
            _isSupplyOn = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        DeviceOperationCheck();
    }
}
