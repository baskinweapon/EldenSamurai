using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ManaBar : MonoBehaviour {
    private Mana mana;
    
    public Slider sliderBar;
    public float speed = 5f;
    public Image fill;
    [GradientUsage(true)]
    public Gradient gradient;

    public TextMeshProUGUI text;
    
    private RectTransform rect;
    private void Start() {
        rect = GetComponent<RectTransform>();
        mana = Player.instance.mana;
        mana.OnSpend.AddListener(Fade);
        mana.OnRest.AddListener(Fade);
        mana.OnChangeMaxMana += ChangeMaxHealth;
        sliderBar.maxValue = Player.instance.mana.GetMaxMana();
        sliderBar.value = Player.instance.mana.GetCurrentMana();
        
        rect.SetRight(1000 - (mana.GetMaxMana() > 1000 ? 1000 : mana.GetMaxMana()));
        
        fill.color = gradient.Evaluate(sliderBar.value);
        
        text.text = Mathf.Round(mana.GetCurrentMana()) + "/" + Mathf.Round(mana.GetMaxMana());
        
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
            text.text = Mathf.Round(sliderBar.value) + "/" + Mathf.Round(mana.GetMaxMana());
            yield return null;
        }

        fadeCoroutine = null;
    }

    private void ChangeMaxHealth() {
        rect.SetRight(1000 - (mana.GetMaxMana() > 1000 ? 1000 : mana.GetMaxMana()));
        sliderBar.maxValue = mana.GetMaxMana();
        Fade(0);
    }
    
    private void OnDestroy() {
        mana.OnSpend.RemoveListener(Fade);
        mana.OnRest.RemoveListener(Fade);
        mana.OnChangeMaxMana -= ChangeMaxHealth;
    }
}
