using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HoseDetector : MonoBehaviour
{
    public TestCockController testCockController;
    // public DoubleCheckTestKitController doubleCheckTestKitController;
    public RpzTestKitController rpzTestKitController;
    public GameObject testCock;
    public GameObject currentHoseConnection;
    public GameObject currentHose;
    public List<GameObject> disallowedConnections;
    public bool isConnectionAttempting;
    public bool isConnected = false;
    Coroutine onAttachAttempt;

    public PlayerController playerController;
    public OperableComponentDescription currentTestCockDescription;
    public OperableComponentDescription currentHoseDescription;
    private Collider boxCollider;
    private Coroutine InitialColliderBlock;

    public CameraController cameraController;
    void OnEnable()
    {

        boxCollider = GetComponent<Collider>();

        InitialColliderBlock = StartCoroutine(HideCollider());
    }
    void Start()
    {
        if (boxCollider.enabled != false)
        {
            boxCollider.enabled = false;
        }
        currentTestCockDescription = GetComponent<OperableComponentDescription>();
    }
    /// <summary>
    /// If collider is enabled at opening of scene, the hosebib gets thrown around due to the hosbib config joint moving the bib through the collider @Start
    /// </summary>
    /// <returns></returns>
    private IEnumerator HideCollider()
    {
        yield return new WaitForSeconds(1f);
        if (boxCollider.enabled == false)
        {
            boxCollider.enabled = true;
        }

    }
    public void OnTriggerEnter(Collider other)
    {
        currentHoseDescription = other.GetComponent<OperableComponentDescription>();
        if (!rpzTestKitController.AttachedHoseList.Contains(other.gameObject))
        {
            currentHose = other.gameObject;
        }
        else
        {
            currentHose = null;
        }



        if (cameraController.isPanning == false)
        {
            if (currentHose != null)
            {

                foreach (var item in disallowedConnections)
                {
                    if (currentHose == item)
                        return;
                }

                onAttachAttempt = StartCoroutine(AttachInitiate(currentHose));
            }

            Actions.onTestCockColliderEnter?.Invoke(this.gameObject, currentTestCockDescription);

        }

        isConnectionAttempting = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isConnectionAttempting = false;


        //check if exiting collider is connected or just passing through
        if (rpzTestKitController.AttachedHoseList.Contains(currentHoseConnection) && rpzTestKitController.AttachedTestCockList.Contains(this.gameObject) && currentHoseConnection == other.gameObject)
        {
            Actions.onRemoveHoseFromList?.Invoke(currentHoseConnection, GetComponent<OperableComponentDescription>());
            Actions.onRemoveTestCockFromList?.Invoke(this.gameObject, GetComponent<OperableComponentDescription>());
            currentHoseConnection = null;
            currentHose = null;
            isConnected = false;
        }
        //for SightTubeController
        Actions.onTestCockColliderExit?.Invoke(this.gameObject, GetComponent<OperableComponentDescription>());


    }

    IEnumerator AttachInitiate(GameObject currentAttachment)
    {
        //check if test cock and hose are already in lists @rpzTestKitController, if they are, then do not add them again ---->
        if (!rpzTestKitController.AttachedTestCockList.Contains(this.gameObject) && !rpzTestKitController.AttachedHoseList.Contains(currentHoseConnection))
        {
            yield return new WaitForSeconds(0.5f);
            if (isConnectionAttempting == true)
            {
                // if (currentTestCockDescription.componentId == OperableComponentDescription.ComponentId.TestCock4 && currentHoseDescription.componentId == OperableComponentDescription.ComponentId.LowHose)
                // {
                //     isConnected = false;
                // }

                //currentTestCockDescription: HoseDetector4 (OperableComponentDescription); currentHoseDescription: LowHoseTipHandle (OperableComponentDescription)
                currentHoseConnection = currentAttachment;
                Actions.onAddTestCockToList?.Invoke(this.gameObject, currentTestCockDescription);
                Actions.onAddHoseToList?.Invoke(currentAttachment, currentHoseDescription);
                isConnected = true;
                //send data on test cock that is having an attachment added
                Actions.onComponentConnect?.Invoke(this.gameObject, currentTestCockDescription, currentHose);

            }

        }


    }
}
