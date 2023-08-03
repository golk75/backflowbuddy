using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoseDetector : MonoBehaviour
{
    public static event Action<GameObject> onHoseAttach;

    public void OnTriggerEnter(Collider other)
    {
        //onHoseBibEnter.Invoke();
        onHoseAttach?.Invoke(this.gameObject);
    }

    private void OnTriggerStay() { }

    private void OnTriggerExit() { }
}
