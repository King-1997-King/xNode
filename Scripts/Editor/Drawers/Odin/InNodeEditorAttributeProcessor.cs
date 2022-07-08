#if UNITY_EDITOR && ODIN_INSPECTOR
using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEngine;
using XNode;

namespace XNodeEditor {
	internal class OdinNodeInGraphAttributeProcessor<T> : OdinAttributeProcessor<T> where T : Node {
		public override bool CanProcessSelfAttributes(InspectorProperty property) {
			return false;
		}

		public override bool CanProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member) {
			if (!NodeEditor.inNodeEditor)
				return false;

			return true;
		}

		public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes) {
			switch (member.Name) {
				case "graph":
				case "position":
				case "ports":
					attributes.Add(new HideInInspector());
					break;

				default:
					break;
			}

			for( int i = 0; i < attributes.Count; i++ ) {
				if (attributes[i] is FoldAttribute) {
					attributes.RemoveAt(i);
					attributes.Insert(i, new HideIfAttribute("@isFold"));
					break;
				}
			}
		}
	}
	internal class BlackboardAttributeProcessor<T> : OdinAttributeProcessor<T> where T : NodeBlackboard {
		public override bool CanProcessSelfAttributes(InspectorProperty property) {
			return false;
		}

		public override bool CanProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member) {
			if (!NodeBlackboardEditor.inNodeEditor)
				return false;

			if (member.MemberType == MemberTypes.Field) {
				switch (member.Name) {
					case "graph":
					case "isShow":
					case "rect":
						return true;

					default:
						break;
				}
			}

			return false;
		}

		public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes) {
			switch (member.Name) {
				case "graph":
				case "isShow":
				case "rect":
					attributes.Add(new HideInInspector());
					break;

				default:
					break;
			}
		}
	}
	
	
}
#endif