using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class testCube : MonoBehaviour
{
    public UiClickFilter uiClickFilter;
    PlayerInputAction playerInput;
    Coroutine delayFilterResults;

    void Awake()
    {
        playerInput = new PlayerInputAction();
        playerInput.DualMap.Touch0Contact.started += Touch0Contact_started;
        playerInput.DualMap.Touch0Contact.canceled += Touch0Contact_canceled;
        playerInput.DualMap.Touch0Contact.performed += Touch0Contact_performed;
    }
    void OnEnable()
    {
        playerInput.Enable();
    }
    void OnDisable()
    {
        playerInput.Disable();
    }

    private void Touch0Contact_performed(InputAction.CallbackContext context)
    {

    }

    private void Touch0Contact_canceled(InputAction.CallbackContext context)
    {
        // throw new NotImplementedException();
    }

    private void Touch0Contact_started(InputAction.CallbackContext context)
    {
        DetectObjectWithRaycast();
        // throw new NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void DetectObjectWithRaycast()
    {


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;


        Physics.Raycast(ray, out hit, Mathf.Infinity);

        delayFilterResults = StartCoroutine(Delay());

        if (hit.collider != null)
        {

            Debug.Log($"hit.collider: {hit.collider.name}");

        }
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.01f);
        if (uiClickFilter.isUiClicked == true)
        {
            Debug.Log($"ui clicked");
        }
        else
        {
            Debug.Log($"ui NOT clicked");
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
