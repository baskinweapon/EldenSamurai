using System;
using Architecture.Interfaces;
using UnityEngine;

public class MoveDirection : MonoBehaviour, IResetAfterDamage {
    public Transform target;
    public Transform mover;
    public float speed;
    
    public Vector3 dir;
    private Damager damager;
    
    private void Awake() {
        damager = GetComponent<Damager>();
        damager.Set(this);
        target = Player.instance.bodyTransform;
    }

    private void OnEnable() {
        if (dir != Vector3.zero) return;
        dir = target.position - transform.position;
        dir.Normalize();
        dir.y = 0;
        if (dir.x < 0) dir.x = -1;
        else dir.x = 1f;
    }

    private void Update() {
        mover.position += speed * Time.deltaTime * dir;
    }

    public void ResetObj() {
        mover.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
}
