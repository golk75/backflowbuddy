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


        // button.clicked += () =>
        // {
        //     if (root.styleSheets.Contains(theme1))
        //     {
        //         root.styleSheets.Remove(theme1);
        //         root.styleSheets.Add(theme2);
        //     }
        //     else
        //     {
        //         root.styleSheets.Remove(theme2);
        //         root.styleSheets.Add(theme1);
        //     }
        // };
    }
    void Update()
    {
        if (Screen.orientation == ScreenOrientation.Portrait)
        {

            root.styleSheets.Remove(landscapeTheme);
            root.styleSheets.Add(portraitTheme);
            Debug.Log($"portrait");



        }
        else
        {
            root.styleSheets.Remove(portraitTheme);
            root.styleSheets.Add(landscapeTheme);
            Debug.Log($"landscape");

        }
    }


}
