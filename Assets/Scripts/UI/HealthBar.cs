using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public Slider sliderBar;
    public float speed = 5f;
    public Image fill;
    public Gradient gradient;
    
    private void Start() {
        Player.instance.health.OnDamage.AddListener(Fade);
        Player.instance.health.OnHeal.AddListener(Fade);
        Player.instance.health.OnChangeMaxHealth += ChangeMaxHealth;
        sliderBar.maxValue = Player.instance.health.GetMaxHealth();
        sliderBar.value = Player.instance.health.GetCurrentHealth();
        fill.color = gradient.Evaluate(sliderBar.value);
    }
    
    private Coroutine fadeCoroutine;
    public void Fade(float value) {
        if (fadeCoroutine != null) return;
        fadeCoroutine = StartCoroutine(FadeProcess());
    }
    
    IEnumerator FadeProcess() {
        while (sliderBar.value != Player.instance.health.GetCurrentHealth()) {
            sliderBar.value = Mathf.Lerp(sliderBar.value, Player.instance.health.GetCurrentHealth(), Time.deltaTime * speed);
            fill.color = gradient.Evaluate(sliderBar.value / sliderBar.maxValue);
            yield return null;
        }

        fadeCoroutine = null;
    }

    private void ChangeMaxHealth() {
        sliderBar.maxValue = Player.instance.health.GetMaxHealth();
        Fade(0);
    }
    
    private void OnDestroy() {
        Player.instance.health.OnDamage.RemoveListener(Fade);
        Player.instance.health.OnHeal.RemoveListener(Fade);
        Player.instance.health.OnChangeMaxHealth -= ChangeMaxHealth;
    }
}
