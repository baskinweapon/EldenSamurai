using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public Health health;
    public Slider sliderBar;
    public float speed = 5f;
    public Image fill;
    public Gradient gradient;
    
    private void Start() {
        health.OnDamage.AddListener(Fade);
        health.OnHeal.AddListener(Fade);
        health.OnChangeMaxHealth += ChangeMaxHealth;
        sliderBar.maxValue = health.GetMaxHealth();
        sliderBar.value = health.GetCurrentHealth();
        fill.color = gradient.Evaluate(sliderBar.value);
    }
    
    private Coroutine fadeCoroutine;
    public void Fade(float value) {
        if (fadeCoroutine != null) return;
        fadeCoroutine = StartCoroutine(FadeProcess());
    }
    
    IEnumerator FadeProcess() {
        while (sliderBar.value != health.GetCurrentHealth()) {
            sliderBar.value = Mathf.Lerp(sliderBar.value, health.GetCurrentHealth(), Time.deltaTime * speed);
            fill.color = gradient.Evaluate(sliderBar.value / sliderBar.maxValue);
            yield return null;
        }

        fadeCoroutine = null;
    }

    private void ChangeMaxHealth() {
        sliderBar.maxValue = health.GetMaxHealth();
        Fade(0);
    }
    
    private void OnDestroy() {
        health.OnDamage.RemoveListener(Fade);
        health.OnHeal.RemoveListener(Fade);
        health.OnChangeMaxHealth -= ChangeMaxHealth;
    }
}
