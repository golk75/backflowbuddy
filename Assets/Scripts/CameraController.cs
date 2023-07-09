using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    PlayerInputAction playerInput;
    PlayerController playerController;
    public static Coroutine zoomCoroutine;

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
    }

    void OnDisable()
    {
        playerInput.Disable();
    }

    /// <summary>
    /// Camera pan
    /// </summary>
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

    /// <summary>
    /// Camera zoom
    /// </summary>


    private void Zoom_started()
    {
        Debug.Log($"Zoom started!");

        zoomCoroutine = StartCoroutine(ZoomDetection());

        //isZooming = true;
    }

    private void Zoom_canceled()
    {
        Debug.Log($"Zoom canceled!");
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

        while (true)
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
