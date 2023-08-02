using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoseDetector : MonoBehaviour
{
    public UnityEvent<Collider> onHoseBibEnter;
    public UnityEvent<Collider> onHoseBibStay;
    public UnityEvent<Collider> onHoseBibExit;

    private void OnTriggerEnter(Collider other)
    {
        onHoseBibEnter?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        onHoseBibStay?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        onHoseBibExit?.Invoke(other);
    }
}
