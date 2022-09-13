using System.Collections;
using Architecture.Interfaces;
using Damage;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damager : BaseDamager {
    [Header("no ended")]
    public bool permanent;
    
    private IResetAfterDamage resetObject;
    public void Set(IResetAfterDamage resetObject) {
        this.resetObject = resetObject;
    }
    
    private void OnEnable() {
        if (permanent) return;
        StopAllCoroutines();
        StartCoroutine(EndProcess());
    }
    
    protected override void OnTriggerEnter2D(Collider2D col) {
        base.OnTriggerEnter2D(col);
        if (col.transform == this.transform.parent) return;
        
        if (resetObject == null) return;
        StopAllCoroutines();
        resetObject.ResetObj();
    }
    
    IEnumerator EndProcess() {
        yield return new WaitForSeconds(duration);
        resetObject?.ResetObj();
        OnEnd?.Invoke();
        gameObject.SetActive(false);
    }
}
