using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class TestKitOperableCheck : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public TestKitController testKitController;

    // Start is called before the first frame update
    void Start() { }

    public void DetectOperation()
    {
        Actions.OnTestKitOperate(this);
    }

    // Update is called once per frame
    void Update() { }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (testKitController.isOperableObject == false)
            Actions.OnTestKitOperate(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        testKitController.isOperableObject = true;
    }
}
