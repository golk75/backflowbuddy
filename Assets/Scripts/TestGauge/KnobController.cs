using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KnobController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"pointer down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"pointer released");
    }

    public void EventClicker()
    {
        Debug.Log($"Event clicker clicked");
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
