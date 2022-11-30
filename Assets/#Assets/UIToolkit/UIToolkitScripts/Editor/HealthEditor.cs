using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR

[CustomEditor(typeof(Health))]
public class HealthEditor : Editor {
	
	public VisualTreeAsset m_UXML;
	public Gradient gradient;
	
	private Health health;
	private VisualElement root;
	
	public override VisualElement CreateInspectorGUI() {
		health = target as Health;
		
		CreateGradient();

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

	private void ProgressBar(float _, Collider2D col) {
		var rootPanel = root.Q<VisualElement>("RootPanel");
		var progressBar = rootPanel.Q<VisualElement>("ProgressBar");

		var bg = progressBar.Q<VisualElement>("Background");
		
		var fill = bg.Q<VisualElement>("Fill");
		var title = bg.Q<VisualElement>("Title");

		var text = title.Q<Label>("Value");
		text.text = health.GetCurrentHealth() + " / " + health.GetMaxHealth();
		
		var visual = fill.Q<VisualElement>("VisualElement");

		var value = health.GetCurrentHealth() / health.GetMaxHealth();
		visual.style.backgroundColor = gradient.Evaluate(value);
		
		fill.style.width = Length.Percent(value * 100);
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
		visual.style.backgroundColor = gradient.Evaluate(value);
		
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
		health.Damage(100f, null);
	}

	private void HealButtonAction(ClickEvent evt) {
		health.Heal(100f);
	}
	
	#region Helpers
	
	private void CreateGradient() {
		gradient = new Gradient();
		
		GradientColorKey[] colorKeys;
		GradientAlphaKey[] alphaKeys;
		alphaKeys = new GradientAlphaKey[2];
		alphaKeys[0].alpha = 1;
		alphaKeys[0].time = 0;
		alphaKeys[1].alpha = 1;
		alphaKeys[1].time = 1;
		
		colorKeys = new GradientColorKey[2];
		colorKeys[0].color = Color.red;
		colorKeys[0].time = 0;
		colorKeys[1].color = Color.green;
		colorKeys[1].time = 1;
		gradient.SetKeys(colorKeys, alphaKeys);	
	}

	#endregion
}

#endif
