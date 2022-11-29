using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {
    [SerializeField][HideInInspector]
    private float curHealth = 100f;
    [SerializeField][HideInInspector]
    private float maxHealth = 100f;

    [HideInInspector]
    public float persistantHealValue = 0.5f;
    
    public UnityEvent<float> OnDamage;
    public UnityEvent<float> OnHeal;
    public UnityEvent OnDeath;
    public UnityEvent<float> OnChangeMaxHealth;

    public float GetCurrentHealth() => curHealth;
    public float GetCurrentHPPercent() => Mathf.InverseLerp(0, maxHealth, curHealth);
    public float GetMaxHealth() => maxHealth;
    public float GetPersistantValue() => persistantHealValue;
    
    public void Damage(float value) {
        if (curHealth == 0) return;
        curHealth -= value;
        if (curHealth <= 0) curHealth = 0;
        if (curHealth <= 0) {
            Death();
            return;
        }
        OnDamage?.Invoke(curHealth);
        StopAllCoroutines();
        StartCoroutine(PersistandHeal());
    }
    
    public void Heal(float value) {
        curHealth = Mathf.Clamp(curHealth + value, 0, maxHealth);
        OnHeal?.Invoke(value);
    }

    public void Death() {
        Debug.Log("Death");
        StopAllCoroutines();
        OnDeath?.Invoke();
    }
    
    IEnumerator PersistandHeal() {
        if (persistantHealValue == 0) yield break;
        while (curHealth <= maxHealth) {
            yield return new WaitForSeconds(1f);
            curHealth = Mathf.Clamp(curHealth + persistantHealValue, 0, maxHealth);
            OnHeal?.Invoke(persistantHealValue);
        }
    }

    public void ChangeMaxHealth(float value) {
        maxHealth += value;
        StopAllCoroutines();
        StartCoroutine(PersistandHeal());
        OnChangeMaxHealth?.Invoke(value);
    }
}

// #if UNITY_EDITOR
// [CustomEditor(typeof(Health))]
// public class HealthEditor : Editor {
//     public override void OnInspectorGUI() {
//         var me = target as Health;
//         base.OnInspectorGUI();
//
//         EditorGUILayout.BeginHorizontal();
//         if (GUILayout.Button("Damage")) {
//             if (me != null) me.Damage(10f);
//         }
//
//         if (GUILayout.Button("Heal")) {
//             if (me != null) me.Heal(10f);
//         }
//         
//         EditorGUILayout.EndHorizontal();
//
//         EditorGUILayout.Space();
//         
//         if (GUILayout.Button("Up max Health")) {
//             if (me != null) me.ChangeMaxHealth(100f);
//         }
//         
//         EditorGUILayout.Space();
//         
//         if (GUILayout.Button("Death")) {
//             if (me != null) me.Death();
//         }
//         
//     }
// }
//
// #endif
