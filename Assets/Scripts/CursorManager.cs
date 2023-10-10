using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{




    [SerializeField]
    private int frameCount;
    [SerializeField]
    private float frameRate;

    private int currentFrame;
    private float frameTimer;

    Vector2 _hotspot = new Vector2(9, 7);
    Texture2D _texture;
    [SerializeField]
    private List<CursorAnimation> cursorAnimationList;
    private CursorAnimation cursorAnimation;
    // Start is called before the first frame update
    public enum CursorType
    {
        Hand,
        Grab_hover,
        Grab,
        Point
    }
    void Start()
    {
        SetCursorAnimation(cursorAnimationList[0]);
    }

    void Awake()
    {

    }

    void Update()
    {

        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0)
        {
            frameTimer += cursorAnimation.frameRate;
            currentFrame = (currentFrame + 1) % frameCount;
            Cursor.SetCursor(cursorAnimation.textureArray[currentFrame], cursorAnimation.offset, CursorMode.Auto);
        }

        if (Input.GetMouseButtonDown(0))
        {

            SetCursorAnimation(cursorAnimationList[2]);
        }
        if (Input.GetMouseButtonUp(0))
        {
            SetCursorAnimation(cursorAnimationList[0]);
        }
    }

    private void SetCursorAnimation(CursorAnimation cursorAnimation)
    {
        this.cursorAnimation = cursorAnimation;
        currentFrame = 0;
        frameTimer = cursorAnimation.frameRate;
        frameCount = cursorAnimation.textureArray.Length;
    }




    [System.Serializable]
    public class CursorAnimation
    {
        public CursorType cursorType;
        public Texture2D[] textureArray;
        public float frameRate;
        public Vector2 offset;

    }
}
