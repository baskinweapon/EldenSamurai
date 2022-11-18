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
		
		m_UXML.CloneTree(root);
		
		var rootPanel = root.Q<VisualElement>("RootPanel");
		
		DamageHealPanel(rootPanel);
		
		ProgressBar();
		
		health.OnDamage.AddListener(ProgressBar);
		health.OnHeal.AddListener(ProgressBar);
		health.OnChangeMaxHealth.AddListener(ProgressBar);
		
		var maxXPPanel = rootPanel.Q<VisualElement>("MaxXPPanel");
		
		var changeMaxXp = maxXPPanel.Q<VisualElement>("MaxXP");
		changeMaxXp.RegisterCallback<ClickEvent>(ChangeMaxXp);
		
		// standart Inspector
		var foldout = new Foldout() { viewDataKey = "TankManagerFullInspectorFoldOut", text = "Full Inspector" };
		InspectorElement.FillDefaultInspector(foldout, serializedObject, this);
		root.Add(foldout);
		
		return root;
	}

	private void ProgressBar(float _ = 0f) {
		var rootPanel = root.Q<VisualElement>("RootPanel");
		var progressBar = rootPanel.Q<VisualElement>("ProgressBar");

		var bg = progressBar.Q<VisualElement>("Background");
		
		var fill = bg.Q<VisualElement>("Fill");
		var title = bg.Q<VisualElement>("Title");

		var text = title.Q<Label>("Value");
		text.text = health.GetCurrentHealth() + " / " + health.GetMaxHealth();
		
		var visual = fill.Q<VisualElement>("VisualElement");

		var value = health.GetCurrentHealth() / health.GetMaxHealth();
		Color color = new Color(value, 1, value, 1);
		visual.style.backgroundColor = color;
		
		fill.style.width = Length.Percent(value * 100);
		
	}

	private void ChangeMaxXp(ClickEvent evt) {
		health.ChangeMaxHealth(100f);
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
	}

	private void HealButtonAction(ClickEvent evt) {
		health.Heal(100f);
	}
}

#endif
