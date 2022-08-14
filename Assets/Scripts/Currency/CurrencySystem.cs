using System;
using Currency;
using UnityEditor;
using UnityEngine;

namespace Currency {
    public class CurrencySystem : Singleton<CurrencySystem> {
        [SerializeField] private int currentMoney = 100;

        public Action<int> OnSpendMoney;
        public Action<int> OnAddMoney;
        
        // send how money need
        public Action<int> OnNoEnoghtMoney;

        public int GetCurrentMoney() => currentMoney;

        public void AddMoney(int value) {
            currentMoney += value;
            OnAddMoney?.Invoke(value);
        }

        public bool TrySpendMoney(int value) {
            if (currentMoney >= value) {
                SpendMoney(value);
                return true;
            }
            OnNoEnoghtMoney?.Invoke(Mathf.Abs(currentMoney - value));
            return false;
        }
        
        private void SpendMoney(int value) {
            currentMoney -= value;
            OnSpendMoney?.Invoke(value);
            
        }  

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CurrencySystem))]
public class CurrencySystemEditor : Editor {
    public override void OnInspectorGUI() {
        var me = target as CurrencySystem;
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Money")) {
            me.AddMoney(10);
        }

        if (GUILayout.Button("Spend Money")) {
            me.TrySpendMoney(10);
        }
        
        EditorGUILayout.EndHorizontal();
    }
}

#endif