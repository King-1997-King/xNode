using System;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace XNodeEditor
{
    [CustomNodeBlackboardEditor(typeof(XNode.NodeBlackboard))]
    public class NodeBlackboardEditor : XNodeEditor.Internal.NodeEditorBase<NodeBlackboardEditor,
        NodeBlackboardEditor.CustomNodeBlackboardEditorAttribute, XNode.NodeBlackboard>
    {
        #if ODIN_INSPECTOR
        protected internal static bool inNodeEditor = false;
        #endif

        /// <summary> Draws standard field editors for all public fields </summary>
        public virtual void OnBodyGUI() {
            #if ODIN_INSPECTOR
            inNodeEditor = true;
            #endif

            // Unity specifically requires this to save/update any serial object.
            // serializedObject.Update(); must go at the start of an inspector gui, and
            // serializedObject.ApplyModifiedProperties(); goes at the end.
            serializedObject.Update();
            string[] excludes = { "m_Script", "isShow", "graph", "position" };

            #if ODIN_INSPECTOR
            try {
                #if ODIN_INSPECTOR_3
                objectTree.BeginDraw(true);
                #else
                InspectorUtilities.BeginDrawPropertyTree(objectTree, true);
                #endif
            } catch (ArgumentNullException) {
                #if ODIN_INSPECTOR_3
                objectTree.EndDraw();
                #else
                InspectorUtilities.EndDrawPropertyTree(objectTree);
                #endif
                NodeBlackboardEditor.DestroyEditor(this.target);
                return;
            }

            GUIHelper.PushLabelWidth(84);
            objectTree.Draw(true);
            #if ODIN_INSPECTOR_3
            objectTree.EndDraw();
            #else
            InspectorUtilities.EndDrawPropertyTree(objectTree);
            #endif
            GUIHelper.PopLabelWidth();
            #else
            // Iterate through serialized properties and draw them like the Inspector (But with ports)
            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren)) {
                enterChildren = false;
                if (excludes.Contains(iterator.name)) continue;
                NodeEditorGUILayout.PropertyField(iterator, true);
            }
            #endif

            serializedObject.ApplyModifiedProperties();

            #if ODIN_INSPECTOR
            // Call repaint so that the graph window elements respond properly to layout changes coming from Odin
            if (GUIHelper.RepaintRequested) {
                GUIHelper.ClearRepaintRequest();
                window.Repaint();
            }
            #endif

            #if ODIN_INSPECTOR
            inNodeEditor = false;
            #endif

            ResizeGui();
        }

        private void ResizeGui() {
            Event e = Event.current;
            switch (e.type) {
            case EventType.Repaint:
                // Add scale cursors
                Rect lowerRight = new Rect(GetInWindowPos() + new Vector2(target.rect.width - 10, target.rect.height - 10), new Vector2(10, 10));
                // lowerRight = NodeEditorWindow.current.GridToWindowRect(lowerRight);
                NodeEditorWindow.current.onLateGUI += () => AddMouseRect(lowerRight);
                break;
            }
        }

        /// <summary>
        /// 获取在窗口中的坐标
        /// </summary>
        /// <returns></returns>
        public Vector2 GetInWindowPos() {
            return target.rect.position + new Vector2(0, 25);
        }

        /// <summary>
        /// 获取在窗口中的大小
        /// </summary>
        /// <returns></returns>
        public Rect GetInWindowRect() {
            return new Rect(GetInWindowPos(), target.rect.size);

        }
        
        public static void AddMouseRect(Rect rect) {
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeUpLeft);
        }


        [AttributeUsage(AttributeTargets.Class)]
        public class CustomNodeBlackboardEditorAttribute : Attribute,
            XNodeEditor.Internal.NodeEditorBase<NodeBlackboardEditor,
                NodeBlackboardEditor.CustomNodeBlackboardEditorAttribute, XNode.NodeBlackboard>.INodeEditorAttrib
        {
            private Type inspectedType;

            /// <summary> Tells a NodeBlackboardEditor which NodeBlackboard type it is an editor for </summary>
            /// <param name="inspectedType">Type that this editor can edit</param>
            public CustomNodeBlackboardEditorAttribute(Type inspectedType) {
                this.inspectedType = inspectedType;
            }

            public Type GetInspectedType() {
                return inspectedType;
            }
        }
    }
}