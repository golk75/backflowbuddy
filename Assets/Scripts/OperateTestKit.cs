using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperateTestKit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { }

    public void DetectOperation()
    {
        Actions.OnTestKitOperate(this);
    }

    // Update is called once per frame
    void Update() { }
}
