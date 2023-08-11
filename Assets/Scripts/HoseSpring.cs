using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class HoseSpring : MonoBehaviour
{
    public ConfigurableJoint configurableJoint;
    public PlayerController playerController;
    private Vector3 initAnchorPos;
    private Coroutine OnHoseBibPress;

    void OnMouseDown()
    {
        //configurableJoint.connectedAnchor = Input.mousePosition;
        OnHoseBibPress = StartCoroutine(MoveAnchor());
    }

    IEnumerator MoveAnchor()
    {
        while (true)
        {
            configurableJoint.connectedAnchor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            yield return null;
        }
    }

    void OnMouseUp()
    {
        StopCoroutine(MoveAnchor());
        configurableJoint.connectedAnchor = initAnchorPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        initAnchorPos = configurableJoint.connectedAnchor;
    }

    // Update is called once per frame
    void Update()
    {
        // if (playerController.OperableObject != null)
        // {
        //     Debug.Log(playerController.OperableObject.tag);
        // }
        // else
        // {
        //     Debug.Log($"Not an operable object");
        // }
    }
}
