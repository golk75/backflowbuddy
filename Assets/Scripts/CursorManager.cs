using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCursorTexture;
    public Texture2D clickedCursorTexture;
    Vector2 _hotspot = new Vector2(9, 7);
    Texture2D _texture;
    // Start is called before the first frame update
    void Start()
    {



        void Awake()
        {
            _texture = Resources.Load<Texture2D>("Assets/Cursor/G_Cursor_Settings.png");
        }

        void Update()
        {


        }
    }
}
