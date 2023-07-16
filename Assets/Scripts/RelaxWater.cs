using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.zibra.liquid.SDFObjects;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.DataStructures;
using com.zibra.liquid.Analytics;

public class RelaxWater : MonoBehaviour
{
    [SerializeField]
    ZibraLiquidEmitter supplyEmitter;

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
    ZibraLiquidForceField supplyFF;

    [SerializeField]
    ZibraLiquidVoid supplyVoid;

    float initSupplyVolume;

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

    [SerializeField]
    ShutOffValveController shutOffValveController;

    [SerializeField]
    ZibraLiquidCollider supplyCollider;

    SDFObject supplyVoidSDF;

    float skinSurface;

    Vector3 supplyColliderClosedPos = new Vector3(-16, -0.06f, 0.03f);
    Vector3 initSupplyColliderPos;
    Vector3 initSupplyVoidScale;
    Vector3 currentSupplyVoidScale;
    Vector3 targetSupplyVoidScale;
    Vector3 supplyVoidRef = Vector3.zero;

    [Range(0, 0.00009f)]
    public float supplyVoidSurfaceDepthLerpFactor;

    PlayerController playerController;
    public GameObject playerManager;

    public float supplyVolume;

    float currentVelocity;

    private void checkRelax()
    {
        //Debug.Log((zone2Detector.ParticlesInside + zone2Detector.ParticlesInside) / 150000);
        //Debug.Log(zone1Detector.ParticlesInside + zone2Detector.ParticlesInside);
        supplyVolume = shutOffValveController.mainSupplyEmitter.VolumePerSimTime;
        //150,000

        if (
            zone1Detector.ParticlesInside + zone2Detector.ParticlesInside > 10000
            && zone1Detector.ParticlesInside + zone2Detector.ParticlesInside < 120000
        )
        {
            //supplyVoidSDF.SurfaceDistance = Mathf.SmoothDamp(0, -1, ref supplyVoidRef.x, 0.1f);
            /*
            targetSupplyVoidScale.y = Mathf.Lerp(
                initSupplyVoidScale.y,
                initSupplyVoidScale.y + 2,
                zone1Detector.ParticlesInside + zone2Detector.ParticlesInside
            );

            supplyVoid.transform.localScale = new Vector3(
                supplyVoid.transform.localScale.x,
                targetSupplyVoidScale.y,
                supplyVoid.transform.localScale.z
            );
            */
            /*
            supplyVoidSDF.SurfaceDistance = Mathf.Lerp(
                0,
                -2,
                (zone1Detector.ParticlesInside + zone2Detector.ParticlesInside / 2)
                    * supplyVoidSurfaceDepthLerpFactor
            );
            */
            supplyVoidSDF.SurfaceDistance = Mathf.MoveTowards(
                supplyVoidSDF.SurfaceDistance,
                -1,
                0.01f
            );
        }
        else if (zone3Detector.ParticlesInside > 20000 && zone3Detector.ParticlesInside < 60000)
        {
            supplyVoidSDF.SurfaceDistance = Mathf.MoveTowards(
                supplyVoidSDF.SurfaceDistance,
                -1,
                0.01f
            );
            /*
            supplyVoidSDF.SurfaceDistance = Mathf.Lerp(
                -2,
                0,
                (zone1Detector.ParticlesInside + zone2Detector.ParticlesInside / 2)
                    * supplyVoidSurfaceDepthLerpFactor
            );
            */
        }
        else if (zone3Detector.ParticlesInside > 60000)
        {
            //supplyVoidSDF.SurfaceDistance = 0;
            supplyVoidSDF.SurfaceDistance = Mathf.MoveTowards(
                supplyVoidSDF.SurfaceDistance,
                0,
                0.001f
            );
        }
        //supplyVoidSDF.SurfaceDistance = Mathf.SmoothDamp(supplyVoidSDF.SurfaceDistance,0,ref supplyVoidRef.x,0.1f);


        //zone1Detector+zone2Detector


        if (shutOffValveController.IsSupplyOn == false)
        {
            //relax water in check housing
            checkValve1ForceField.enabled = false;
            checkValve2ForceField.enabled = false;
            supplyVoidSDF.SurfaceDistance = 0;
        }
        else
        {
            checkValve1ForceField.enabled = true;
            checkValve2ForceField.enabled = true;
        }

        //close supply end with collider if shutoff is closed, to keep current volume of water at time of shutoff (protect water from supply void)
        if (supplyVolume <= 0 && playerController.isInit == true)
        {
            supplyCollider.transform.position = supplyColliderClosedPos;
        }
        else if (supplyVolume > 0 && playerController.isInit == true)
        {
            supplyCollider.transform.position = initSupplyColliderPos;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        supplyVoidSDF = supplyVoid.GetComponent<SDFObject>();
        playerController = playerManager.GetComponent<PlayerController>();
        initSupplyColliderPos = supplyCollider.transform.position;
        initSupplyVolume = shutOffValveController.supplyVolume;
        initSupplyVoidScale = supplyVoid.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        checkRelax();
    }
}
