using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckValveStatus : MonoBehaviour
{
    public bool isCheck1Open = false;
    public bool isCheck1Closed = false;
    public bool isCheck2Open = false;
    public bool isCheck2Closed = false;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        Actions.onCheck1Closed += Check1Closed;
        Actions.onCheck2Closed += Check2Closed;
        Actions.onCheck1Opened += Check1Opened;
        Actions.onCheck2Opened += Check2Opened;
    }

    private void OnDisable()
    {
        Actions.onCheck1Closed -= Check1Closed;
        Actions.onCheck2Closed -= Check2Closed;
        Actions.onCheck1Opened -= Check1Opened;
        Actions.onCheck2Opened -= Check2Opened;
    }

    public void Check1Closed(GameObject gameObject)
    {
        isCheck1Closed = true;
        isCheck1Open = false;
    }

    public void Check1Opened(GameObject gameObject)
    {
        isCheck1Closed = false;
        isCheck1Open = true;
    }

    public void Check2Closed(GameObject gameObject)
    {
        isCheck2Closed = true;
        isCheck2Open = false;
    }

    public void Check2Opened(GameObject gameObject)
    {
        isCheck2Closed = false;
        isCheck2Open = true;
    }

    void Update()
    {
        Debug.Log($"isCheck1Closed = {isCheck1Closed}");
    }
}
