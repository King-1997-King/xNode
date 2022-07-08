using Sirenix.OdinInspector;
using UnityEngine;

namespace XNode
{
    [CreateNodeMenu("描述")]
    public class NodeDescribe : Node
    {
        public string describe = null;


        public int width = 200;
        public int height = 100;
        public Color color = new Color(0.55581f, 0, 1, 0.1f);
    }
}