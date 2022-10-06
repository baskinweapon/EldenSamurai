using System;
using System.Collections;
using UnityEditor;
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
    public float GetCurrentManaPercant() => Mathf.InverseLerp(0, maxMana, curMana);
    public float GetMaxMana() => maxMana;
    public float GetPersistantValue() => persistantRestValue;

    public bool SpendMana(float value) {
        if (curMana - value < 0) {
            OnNeedMana?.Invoke();
            return false;
        }
        curMana -= value;
        StopAllCoroutines();
        StartCoroutine(PersistandRest());
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
            curMana = Mathf.Clamp(curMana + persistantRestValue, 0, maxMana);
            OnRest?.Invoke(persistantRestValue);
        }
    }

    public void ChangeMaxMana(float value) {
        maxMana += value;
        StopAllCoroutines();
        StartCoroutine(PersistandRest());
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
            if (me != null) me.SpendMana(10f);
        }

        if (GUILayout.Button("Rest")) {
            if (me != null) me.RestMana(10f);
        }
        

        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Up max Health")) {
            if (me != null) me.ChangeMaxMana(100f);
        }
    }
}

#endif
