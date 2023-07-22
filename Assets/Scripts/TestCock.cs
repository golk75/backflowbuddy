using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TestCock
{
    public OperableComponentDescription description;

    public TestCock(OperableComponentDescription componentDescription)
    {
        this.description = componentDescription;
    }
}
