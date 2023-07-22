using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperableComponentsUsed : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> GameObjectsUsed;

    //GameObject gameObject;

    public void AddUsedComponent(GameObject gameObject)
    {
        if (!GameObjectsUsed.Contains(gameObject))
            GameObjectsUsed.Add(gameObject);
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
