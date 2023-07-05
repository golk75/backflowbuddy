using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    // Start is called before the first frame update
    public GameEvent gameEvent;
    public UnityEvent response;

    void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    void OnDisable()
    {
        gameEvent.UnRegisterListener(this);
    }

    public void OnEventRaised()
    {
        response.Invoke();
    }
}
