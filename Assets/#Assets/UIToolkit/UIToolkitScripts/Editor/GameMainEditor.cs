
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;


#if UNITY_EDITOR

[CustomEditor(typeof(GameMain))]
public class GameMainEditor : Editor {
	
	public VisualTreeAsset m_UXML;
	
	public override VisualElement CreateInspectorGUI() {
		
		var root = new VisualElement();
		m_UXML.CloneTree(root);
		
		// standart Inspector
		var foldout = new Foldout() { viewDataKey = "TankManagerFullInspectorFoldOut", text = "Full Inspector" };
		InspectorElement.FillDefaultInspector(foldout, serializedObject, this);
		root.Add(foldout);
		
		return root;
	}
}

#endif
