using CodiceApp;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


#if UNITY_EDITOR

[CustomEditor(typeof(PlayerMind))]
public class PLayerMindEditor : Editor {
	public VisualTreeAsset m_UXML;

	private Label curStateLabel;
	private PlayerMind playerMind;

	private VisualElement rootPanel;
	
	public override VisualElement CreateInspectorGUI() {
		playerMind = target as PlayerMind;

		playerMind.OnChangeStateType += ChangeState;
		playerMind.OnChangeIsGround += IsGround;
		
		var root = new VisualElement();

		m_UXML.CloneTree(root);
		
		rootPanel = root.Q<VisualElement>("RootPanel");
		
		var curStatePanlel = rootPanel.Q<VisualElement>("StateLabel");
		curStateLabel = curStatePanlel.Q<Label>("StateLabel");
		
		if (playerMind)
			curStateLabel.text = playerMind.stateType.ToString();
		
		var foldout = new Foldout() { viewDataKey = "TankManagerFullInspectorFoldOut", text = "Full Inspector" };
		InspectorElement.FillDefaultInspector(foldout, serializedObject, this);
		root.Add(foldout);

		return root;
	}

	private void IsGround(bool isGround) {
		var panel = rootPanel.Q<VisualElement>("IsGround");

		var ground = rootPanel.Q<VisualElement>("Ground");
		var player = rootPanel.Q<VisualElement>("Player");

		if (isGround) {
			ground.style.backgroundColor = Color.green;
			player.transform.position = Vector3.zero;
		}
		else {
			ground.style.backgroundColor = Color.red;
			player.transform.position += Vector3.down * 25;
		}
	}

	private void ChangeState(PlayerStatesType state) {
		if (playerMind)
			curStateLabel.text = state.ToString();
	}
}

#endif
