using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckValveCollision : MonoBehaviour
{
    public bool isCheckClosed;

    void OnCollisionEnter(Collision other)
    {
        isCheckClosed = true;
        Actions.onCheck1Closed?.Invoke(this.gameObject);
    }

    void OnCollisionExit(Collision other)
    {
        isCheckClosed = false;
        Actions.onCheck1Opened?.Invoke(this.gameObject);
    }

    void Start() { }
}
