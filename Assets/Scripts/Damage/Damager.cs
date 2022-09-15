using Damage;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damager : BaseDamager {
    protected override void OnTriggerEnter2D(Collider2D col) {
        base.OnTriggerEnter2D(col);
    }
}
