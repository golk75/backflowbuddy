using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class QuizThemeManager : MonoBehaviour
{
    public StyleSheet landscapeTheme;
    public StyleSheet portraitTheme;
    public UIDocument uIDocument;
    private VisualElement root;
    // Start is called before the first frame update
    void Start()
    {

        root = uIDocument.rootVisualElement;



    }
    void Update()
    {
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
