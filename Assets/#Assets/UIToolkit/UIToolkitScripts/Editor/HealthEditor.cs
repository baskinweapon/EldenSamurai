using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR


[CustomEditor(typeof(Health))]
public class HealthEditor : Editor {
	
	public VisualTreeAsset m_UXML;
	private Health health;
	private VisualElement root;
	
	public override VisualElement CreateInspectorGUI() {
		health = target as Health;
		
		root = new VisualElement();
		
		// standart Inspector
		var foldout = new Foldout() { viewDataKey = "TankManagerFullInspectorFoldOut", text = "Full Inspector" };
		InspectorElement.FillDefaultInspector(foldout, serializedObject, this);
		root.Add(foldout);
		
		m_UXML.CloneTree(root);
		
		var rootPanel = root.Q<VisualElement>("RootPanel");
		
		DamageHealPanel(rootPanel);
		
		ChangeHealthBar();
		
		var maxXPPanel = rootPanel.Q<VisualElement>("MaxXPPanel");
		
		var changeMaxXp = maxXPPanel.Q<VisualElement>("MaxXP");
		changeMaxXp.RegisterCallback<ClickEvent>(ChangeMaxXp);
		
		return root;
	}

	private void ChangeHealthBar() {
		var rootPanel = root.Q<VisualElement>("RootPanel");
		
		var healthBar = rootPanel.Q<ProgressBar>("HealthBar");
		healthBar.title = health.GetCurrentHealth() + " / " + health.GetMaxHealth();

		healthBar.value = health.GetCurrentHealth() / health.GetMaxHealth();
	}


	private void ChangeMaxXp(ClickEvent evt) {
		health.ChangeMaxHealth(100f);
		ChangeHealthBar();
	}

	private void DamageHealPanel(VisualElement root) {
		var damageHealPanel = root.Q<VisualElement>("DamageHealPanel");
		
		var damageButton = damageHealPanel.Q<Button>("DamageButton");
		damageButton.RegisterCallback<ClickEvent>(DamageButtonAction);
		
		var healButton = damageHealPanel.Q<Button>("HealButton");
		healButton.RegisterCallback<ClickEvent>(HealButtonAction);
	}

	private void DamageButtonAction(ClickEvent evt) {
		health.Damage(100f);
		ChangeHealthBar();
	}

	private void HealButtonAction(ClickEvent evt) {
		health.Heal(100f);
		ChangeHealthBar();
	}
}

#endif
