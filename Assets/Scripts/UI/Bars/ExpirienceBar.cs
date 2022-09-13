using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Bars {
    public class ExpirienceBar : MonoBehaviour {
        private Expirience expirience;
        public Slider sliderBar;
        public float speed = 5f;
        public Image fill;
        [GradientUsage(true)]
        public Gradient gradient;
        
        public TextMeshProUGUI text;
        
        private RectTransform rect;
        private void Start() {
            rect = GetComponent<RectTransform>();
            expirience = Player.instance.expirience;
            expirience.OnUpExpirence.AddListener(Fade);
            expirience.OnLevelUp.AddListener(ChangeLevel);
            sliderBar.maxValue = expirience.GetNeddedExpirience();
            sliderBar.value = expirience.GetCurrentExpirience();
            
            text.text = expirience.GetCurrentLevel() + " Lvl";
            
            fill.color = gradient.Evaluate(sliderBar.value);
        }

        private void ChangeLevel(int level) {
            sliderBar.maxValue = expirience.GetNeddedExpirience();
            sliderBar.value = expirience.GetCurrentExpirience();
        }
        
        private Coroutine fadeCoroutine;
        public void Fade(float value) {
            if (fadeCoroutine != null) return;
            fadeCoroutine = StartCoroutine(FadeProcess());
        }
    
        IEnumerator FadeProcess() {
            while (sliderBar.value != expirience.GetCurrentExpirience()) {
                sliderBar.value = Mathf.Lerp(sliderBar.value, expirience.GetCurrentExpirience(), Time.deltaTime * speed);
                fill.color = gradient.Evaluate(sliderBar.value / sliderBar.maxValue);
                yield return null;
            }

            text.text = expirience.GetCurrentLevel() + " Lvl";
            fadeCoroutine = null;
        }
        
        private void OnDestroy() {
            expirience.OnUpExpirence.RemoveListener(Fade);
            expirience.OnLevelUp.RemoveListener(ChangeLevel);
           
        }
    }
}