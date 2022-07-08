using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DemoNode : Node
{

    public enum MyEnum
    {
        N, M, L
    }

    [NodeEnum]
    [Fold]
    public MyEnum myEnum;

    [Input] public string a;
    [Output] public string b;

    [Fold]
    public float c;
}