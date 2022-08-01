using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController> {
    
    public void LoadScene(int sceneId) {
        SceneManager.LoadScene(sceneId);
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SceneController))]
public class SceneLoaderEditor : Editor {
    public override void OnInspectorGUI() {
        var me = target as SceneController;
        
        if (GUILayout.Button("Load Test scene")) {
            me.LoadScene(1);
        }
    }
}

#endif
