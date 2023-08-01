using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    Vector3 initNeedleRot;
    Vector3 endNeedleRot = new Vector3(0, 0, 255);
    Vector3 currentNeedleRot;

    // Start is called before the first frame update
    void Start()
    {
        initNeedleRot = needle.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.OperableObjectRotation != null)
        {
            currentNeedleRot = Vector3.Lerp(
                initNeedleRot,
                endNeedleRot,
                Mathf.Clamp(playerController.OperableObjectRotation.z, 0, 1)
            );
            needle.transform.eulerAngles = currentNeedleRot;
        }
    }
}
