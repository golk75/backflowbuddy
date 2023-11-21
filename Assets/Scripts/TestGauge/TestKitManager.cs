using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestKitManager : MonoBehaviour
{
    public bool isDoubleCheckTesting;
    private DoubleCheckTestKitController doubleCheckTestKitController;
    void OnEnable()
    {
        doubleCheckTestKitController = GetComponent<DoubleCheckTestKitController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (isDoubleCheckTesting == true)
        {
            doubleCheckTestKitController.enabled = true;
        }
        else
        {
            doubleCheckTestKitController.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
