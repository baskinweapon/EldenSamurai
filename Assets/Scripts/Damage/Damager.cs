using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damager : MonoBehaviour {
    public float damageValue = 10f;
    public float duration = 1f;
    
    [SerializeField]
    private Collider2D col;

    private void OnEnable() {
        StartCoroutine(EndProcess());
    }

    // public void SetActive(bool _state) {
    //     col.enabled = _state;
    //     // if (_state)
    //     //     StartCoroutine(EndProcess());
    // }
    
    private void OnTriggerEnter2D(Collider2D col) {
        var health = col.GetComponentInParent<Health>();
        if (!health) return;
        health.Damage(damageValue);
    }

    IEnumerator EndProcess() {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
        col.enabled = false;
    }
}
