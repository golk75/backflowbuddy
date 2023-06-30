using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDefines : MonoBehaviour
{
    [SerializeField]
    AssemblyController assemblyController;

    [SerializeField]
    AssemblyController_Touch assemblyTouchController;

    void Start()
    {
        assemblyTouchController = GetComponent<AssemblyController_Touch>();
        assemblyController = GetComponent<AssemblyController>();
#if UNITY_EDITOR
        //Debug.Log("Unity Editor");


#endif

#if UNITY_IOS
        //Debug.Log("iOS");
        assemblyTouchController.enabled = true;
        assemblyController.enabled = false;

#endif

#if UNITY_STANDALONE_OSX
        Debug.Log("Standalone OSX");
#endif

#if UNITY_STANDALONE_WIN
        Debug.Log("Standalone Windows");
#endif
    }
}
