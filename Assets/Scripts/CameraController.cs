using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    PlayerInputAction playerInput;
    PlayerController playerController;
    private Coroutine zoomCoroutine;
    private Coroutine panCoroutine;

    [SerializeField]
    Camera mainCam;

    private bool isZooming;
    private bool isPanning;

    [SerializeField]
    float maxZoom;

    [SerializeField]
    float minZoom;

    [Range(0, 0.01f)]
    [SerializeField]
    float zoomingSpeed = 0.001f;

    [SerializeField]
    float panningSpeed = 0.01f;

    [SerializeField]
    float panningLeftBoundry = -15.0f;

    [SerializeField]
    float panningRightBoundry = 23.0f;

    [SerializeField]
    float panningTopBoundry = 4.0f;

    [SerializeField]
    float panningBottomBoundry = 0.1f;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInputAction();
    }

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void OnEnable()
    {
        playerInput.Enable();
        // PlayerController.onZoomStart += Zoom_started;
        PlayerController.onZoomStop += Zoom_canceled;
        //PlayerController.onPanStart += Pan_started;
        PlayerController.onPanCanceled += Pan_cancled;
    }

    void OnDisable()
    {
        playerInput.Disable();
        //PlayerController.onZoomStart -= Zoom_started;
        PlayerController.onZoomStop -= Zoom_canceled;
        // PlayerController.onPanStart -= Pan_started;
        PlayerController.onPanCanceled -= Pan_cancled;
    }

    /// <summary>
    /// Camera pan
    /// </summary>

    /*
    private void CameraPanning()
    {
        Vector3 difference =
            playerController.touchStart
            - Camera.main.ScreenToWorldPoint(
                playerInput.Touchscreen.Touch0Position.ReadValue<Vector2>()
            );
        Camera.main.transform.position += difference;
        //placing bouneries on camera panning
        if (Camera.main.transform.position.x < panningLeftBoundry)
        {
            Camera.main.transform.position = new Vector3(
                panningLeftBoundry,
                Camera.main.transform.position.y,
                Camera.main.transform.position.z
            );
        }
        else if (Camera.main.transform.position.x > panningRightBoundry)
        {
            Camera.main.transform.position = new Vector3(
                panningRightBoundry,
                Camera.main.transform.position.y,
                Camera.main.transform.position.z
            );
        }
        if (Camera.main.transform.position.y > panningTopBoundry)
        {
            Camera.main.transform.position = new Vector3(
                Camera.main.transform.position.x,
                panningTopBoundry,
                Camera.main.transform.position.z
            );
        }
        else if (Camera.main.transform.position.y < panningBottomBoundry)
        {
            Camera.main.transform.position = new Vector3(
                Camera.main.transform.position.x,
                panningBottomBoundry,
                Camera.main.transform.position.z
            );
        }
    }
*/
    IEnumerator PanDectection()
    {
        Vector3 pointerOrigin = GetPointerPos();

        while (
            playerController.primaryTouchStarted == true
            && playerController.secondaryTouchStarted == false
        )
        {
            Vector3 currentPointerPos = GetPointerPos();
            Vector3 targetPointerPosition = currentPointerPos - pointerOrigin;

            /*
            currentPos = Camera.main.ScreenToViewportPoint(
                playerInput.Touchscreen.Touch0Position.ReadValue<Vector2>()
            );
            */
            //  Debug.Log($"PANNNNNNNN--> {Camera.main.transform.position}");

            //placing bouneries on camera panning
            Debug.Log($"targetPointerPos = {targetPointerPosition}");
            Camera.main.transform.position += -targetPointerPosition * panningSpeed;
            if (Camera.main.transform.position.x < panningLeftBoundry)
            {
                Camera.main.transform.position = new Vector3(
                    panningLeftBoundry,
                    Camera.main.transform.position.y,
                    Camera.main.transform.position.z
                );
            }
            else if (Camera.main.transform.position.x > panningRightBoundry)
            {
                Camera.main.transform.position = new Vector3(
                    panningRightBoundry,
                    Camera.main.transform.position.y,
                    Camera.main.transform.position.z
                );
            }
            if (Camera.main.transform.position.y > panningTopBoundry)
            {
                Camera.main.transform.position = new Vector3(
                    Camera.main.transform.position.x,
                    panningTopBoundry,
                    Camera.main.transform.position.z
                );
            }
            else if (Camera.main.transform.position.y < panningBottomBoundry)
            {
                Camera.main.transform.position = new Vector3(
                    Camera.main.transform.position.x,
                    panningBottomBoundry,
                    Camera.main.transform.position.z
                );
            }

            yield return null;
        }
    }

    /// <summary>
    /// Camera zoom
    /// </summary>


    private void Pan_started()
    {
        panCoroutine = StartCoroutine(PanDectection());
        Debug.Log($"Pan started");
    }

    private void Pan_cancled()
    {
        Debug.Log($"Pan canceled");

        StopCoroutine(PanDectection());
    }

    private void Zoom_started()
    {
        Debug.Log($"Zoom started!");
        isZooming = true;
        zoomCoroutine = StartCoroutine(ZoomDetection());
    }

    private void Zoom_canceled()
    {
        Debug.Log($"Zoom canceled!");
        isZooming = false;
        StopCoroutine(zoomCoroutine);

        /*
        if (isZooming == true)
        {
            StopCoroutine(zoomCoroutine);
            isZooming = false;
        }
        */
    }

    Vector3 GetPointerPos()
    {
        Vector3 screenPosition = Input.mousePosition;

        // If you're using a perspective camera for parallax,
        // be sure to assign a depth to this point.
        // screenPosition.z = 1f;

        return Camera.main.ScreenToWorldPoint(screenPosition);
    }

    IEnumerator ZoomDetection()
    {
        float prevDistance = Vector2.Distance(
            playerInput.Touchscreen.Touch0Position.ReadValue<Vector2>(),
            playerInput.Touchscreen.Touch1Position.ReadValue<Vector2>()
        );

        float distance = 0f;

        while (playerController.secondaryTouchStarted == true)
        {
            Debug.Log($"ZOOOOOOOM");
            distance = Vector2.Distance(
                playerInput.Touchscreen.Touch0Position.ReadValue<Vector2>(),
                playerInput.Touchscreen.Touch1Position.ReadValue<Vector2>()
            );

            //zoom in
            if (distance > prevDistance)
            {
                //Camera.main.orthographicSize -= distance / zoomFactor;
                Camera.main.orthographicSize -= distance * zoomingSpeed;
                if (Camera.main.orthographicSize <= maxZoom)
                {
                    Camera.main.orthographicSize = maxZoom;
                }
            }
            //zoom out
            else if (prevDistance > distance)
            {
                Camera.main.orthographicSize += distance * zoomingSpeed;

                if (Camera.main.orthographicSize >= minZoom)
                {
                    Camera.main.orthographicSize = minZoom;
                }
            }
            //keeps track of previous distance for next loop


            yield return null;
        }
    }

    private void CameraMovement()
    {
        if (playerController.isOperableObject == false)
        {
            if (
                playerController.secondaryTouchStarted == false
                && playerController.primaryTouchStarted == true
            )
            {
                Pan_started();
            }
            else if (playerController.secondaryTouchStarted)
            {
                Zoom_started();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();
        //Debug.Log(playerInput.Touchscreen.Touch0Position.ReadValue<Vector2>());
    }
}
