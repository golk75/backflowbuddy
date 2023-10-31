using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperableComponentsUsed : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> GameObjectsUsed;


    public void AddUsedComponent(GameObject gameObject)
    {
        if (!GameObjectsUsed.Contains(gameObject))
            GameObjectsUsed.Add(gameObject);
    }

}
