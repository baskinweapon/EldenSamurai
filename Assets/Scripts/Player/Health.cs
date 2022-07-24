using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {
    [SerializeField]
    private float curHealth;
    [SerializeField]
    private float maxHealth = 100f;
    
    public UnityEvent<float> OnDamage;
    public UnityEvent<float> OnHeal;
    public UnityEvent OnDeath;
    public Action OnChangeMaxHealth;

    public float GetCurrentHealth() => curHealth;
    public float GetMaxHealth() => maxHealth;
    
    public void Damage(float value) {
        curHealth -= value;
        if (curHealth <= 0) curHealth = 0;
        OnDamage?.Invoke(curHealth);
        if (curHealth <= 0) 
            Death();
    }
    
    public void Heal(float value) {
        curHealth = Mathf.Clamp(curHealth + value, 0, maxHealth);
        OnHeal?.Invoke(value);
    }

    public void ChangeMaxHealth(float value) {
        maxHealth += value;
        OnChangeMaxHealth?.Invoke();
    }

    public void Death() {
        Debug.Log("Death");
        OnDeath?.Invoke();
    }
    
}

#if UNITY_EDITOR
[CustomEditor(typeof(Health))]
public class HealthEditor : Editor {
    public override void OnInspectorGUI() {
        var me = target as Health;
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Damage")) {
            me.Damage(10f);
        }

        if (GUILayout.Button("Heal")) {
            me.Heal(10f);
        }


        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        
        if (GUILayout.Button("Death")) {
            me.Death();
        }
        
    }
}

#endif
