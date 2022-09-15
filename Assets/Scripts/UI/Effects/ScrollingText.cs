using System.Collections;
using TMPro;
using UnityEngine;

namespace UI.Effects {
    public class ScrollingText : MonoBehaviour {
        [SerializeField, TextArea]
        private string[] text;

        public float scrollSpeed = 0.01f;

        [SerializeField]
        private TextMeshProUGUI textHolder;

        private int currentDisplayText;

        public void ActivateText() {
            StartCoroutine(AnimateText());
        }

        public void NextText() {
            if (currentDisplayText < text.Length - 1)
                currentDisplayText++;
            StopAllCoroutines();
            StartCoroutine(AnimateText());
        }

        public void PrevText() {
            if (currentDisplayText > 0)
                currentDisplayText--;
            StopAllCoroutines();
            StartCoroutine(AnimateText());
        }

        IEnumerator AnimateText() {
            for (int i = 0; i < text[currentDisplayText].Length; i++) {
                textHolder.text = text[currentDisplayText][..i];

                yield return new WaitForSeconds(scrollSpeed * Time.deltaTime);
            }
        }
    }
}