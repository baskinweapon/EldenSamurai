using UnityEngine;

public static class DebugExtension {
    
    public static void Log(this Transform t, string message) {
        Debug.Log(message);
    }
    
}
