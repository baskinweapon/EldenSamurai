using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI.Anonimous {
    public class EnemiesCell : MonoBehaviour {
        public TextMeshProUGUI textInfo;
        public Image image;

        private GameObject prefab;
        
        public void FillInformation(EnemyInfo info) {
            image.sprite = info.image;
            textInfo.text = info.description;
            prefab = info.prefab;
        }

        public void Spawn() {
            if (!prefab) return;
            var obj = Instantiate(prefab);
            obj.transform.position = Player.instance.rb.position + Vector2.left * 3;
        }
    }
}