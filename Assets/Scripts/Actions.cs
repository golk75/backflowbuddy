using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Actions
{
    public static Action<GameObject> onHoseAttach;
    public static Action<GameObject> onHoseDetach;

    public static Action<GameObject> onCheckClosed;
    public static Action<GameObject> onCheckOpened;
    public static Action onCheck1Closed;
    public static Action onCheck1Opened;
    public static Action onCheck2Closed;
    public static Action onCheck2Opened;
}
