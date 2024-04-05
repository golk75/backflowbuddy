using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraController : MonoBehaviour
{
    Camera m_Camera;
    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Screen.orientation == ScreenOrientation.Portrait)
        {
            m_Camera.orthographicSize = 4.75f;
        }
        else
        {
            m_Camera.orthographicSize = 2.1f;

        }
    }
}
