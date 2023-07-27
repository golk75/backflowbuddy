using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckValveCollision : MonoBehaviour
{
    public bool isCheckClosed;

    void OnCollisionEnter(Collision other)
    {
        isCheckClosed = true;
    }

    void OnCollisionExit(Collision other)
    {
        isCheckClosed = false;
    }

    void Start() { }
}
