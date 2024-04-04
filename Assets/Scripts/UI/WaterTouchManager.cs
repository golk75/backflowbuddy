

using System;
using com.zibra.liquid.Manipulators;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;



public class WaterTouchManager : MonoBehaviour
{
    private PlayerInputAction playerInput;
    private float primaryPointerPressed;
    private Vector3 touchStart;
    private Vector3 fingerColliderInitPos;
    public ZibraLiquidCollider m_FingerCollider;
    public GameObject m_Floor;
    public ZibraLiquidForceField m_ResultsForceField;
    public Vector2 touchDelta;

    void OnEnable()
    {
        playerInput.DualMap.Enable();

    }
    void OnDisable()
    {
        playerInput.DualMap.Disable();
    }
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInputAction();
        Actions.onResultsReveal += RevealResults;
        RegisterCallbacks();
        fingerColliderInitPos = m_FingerCollider.transform.position;
    }


    private void RegisterCallbacks()
    {
        playerInput.DualMap.Touch0Contact.started += TouchStarted;
        playerInput.DualMap.Touch0Contact.canceled += TouchCanceled;
        playerInput.DualMap.Touch0Position.started += TouchPosition;
        playerInput.DualMap.Touch0Delta.started += TouchDelta;
        playerInput.DualMap.Touch0Delta.canceled += TouchDeltaCanceled;

    }


    private void RevealResults()
    {
        // m_ResultsForceField.enabled = true;
        // m_ResultsForceField.Strength = DOTween.
        DOTween.To(x => m_ResultsForceField.Strength = x, 0, 4, 5f);
    }


    private void TouchDeltaCanceled(InputAction.CallbackContext context)
    {

    }

    private void TouchDelta(InputAction.CallbackContext context)
    {
        var deltaStart = context.ReadValue<Vector2>();
        // touchDelta = new Vector2(touchStart.x - deltaStart.x, touchStart.y - deltaStart.y);
    }

    private void TouchPosition(InputAction.CallbackContext context)
    {

    }
    // private void Touch0Delta_started(InputAction.CallbackContext context)
    // {
    //     primaryFingerDelta = new Vector2(
    //         touchStart.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
    //         touchStart.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y
    //     );
    // }
    private void TouchStarted(InputAction.CallbackContext context)
    {
        primaryPointerPressed = context.ReadValue<float>();
        touchStart = Camera.main.ScreenToWorldPoint(
      //  playerInput.MouseOperate.MousePosition.ReadValue<Vector2>()
      playerInput.DualMap.Touch0Position.ReadValue<Vector2>()

      //playerInput.DualMap.Touch0Position.ReadValue<Vector2>()
      );
        // RaycastHit hit;
        // var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // if (Physics.Raycast(ray, out hit) && hit.collider == m_Floor.GetComponent<Collider>())
        // {
        //     // touchStart = Camera.main.ScreenToWorldPoint(hit.point);
        //     touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition - hit.point);
        // }

    }

    private void TouchCanceled(InputAction.CallbackContext context)
    {
        primaryPointerPressed = context.ReadValue<float>();
        touchStart = Vector3.zero;
        touchDelta = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (primaryPointerPressed > 0)
        {
            //must use orthographic camera for this movement to work
            var direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_FingerCollider.transform.position = new Vector3(direction.x, direction.y, m_FingerCollider.transform.position.z);

        }
        else
        {
            m_FingerCollider.transform.position = fingerColliderInitPos;
        }
    }
}
