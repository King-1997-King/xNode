using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class DemoNodeGraph : NodeGraph
{
    public override Type RegisterBlackboardType()
    {
        return typeof(DemoBlackboard);
    }
}