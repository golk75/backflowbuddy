using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoseDetector : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        //onHoseBibEnter.Invoke();
        Actions.onHoseAttach?.Invoke(this.gameObject);
    }

    private void OnTriggerStay() { }

    private void OnTriggerExit()
    {
        Actions.onHoseDetach?.Invoke(this.gameObject);
    }
}
