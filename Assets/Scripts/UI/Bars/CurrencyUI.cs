using System.Collections;
using Currency;
using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI text;
    public int step = 1;
    
    void Start() {
        CurrencySystem.instance.OnAddMoney += ChangeCurrency;
        CurrencySystem.instance.OnSpendMoney += ChangeCurrency;
        text.text = CurrencySystem.instance.GetCurrentMoney().ToString();
    }

    private void ChangeCurrency(int value) {
        StopAllCoroutines();
        StartCoroutine(ChangeProcess());
    }

    
    IEnumerator ChangeProcess() {
        var curent = int.Parse(text.text);
        var sign = curent < CurrencySystem.instance.GetCurrentMoney() ? 1 : -1;
        while (curent != CurrencySystem.instance.GetCurrentMoney()) {
            curent += step * sign;
            text.text = curent.ToString();
            yield return null;
        }
    }
    
    private void OnDestroy() {
        CurrencySystem.instance.OnAddMoney -= ChangeCurrency;
        CurrencySystem.instance.OnSpendMoney -= ChangeCurrency;
    }
}
