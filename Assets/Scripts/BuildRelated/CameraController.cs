using System;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    PlayerInputAction playerInput;

    private PlayerController playerController;


    [SerializeField]
    GameObject playerManager;
    public UiClickFilter uiClickFilter;
    private Coroutine zoomCoroutine;
    private Coroutine panCoroutine;

    [SerializeField]
    Camera mainCam;

    public bool isZooming;
    public bool isPanning;

    [SerializeField]
    float maxZoom;

    [SerializeField]
    float minZoom;

    [Range(0, 0.01f)]
    [SerializeField]
    float zoomingSpeed = 0.001f;

    [SerializeField]
    float panningSpeed = 0.01f;


    private float panningLeftBoundry = -30.4f;


    private float panningRightBoundry = 3f;


    private float panningTopBoundry = 2.48f;


    private float panningBottomBoundry = -1.9f;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInputAction();
    }

    void Start()
    {
        playerController = playerManager.GetComponent<PlayerController>();
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
    }

    void OnEnable()
    {
        playerInput.Enable();
        PlayerController.onZoomStop += Zoom_canceled;
        PlayerController.OnPanCanceled += Pan_cancled;


        //orientation lock
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
    }

    void OnDisable()
    {
        playerInput.Disable();
        PlayerController.onZoomStop -= Zoom_canceled;
        PlayerController.OnPanCanceled -= Pan_cancled;
        //realease orientation lock
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = false;
    }

    private void Pan_started()
    {




        panCoroutine = StartCoroutine(PanDectection());
        isPanning = true;


        //Debug.Log($"Pan started");
    }

    private void Pan_cancled()
    {
        //Debug.Log($"Pan canceled");

        StopCoroutine(PanDectection());
        isPanning = false;
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
            playerController.primaryClickStarted > 0
            && playerController.secondaryTouchStarted == false
            && uiClickFilter.isUiClicked == false
        )
        {

            Vector3 currentPointerPos = GetPointerPos();
            Vector3 targetPointerPosition = currentPointerPos - pointerOrigin;



            //placing bounderies on camera panning
            //Debug.Log($"targetPointerPos = {targetPointerPosition}");

            Camera.main.transform.localPosition += -targetPointerPosition * panningSpeed;
            if (Camera.main.transform.localPosition.x < panningLeftBoundry)
            {
                Camera.main.transform.localPosition = new Vector3(
                    panningLeftBoundry,
                    Camera.main.transform.localPosition.y,
                    Camera.main.transform.localPosition.z
                );
            }
            else if (Camera.main.transform.localPosition.x > panningRightBoundry)
            {
                Camera.main.transform.localPosition = new Vector3(
                    panningRightBoundry,
                    Camera.main.transform.localPosition.y,
                    Camera.main.transform.localPosition.z
                );
            }
            if (Camera.main.transform.localPosition.y > panningTopBoundry)
            {
                Camera.main.transform.localPosition = new Vector3(
                    Camera.main.transform.localPosition.x,
                    panningTopBoundry,
                    Camera.main.transform.localPosition.z
                );
            }
            else if (Camera.main.transform.localPosition.y < panningBottomBoundry)
            {
                Camera.main.transform.localPosition = new Vector3(
                    Camera.main.transform.localPosition.x,
                    panningBottomBoundry,
                    Camera.main.transform.localPosition.z
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
        if (
            playerController.isOperableObject == false
            && uiClickFilter.isUiClicked == false
        )
        {
            if (
                playerController.secondaryTouchStarted == false
                && playerController.primaryClickStarted > 0
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
