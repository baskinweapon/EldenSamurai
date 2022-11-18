using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(GameMain))]
public class TestUITollkit : Editor {
	
	public VisualTreeAsset m_UXML;
	
	public override VisualElement CreateInspectorGUI() {
		
		var root = new VisualElement();
		m_UXML.CloneTree(root);

		var foldout = new Foldout() { viewDataKey = "TankManagerFullInspectorFoldOut", text = "Full Inspector" };
		InspectorElement.FillDefaultInspector(foldout, serializedObject, this);
		root.Add(foldout);
		
		return root;
	}
}
