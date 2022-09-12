using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Expirience : MonoBehaviour {
    [SerializeField]
    private float curExpirience;

    [SerializeField]
    private int currentLevel;
    
    [SerializeField]
    private float needExpToLevel = 100f;

    [SerializeField] 
    private float multiplyExp = 100f;
    
    public UnityEvent<float> OnUpExpirence;
    public UnityEvent<int> OnLevelUp;
    
    public float GetCurrentExpirience() => curExpirience;
    public float GetCurrentLevel() => currentLevel;
    public float GetNeddedExpirience() => needExpToLevel;

    public void ExirienceUp(float value) {
        if (curExpirience + value >= needExpToLevel) {
            var ost = curExpirience + value - needExpToLevel;
            LevelUp();
            curExpirience = ost;
        } else {
            curExpirience += value;
            OnUpExpirence?.Invoke(value);
        }
    }

    public void LevelUp() {
        currentLevel++;
        curExpirience = 0;
        
        //up needed next lvl exp
        needExpToLevel += multiplyExp;
        OnLevelUp?.Invoke(currentLevel);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Expirience))]
public class ExpirienceEditor : Editor {
    public override void OnInspectorGUI() {
        var me = target as Expirience;
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Up 100 xp")) {
            me.ExirienceUp(100f);
        }

        if (GUILayout.Button("Level Up")) {
            me.LevelUp();
        }
        
        EditorGUILayout.EndHorizontal();
    }
}

#endif
