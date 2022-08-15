using System;
using System.Collections;
using Currency;
using UnityEngine;
using Random = UnityEngine.Random;

public class DropObject : MonoBehaviour {
    public int moneyValue;

    private void OnEnable() {
        AddRandomForce();
    }

    private void AddRandomForce() {
        var vec = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
        GetComponent<Rigidbody2D>().AddForce(vec * Random.Range(200f, 1000f));
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (!col.gameObject.tag.Contains("Player")) return;
        Debug.Log("Add money");
        GetComponent<Collider2D>().enabled = false;
        CurrencySystem.instance.AddMoney(moneyValue);
        StartCoroutine(CollectProcess());
    }
    
    IEnumerator CollectProcess() {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
