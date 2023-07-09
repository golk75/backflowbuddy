using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    PlayerInputAction playerInput;
    PlayerController playerController;
    private Coroutine zoomCoroutine;
    private Coroutine panCoroutine;

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
    float panningSpeed;

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
        PlayerController.onZoomStart += Zoom_started;
        PlayerController.onZoomStop += Zoom_canceled;
        PlayerController.onPanStart += Pan_started;
        PlayerController.onPanCanceled += Pan_cancled;
    }

    void OnDisable()
    {
        playerInput.Disable();
        PlayerController.onZoomStart -= Zoom_started;
        PlayerController.onZoomStop -= Zoom_canceled;
        PlayerController.onPanStart -= Pan_started;
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
        Vector3 difference =
            playerController.touchStart
            - Camera.main.ScreenToWorldPoint(
                playerInput.Touchscreen.Touch0Position.ReadValue<Vector2>()
            );
        while (isPanning == true)
        {
            Debug.Log($"PANNNNNNNN");
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
            yield return null;
        }
    }

    /// <summary>
    /// Camera zoom
    /// </summary>

    private void Pan_started()
    {
        Debug.Log($"Pan started");
        isPanning = true;
        panCoroutine = StartCoroutine(PanDectection());
    }

    private void Pan_cancled()
    {
        Debug.Log($"Pan canceled");
        isPanning = false;
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

    IEnumerator ZoomDetection()
    {
        float prevDistance = Vector2.Distance(
            playerInput.Touchscreen.Touch0Position.ReadValue<Vector2>(),
            playerInput.Touchscreen.Touch1Position.ReadValue<Vector2>()
        );

        float distance = 0f;

        while (isZooming == true)
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
            prevDistance = distance;

            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(playerInput.Touchscreen.Touch0Position.ReadValue<Vector2>());
    }
}
