using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ManaBar : MonoBehaviour {
    private Mana mana;
    
    public Slider sliderBar;
    public float speed = 5f;
    public Image fill;
    public Gradient gradient;
    
    private RectTransform rect;
    private void Start() {
        rect = GetComponent<RectTransform>();
        mana = Player.instance.mana;
        Player.instance.mana.OnSpend.AddListener(Fade);
        Player.instance.mana.OnRest.AddListener(Fade);
        Player.instance.mana.OnChangeMaxMana += ChangeMaxHealth;
        sliderBar.maxValue = Player.instance.mana.GetMaxMana();
        sliderBar.value = Player.instance.mana.GetCurrentMana();
        
        rect.SetRight(1000 - (mana.GetMaxMana() > 1000 ? 1000 : mana.GetMaxMana()));
        
        fill.color = gradient.Evaluate(sliderBar.value);
        
        
    }
    
    private Coroutine fadeCoroutine;
    public void Fade(float value) {
        if (fadeCoroutine != null) return;
        fadeCoroutine = StartCoroutine(FadeProcess());
    }
    
    IEnumerator FadeProcess() {
        while (sliderBar.value != Player.instance.mana.GetCurrentMana()) {
            sliderBar.value = Mathf.Lerp(sliderBar.value, Player.instance.mana.GetCurrentMana(), Time.deltaTime * speed);
            fill.color = gradient.Evaluate(sliderBar.value / sliderBar.maxValue);
            yield return null;
        }

        fadeCoroutine = null;
    }

    private void ChangeMaxHealth() {
        rect.SetRight(1000 - (mana.GetMaxMana() > 1000 ? 1000 : mana.GetMaxMana()));
        sliderBar.maxValue = Player.instance.mana.GetMaxMana();
        Fade(0);
    }
    
    private void OnDestroy() {
        Player.instance.mana.OnSpend.RemoveListener(Fade);
        Player.instance.mana.OnRest.RemoveListener(Fade);
        Player.instance.mana.OnChangeMaxMana -= ChangeMaxHealth;
    }
}
