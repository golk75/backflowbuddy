using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckValve2Collision : MonoBehaviour
{
    public bool isCheckClosed;

    void OnCollisionEnter(Collision other)
    {
        isCheckClosed = true;
        Actions.onCheck2Closed?.Invoke(this.gameObject);
    }

    void OnCollisionExit(Collision other)
    {
        isCheckClosed = false;
        Actions.onCheck2Opened?.Invoke(this.gameObject);
    }

    void Start() { }
}
