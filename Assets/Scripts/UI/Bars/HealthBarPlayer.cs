using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Bars {
    public class HealthBarPlayer : MonoBehaviour {
        public Health health;
            public Slider sliderBar;
            public float speed = 5f;
            public Image fill;
            public Gradient gradient;
            
            public RectTransform separator;
            
            private RectTransform rect;
            private void Start() {
                rect = GetComponent<RectTransform>();
                health.OnDamage.AddListener(Fade);
                health.OnHeal.AddListener(Fade);
                health.OnChangeMaxHealth += ChangeMaxHealth;
                sliderBar.maxValue = health.GetMaxHealth();
                sliderBar.value = health.GetCurrentHealth();
                rect.SetRight(1000 - (health.GetMaxHealth() > 1000 ? 1000 : health.GetMaxHealth()));
                SetSeparator();
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


            private List<GameObject> separators = new List<GameObject>();
            private void SetSeparator() {
                foreach (var separator in separators) {
                    Destroy(separator);
                }
                separators.Clear();
                var sizeBetween = 100f; 
                var size = (int)(health.GetMaxHealth() / 100f);
                if (size > 10) {
                    var s = rect.rect.width / size;
                    sizeBetween = s;
                } 
                for (int i = 0; i < size - 1; i++) {
                    var sep = Instantiate(separator, rect);
                    sep.localPosition = new Vector2(rect.rect.x + sizeBetween + i * sizeBetween,0);
                    if (i % 5 == 0 && i != 0) sep.sizeDelta += Vector2.right;
                    separators.Add(sep.gameObject);
                }
            }
        
            private void ChangeMaxHealth() {
                rect.SetRight(1000 - (health.GetMaxHealth() > 1000 ? 1000 : health.GetMaxHealth()));
                SetSeparator();
                sliderBar.maxValue = health.GetMaxHealth();
                Fade(0);
            }
            
            private void OnDestroy() {
                health.OnDamage.RemoveListener(Fade);
                health.OnHeal.RemoveListener(Fade);
                health.OnChangeMaxHealth -= ChangeMaxHealth;
            }
    }
}