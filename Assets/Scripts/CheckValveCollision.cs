using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckValveCollision : MonoBehaviour
{
    public bool isCheckClosed;

    void OnCollisionEnter(Collision other)
    {
        isCheckClosed = true;
        Actions.onCheckClosed?.Invoke(this.gameObject);
    }

    void OnCollisionExit(Collision other)
    {
        isCheckClosed = false;
        Actions.onCheckOpened?.Invoke(this.gameObject);
    }

    void Start() { }
}
