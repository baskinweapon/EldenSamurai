using System.Collections;
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
    public float GetCurrentExpProcent() => Mathf.InverseLerp(0, needExpToLevel, curExpirience);
    public float GetCurrentLevel() => currentLevel;
    public float GetNeddedExpirience() => needExpToLevel;


    private float ostExp;
    public void ExirienceUp(float value) {
        curExpirience += value;
        if (curExpirience >= needExpToLevel) {
            ostExp = needExpToLevel - curExpirience;
            LevelUp();
        } else {
            OnUpExpirence?.Invoke(value);
        }
    }

    public void LevelUp() {
        currentLevel++;

        StartCoroutine(CalcExp());
        //up needed next lvl exp
        needExpToLevel += multiplyExp;
        OnLevelUp?.Invoke(currentLevel);
    }

    IEnumerator CalcExp() {
        yield return new WaitForSeconds(1f);
        curExpirience = ostExp;
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
            if (me != null) me.ExirienceUp(100f);
        }

        if (GUILayout.Button("Level Up")) {
            if (me != null) me.LevelUp();
        }
        
        EditorGUILayout.EndHorizontal();
    }
}

#endif
