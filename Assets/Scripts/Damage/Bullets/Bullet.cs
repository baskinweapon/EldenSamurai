using Damage;
using UnityEngine;


public class Bullet : MonoBehaviour {
    [SerializeField] private Damager damager;
    
    private void OnEnable() {
        damager.OnDamaged.AddListener(Damage);
    }

    private void Damage() {
        gameObject.SetActive(false);
    }

    private void OnDisable() {
        damager.OnDamaged.RemoveListener(Damage);
    }
}