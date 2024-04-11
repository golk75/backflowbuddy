using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraController : MonoBehaviour
{
    Camera m_Camera;
    // Start is called before the first frame update
    void Start()
    {
        // m_Camera = GetComponent<Camera>();
        // if (Screen.orientation == ScreenOrientation.Portrait)
        // {
        //     m_Camera.orthographicSize = 4.75f;
        // }
        // else
        // {
        //     m_Camera.orthographicSize = 2.1f;

        // }
        m_Camera = GetComponent<Camera>();
#if UNITY_IOS || UNITY_ANDROID

        if (Screen.orientation == ScreenOrientation.Portrait)
        {
            m_Camera.orthographicSize = 4.75f;
        }
        else
        {
            m_Camera.orthographicSize = 2.1f;

        }
        Debug.Log($"mobile");
#endif
#if UNITY_EDITOR_OSX
        Debug.Log($"editor osx");


        m_Camera.orthographicSize = 2.1f;


#endif

        // if (Application.platform == RuntimePlatform.Android)
        // {

        // }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_IOS || UNITY_ANDROID
        m_Camera = GetComponent<Camera>();
        if (Screen.orientation == ScreenOrientation.Portrait)
        {
            m_Camera.orthographicSize = 4.75f;
        }
        else
        {
            m_Camera.orthographicSize = 2.1f;

        }
#endif
        // if (Screen.orientation == ScreenOrientation.Portrait)
        // {
        //     m_Camera.orthographicSize = 4.75f;
        // }
        // else
        // {
        //     m_Camera.orthographicSize = 2.1f;

        // }
    }
}
