using System;
using Scriptble.Enemies;
using TMPro;
using UnityEngine;

namespace UI.Anonimous {
    public class EnemiesPanel : MonoBehaviour {
        public EnemiesCell cell;
        public GameObject panel;
        public RectTransform buttonTr;
        public EnemiesInformation enemiesInformation;

        public Transform content;
        
        private void Awake() {
            foreach (var info in enemiesInformation.enemies) {
                var _cell = Instantiate(cell, content).GetComponent<EnemiesCell>();
                _cell.FillInformation(info);
            }
        }

        public void OpenClosePanel() {
            panel.SetActive(!panel.activeSelf);
            var rect = panel.GetComponent<RectTransform>();
            buttonTr.GetComponentInChildren<TextMeshProUGUI>().text = panel.activeSelf ? ">" : "<";
            if (panel.activeSelf) buttonTr.anchoredPosition -= new Vector2(rect.rect.width, 0);
            else buttonTr.anchoredPosition += new Vector2(rect.rect.width, 0);
        }
    }
}