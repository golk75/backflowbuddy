
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class CursorController : MonoBehaviour

{
    // private Button fillButton;
    // private Button menuButton;
    // private Button resetButton;
    // public VisualElement tooltip;
    private VisualElement buttonWrapper;



    // public ToolTipScriptableObject fillButtonTooltip;
    // public ToolTipScriptableObject menuButtonTooltip;
    // public ToolTipScriptableObject resetButtonTooltip;
    UIDocument root;
    [SerializeField]
    private Texture2D[] cursorTextureArray;
    UnityEngine.UIElements.Cursor cursor_default;
    UnityEngine.UIElements.Cursor cursor_grab;
    VisualElement sceneContainer;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    /// 
    private void Awake()
    {
        root = GetComponent<UIDocument>();

        // fillButton = root.rootVisualElement.Q<Button>("FillButton");
        // menuButton = root.rootVisualElement.Q<Button>("MenuButton");
        // resetButton = root.rootVisualElement.Q<Button>("ResetButton");
        buttonWrapper = root.rootVisualElement.Q<VisualElement>("ButtonWrapper");
        //tooltip = root.rootVisualElement.Q<VisualElement>("Tooltip");
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




        // resetButton.RegisterCallback<MouseDownEvent>(MouseDown);
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