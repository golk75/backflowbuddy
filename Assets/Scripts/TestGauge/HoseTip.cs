using System.Collections;
using UnityEngine;


public class HoseTip : MonoBehaviour
{
    Coroutine InitialColliderBlock;
    Collider boxCollider;
    public bool isConnectedToTestCock = false;
    public GameObject currentTestCockConnection;
    OperableComponentDescription description;
    // Start is called before the first frame update
    void OnEnable()
    {
        boxCollider = GetComponent<Collider>();
    }
    void Start()
    {
        InitialColliderBlock = StartCoroutine(HideCollider());
        description = GetComponent<OperableComponentDescription>();

    }
    void OnTriggerEnter(Collider other)
    {
        isConnectedToTestCock = true;
        currentTestCockConnection = other.gameObject;
        Actions.onHoseColliderEnter?.Invoke(this.gameObject, currentTestCockConnection, description);
    }
    void OnTriggerStay(Collider other)
    {
        isConnectedToTestCock = true;
        currentTestCockConnection = other.gameObject;
    }
    void OnTriggerExit(Collider other)
    {
        isConnectedToTestCock = false;
        Actions.onHoseColliderExit?.Invoke(this.gameObject, currentTestCockConnection, description);
    }
    private IEnumerator HideCollider()
    {
        yield return new WaitForSeconds(1);
        if (boxCollider.enabled == false)
        {
            boxCollider.enabled = true;
        }

    }
    // Update is called once per frame
    void Update()
    {

    }
}
