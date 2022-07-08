using System;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using XNode;
using XNode.NodeGroups;
using XNodeEditor;

namespace XNodeEditor.NodeDescribes
{
    [NodeEditor.CustomNodeEditorAttribute(typeof(NodeDescribe))]
    public class NodeDescribeEditor : NodeEditor
    {
        private NodeDescribe _group;

        private NodeDescribe group {
            get { return _group != null ? _group : _group = target as NodeDescribe; }
        }

        private static Texture2D _corner;

        public static Texture2D corner {
            get { return _corner != null ? _corner : _corner = Resources.Load<Texture2D>("xnode_corner"); }
        }


        private bool isDragging = false;
        private Vector2 size;

        private Vector2 scroll;

        public override void OnBodyGUI() {
            Event e = Event.current;

            switch (e.type) {
                case EventType.MouseDrag:
                    if (isDragging) {
                        group.width = Mathf.Max(200, (int)e.mousePosition.x + 16);
                        group.height = Mathf.Max(100, (int)e.mousePosition.y - 34);
                        NodeEditorWindow.current.Repaint();
                    }

                    break;
                case EventType.MouseDown:
                    // Ignore everything except left clicks
                    if (e.button != 0) return;
                    if (NodeEditorWindow.current.nodeSizes.TryGetValue(target, out size)) {
                        // Mouse position checking is in node local space
                        Rect lowerRight = new Rect(size.x - 34, size.y - 34, 30, 30);
                        if (lowerRight.Contains(e.mousePosition)) {
                            isDragging = true;
                        }
                    }

                    break;
                case EventType.MouseUp:
                    isDragging = false;
                    break;
                case EventType.Repaint:
                    // Add scale cursors
                    if (NodeEditorWindow.current.nodeSizes.TryGetValue(target, out size)) {
                        Rect lowerRight = new Rect(target.position, new Vector2(30, 30));
                        lowerRight.y += size.y - 34;
                        lowerRight.x += size.x - 34;
                        lowerRight = NodeEditorWindow.current.GridToWindowRect(lowerRight);
                        NodeEditorWindow.current.onLateGUI += () => AddMouseRect(lowerRight);
                    }

                    break;
            }

            scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(group.height - 30));
            group.describe =
                EditorGUILayout.TextArea(group.describe);
            EditorGUILayout.EndScrollView();
            GUILayout.Space(30);
            GUI.DrawTexture(new Rect(group.width - 34, group.height + 16, 24, 24), corner);
        }

        public override Color GetTint() {
            return group.color;
        }

        public override int GetWidth() {
            return group.width;
        }

        public static void AddMouseRect(Rect rect) {
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeUpLeft);
        }
    }
}