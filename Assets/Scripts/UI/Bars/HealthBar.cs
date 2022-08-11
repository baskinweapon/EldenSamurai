using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public Health health;
    public Slider sliderBar;
    public float speed = 5f;
    public Image fill;
    public Gradient gradient;
    
    public GameObject separator;
    
    // private RectTransform rect;
    private void Start() {
        // rect = GetComponent<RectTransform>();
        health.OnDamage.AddListener(Fade);
        health.OnHeal.AddListener(Fade);
        health.OnChangeMaxHealth += ChangeMaxHealth;
        sliderBar.maxValue = health.GetMaxHealth();
        sliderBar.value = health.GetCurrentHealth();
        // rect.SetRight(1000 - (health.GetMaxHealth() > 1000 ? 1000 : health.GetMaxHealth()));
        // var size = (int)health.GetMaxHealth() / 100f;
        // for (int i = 0; i < size; i++) {
        //     var sep = Instantiate(separator.gameObject, rect).GetComponent<RectTransform>();
        //     sep.localPosition = new Vector2(i * 100,0);
        // }
        
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
        // rect.SetRight(1000 - (health.GetMaxHealth() > 1000 ? 1000 : health.GetMaxHealth()));
        sliderBar.maxValue = health.GetMaxHealth();
        Fade(0);
    }
    
    private void OnDestroy() {
        health.OnDamage.RemoveListener(Fade);
        health.OnHeal.RemoveListener(Fade);
        health.OnChangeMaxHealth -= ChangeMaxHealth;
    }
}
