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


    VisualElement m_GameMenuScreenPanel;
    // Start is called before the first frame update
    void Start()
    {

        root = uIDocument.rootVisualElement;
        m_GameMenuScreenPanel = root.Q("GameMenuScreen_panel");



    }
    void Update()
    {
#if UNITY_IOS || UNITY_ANDROID

        if (Screen.orientation == ScreenOrientation.Portrait)
        {

            root.styleSheets.Remove(landscapeTheme);
            root.styleSheets.Add(portraitTheme);
            m_GameMenuScreenPanel.style.scale = new StyleScale(new Vector2(3, 3));


        }
        else
        {
            root.styleSheets.Remove(portraitTheme);
            root.styleSheets.Add(landscapeTheme);
            m_GameMenuScreenPanel.style.scale = new StyleScale(new Vector2(1, 1));

        }
#endif
#if UNITY_EDITOR_OSX
                root.styleSheets.Remove(portraitTheme);
                root.styleSheets.Remove(landscapeTheme);
                root.styleSheets.Add(desktopTheme);

#endif
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
                root.styleSheets.Remove(portraitTheme);
                root.styleSheets.Remove(landscapeTheme);
                root.styleSheets.Add(desktopTheme);

#endif

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
