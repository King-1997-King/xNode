using System;
using UnityEngine;

namespace XNode
{
    [Serializable]
    public class NodeBlackboard : ScriptableObject
    {
        /// <summary> Parent <see cref="NodeGraph"/> </summary>
        [SerializeField] public NodeGraph graph;

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool isShow = true;

        public Rect rect = new Rect(0, 0, 200, 200);
    }
}