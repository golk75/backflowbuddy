using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class HoseManager : MonoBehaviour
{
    [SerializeField]
    private Rigidbody[] hingeJointConnections;

    [SerializeField]
    private GameObject hoseArmature;
    Rigidbody connectedAbove;
    Rigidbody connectedBelow;

    // HingeJoint connectedAbove;
    // HingeJoint connectedBelow;

    // Start is called before the first frame update
    void Awake()
    {
        hingeJointConnections = hoseArmature.GetComponentsInChildren<Rigidbody>();
        for (int i = 1; i < hingeJointConnections.Length; i++)
        {
            hingeJointConnections[i].GetComponent<FixedJoint>().connectedBody =
                hingeJointConnections[i - 1];
            // hingeJointConnections[i].GetComponent<ConfigurableJoint>().connectedBody =
            //     hingeJointConnections[i - 1];

            // hingeJointConnections[i].GetComponent<HingeJoint>().connectedBody =
            //     hingeJointConnections[i - 1];
        }
    }

    // Update is called once per frame
    void Update() { }
}
