using System;
using UnityEngine;

namespace XNode
{
    /// <summary>
    /// 添加该属性, 则可以应用折叠功能
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FoldAttribute : PropertyAttribute
    {

    }
}