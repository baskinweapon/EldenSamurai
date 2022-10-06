using UnityEditor;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour {
    private void Start() {
        Player.instance.bodyTransform.position = transform.position;
    }

    public void TeleportToSpawnPoint() {
        Player.instance.bodyTransform.position = transform.position;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PlayerSpawnPoint))]
public class PlayerSpawnPointEditor : Editor {
    public override void OnInspectorGUI() {
        var me = target as PlayerSpawnPoint;
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Teleport")) {
            if (me != null) me.TeleportToSpawnPoint();
        }
    }
}

#endif
