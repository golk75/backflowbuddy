using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Texture2D operableCursor;
    private Cursor defaultCursor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnMouseEnter()
    {
        Cursor.SetCursor(operableCursor, Vector2.zero, CursorMode.Auto);
    }
    void OnMouseExit()
    {
        Cursor.SetCursor(default, default, default);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
