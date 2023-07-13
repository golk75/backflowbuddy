using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    PlayerInputAction playerInput;

    private PlayerController playerController;

    [SerializeField]
    GameObject playerManager;

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
        playerController = playerManager.GetComponent<PlayerController>();
    }

    void OnEnable()
    {
        playerInput.Enable();
        PlayerController.onZoomStop += Zoom_canceled;
        PlayerController.onPanCanceled += Pan_cancled;
    }

    void OnDisable()
    {
        playerInput.Disable();
        PlayerController.onZoomStop -= Zoom_canceled;
        PlayerController.onPanCanceled -= Pan_cancled;
    }

    private void Pan_started()
    {
        panCoroutine = StartCoroutine(PanDectection());
        //Debug.Log($"Pan started");
    }

    private void Pan_cancled()
    {
        //Debug.Log($"Pan canceled");

        StopCoroutine(PanDectection());
    }

    private void Zoom_started()
    {
        //Debug.Log($"Zoom started!");
        isZooming = true;
        zoomCoroutine = StartCoroutine(ZoomDetection());
    }

    private void Zoom_canceled()
    {
        //Debug.Log($"Zoom canceled!");
        isZooming = false;
        StopCoroutine(zoomCoroutine);
    }

    Vector3 GetPointerPos()
    {
        Vector3 screenPosition = Input.mousePosition;

        // If you're using a perspective camera for parallax,
        // be sure to assign a depth to this point.
        // screenPosition.z = 1f;

        return Camera.main.ScreenToWorldPoint(screenPosition);
    }

    /// <summary>
    /// Camera pan
    /// </summary>

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

            //placing bounderies on camera panning
            //Debug.Log($"targetPointerPos = {targetPointerPosition}");
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

    IEnumerator ZoomDetection()
    {
        float prevDistance = Vector2.Distance(
            playerInput.Touchscreen.Touch0Position.ReadValue<Vector2>(),
            playerInput.Touchscreen.Touch1Position.ReadValue<Vector2>()
        );

        float distance = 0f;
        float distanceDamp = 0.5f;
        while (playerController.secondaryTouchStarted == true)
        {
            //Debug.Log($"Zooming");
            distance = Vector2.Distance(
                playerInput.Touchscreen.Touch0Position.ReadValue<Vector2>(),
                playerInput.Touchscreen.Touch1Position.ReadValue<Vector2>()
            );

            //zoom in
            if (distance > prevDistance)
            {
                Camera.main.orthographicSize = Mathf.Lerp(
                    Camera.main.orthographicSize,
                    -distance * distanceDamp,
                    zoomingSpeed
                );
                //Camera.main.orthographicSize -= distance * zoomingSpeed;
                if (Camera.main.orthographicSize <= maxZoom)
                {
                    Camera.main.orthographicSize = maxZoom;
                }
            }
            //zoom out
            else if (prevDistance > distance)
            {
                Camera.main.orthographicSize = Mathf.Lerp(
                    Camera.main.orthographicSize,
                    distance * distanceDamp,
                    zoomingSpeed
                );
                //Camera.main.orthographicSize += distance * zoomingSpeed;

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
        //Debug.Log(playerController.Touch0Position.ReadValue<Vector2>());
        CameraMovement();
    }
}
