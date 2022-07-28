using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.Events;

public class Mana : MonoBehaviour
{
    [SerializeField]
    private float curMana;
    [SerializeField]
    private float maxMana;

    [SerializeField] 
    private float persistantRestValue = 0.5f;
    
    public UnityEvent<float> OnSpend;
    public UnityEvent<float> OnRest;
    public UnityEvent OnNeedMana;

    public Action OnChangeMaxMana;
    
    public float GetCurrentMana() => curMana;
    public float GetMaxMana() => maxMana;

    public bool SpendMana(float value) {
        curMana -= value;
        StopAllCoroutines();
        StartCoroutine(PersistandRest());
        if (curMana < 0) {
            OnNeedMana?.Invoke();
            curMana = 0;
            return false;
        } 
        OnSpend?.Invoke(value);
        return true;
    }

    public void RestMana(float value) {
        curMana = Mathf.Clamp(curMana + value, 0, maxMana);
        OnRest?.Invoke(value);
    }
    
    IEnumerator PersistandRest() {
        while (curMana <= maxMana) {
            yield return new WaitForSeconds(1f);
            curMana += persistantRestValue;
            OnRest?.Invoke(persistantRestValue);
        }
    }

    public void ChangeMaxMana(float value) {
        maxMana += value;
        OnChangeMaxMana?.Invoke();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Mana))]
public class ManaEditor : Editor {
    public override void OnInspectorGUI() {
        var me = target as Mana;
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Spend")) {
            me.SpendMana(10f);
        }

        if (GUILayout.Button("Rest")) {
            me.RestMana(10f);
        }
        
        EditorGUILayout.EndHorizontal();
    }
}

#endif
