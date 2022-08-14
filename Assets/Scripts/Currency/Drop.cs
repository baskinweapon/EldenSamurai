using System;
using Currency;
using UnityEngine;

public class Drop : MonoBehaviour {
    public int dropValue;

    public void OnEnable() {
        GetComponentInParent<Health>().OnDeath.AddListener(Dropped);
    }

    private void Dropped() {
        CurrencySystem.instance.AddMoney(dropValue);
    }

    private void OnDisable() {
        GetComponentInParent<Health>().OnDeath.RemoveListener(Dropped);
    }
}
