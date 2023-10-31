using UnityEngine;
using UnityEngine.UIElements;


public class CursorController : MonoBehaviour
{

    private VisualElement buttonWrapper;
    VisualElement sceneContainer;
    UIDocument root;
    UnityEngine.UIElements.Cursor cursor_default;
    UnityEngine.UIElements.Cursor cursor_grab;


    private void Awake()
    {
        root = GetComponent<UIDocument>();
        buttonWrapper = root.rootVisualElement.Q<VisualElement>("ButtonWrapper");
        sceneContainer = root.rootVisualElement.Q<VisualElement>("SceneContainer");
        cursor_grab = new()
        {
            texture = Resources.Load<Texture2D>("UI/Textures/icons8-hand-34"),
            hotspot = new Vector2(15, 12)
        };
        cursor_default = new()
        {
            texture = Resources.Load<Texture2D>("UI/Textures/icons8-hand-32"),
            hotspot = new Vector2(15, 12)
        };

        sceneContainer.RegisterCallback<MouseDownEvent>(MouseDown);
        sceneContainer.RegisterCallback<MouseUpEvent>(MouseUp);
        buttonWrapper.RegisterCallback<MouseDownEvent>(MouseDown);
        buttonWrapper.RegisterCallback<MouseUpEvent>(MouseUp);


    }

    private void MouseUp(MouseUpEvent evt)
    {
        sceneContainer.style.cursor = new StyleCursor(cursor_default);
        buttonWrapper.style.cursor = new StyleCursor(cursor_default);
    }

    private void MouseDown(MouseDownEvent evt)
    {

        sceneContainer.style.cursor = new StyleCursor(cursor_grab);
        buttonWrapper.style.cursor = new StyleCursor(cursor_grab);


    }
    private void Start()
    {


    }
    void Update()
    {

    }

}