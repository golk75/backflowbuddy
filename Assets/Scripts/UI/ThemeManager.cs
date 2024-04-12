using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class ThemeManager : MonoBehaviour
{
    public StyleSheet landscapeTheme;
    public StyleSheet portraitTheme;
    public StyleSheet desktopTheme;
    public UIDocument uIDocument;
    private VisualElement root;
    // Start is called before the first frame update
    void Start()
    {

        root = uIDocument.rootVisualElement;



    }
    void Update()
    {
#if UNITY_IOS || UNITY_ANDROID

        if (Screen.orientation == ScreenOrientation.Portrait)
        {

            root.styleSheets.Remove(landscapeTheme);
            root.styleSheets.Add(portraitTheme);


        }
        else
        {
            root.styleSheets.Remove(portraitTheme);
            root.styleSheets.Add(landscapeTheme);


        }
#endif
        // #if UNITY_EDITOR_OSX
        //         root.styleSheets.Remove(portraitTheme);
        //         root.styleSheets.Remove(landscapeTheme);
        //         root.styleSheets.Add(desktopTheme);

        // #endif

        // if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        // {

        //     root.styleSheets.Remove(portraitTheme);
        //     root.styleSheets.Add(landscapeTheme);
        //     Debug.Log($"landscape");


        // }
        // else
        // {
        //     root.styleSheets.Remove(landscapeTheme);
        //     root.styleSheets.Add(portraitTheme);
        //     Debug.Log($"portrait");
        // }


    }


}
