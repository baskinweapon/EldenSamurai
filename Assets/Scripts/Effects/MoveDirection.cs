using System;
using Architecture.Interfaces;
using UnityEngine;

public class MoveDirection : MonoBehaviour, IResetAfterDamage {
    public Transform target;
    public float speed;
    
    private Vector3 dir;
    private Damager damager;
    
    private void Awake() {
        damager = GetComponent<Damager>();
        damager.Set(this);
        target = Player.instance.bodyTransform;
    }

    private void OnEnable() {
        dir = target.position - transform.position;
        dir.Normalize();
        dir.y = 0;
        if (dir.x < 0) dir.x = -1;
        else dir.x = 1f;
    }

    private void Update() {
        transform.position += speed * Time.deltaTime * dir;
    }

    public void ResetObj() {
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
}
