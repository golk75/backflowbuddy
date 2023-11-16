using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class HoseJointConfig : MonoBehaviour
{
    [SerializeField]
    private Rigidbody[] hingeJointConnections;

    [SerializeField]
    private GameObject hoseArmature;
    Rigidbody connectedAbove;
    Rigidbody connectedBelow;



    // Start is called before the first frame update
    void Awake()
    {
        //create hinge joints for hose armature
        hingeJointConnections = hoseArmature.GetComponentsInChildren<Rigidbody>();
        for (int i = 1; i < hingeJointConnections.Length; i++)
        {
            hingeJointConnections[i].GetComponent<FixedJoint>().connectedBody =
                hingeJointConnections[i - 1];

        }
    }


}
