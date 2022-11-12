using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour {
    public Slider slider;
    public Health health;

    public TextMeshProUGUI text;

    private void OnEnable() {
        ChangeValue();
    }

    public void ChangeValue() {
        text.text = (int)health.GetCurrentHealth() + " / " + health.GetMaxHealth();
        slider.value = health.GetCurrentHealth() / health.GetMaxHealth();
    }
}
