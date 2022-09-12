using Currency;
using UnityEngine;

public class MoneyDrop: MonoBehaviour, IDrop {
    public int moneyValue = 100;
    
    public void CompleteDrop() {
        CurrencySystem.instance.AddMoney(moneyValue);
    }
}