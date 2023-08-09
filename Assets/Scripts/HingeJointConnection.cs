using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HingeJointConnection : MonoBehaviour
{
    GameObject connectedAbove,
        connectedBelow;

    // Start is called before the first frame update
    void Start()
    {
        // connectedAbove = GetComponent<HingeJoint>().connectedBody.gameObject;
        // HingeJointConnection aboveSegment = connectedAbove.GetComponent<HingeJointConnection>();
        // if (connectedAbove != null)
        // {
        //     aboveSegment.connectedBelow = gameObject;
        // }
    }

    // Update is called once per frame
    void Update() { }
}
