using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Actions
{
    public static Action<GameObject, OperableComponentDescription> onHoseAttach;
    public static Action<GameObject, OperableComponentDescription> onHoseDetach;
    public static Action<GameObject, OperableComponentDescription> onHoseBibConnect;
    public static Action<GameObject> onCheckClosed;
    public static Action<GameObject> onCheckOpened;
    public static Action<bool, GameObject> onTestCockOpen;
    public static Action<GameObject> onCheck1Closed;
    public static Action<GameObject> onCheck1Opened;
    public static Action<GameObject> onCheck2Closed;
    public static Action<GameObject> onCheck2Opened;
    public static Action onTestCock1Opened;
    public static Action onTestCock1Closed;
    public static Action onTestCock2Opened;
    public static Action onTestCock2Closed;
    public static Action onTestCock3Opened;
    public static Action onTestCock3Closed;
    public static Action onTestCock4Opened;
    public static Action onTestCock4Closed;
    public static Action<GameObject, OperableComponentDescription> onHoseBibGrab;
    public static Action<GameObject, OperableComponentDescription> onHoseBibDrop;
}
