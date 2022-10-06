using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes {
    MAIN,
    HELL,
    TEAHOUSE,
    TEST
}

public class SceneController : Singleton<SceneController> {
    
    public void LoadScene(int sceneId) {
        SceneManager.LoadScene(sceneId);
    }
    
    public void LoadScene(Scenes sceneId) {
        SceneManager.LoadScene((int)sceneId);
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SceneController))]
public class SceneLoaderEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        var me = target as SceneController;
        
        if (GUILayout.Button("Load Test scene")) {
            if (me != null) me.LoadScene(Scenes.TEST);
        } 
        if (GUILayout.Button("Load Hell scene")) {
            if (me != null) me.LoadScene(Scenes.HELL);
        }
    }
}

#endif
