using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HoseBib : MonoBehaviour
{
    public GameObject testCock;

    void OnTriggerStay(Collider other)
    {
        testCock = other.gameObject;

    }
}
