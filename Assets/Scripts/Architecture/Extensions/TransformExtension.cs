using UnityEngine;

public static class TransformExtension {
    public static void ChangePosition(this Transform t, float x = 10000f, float y = 10000f, float z = 10000f) {
        if (x == 10000f) x = t.position.x;
        if (y == 10000f) y = t.position.y;
        if (z == 10000f) z = t.position.z;
        t.position = new Vector3(x, y, z);
    }
    
    public static void ChangeScale(this Transform t, float x = 10000f, float y = 10000f, float z = 10000f) {
        if (x == 10000f) x = t.localScale.x;
        if (y == 10000f) y = t.localScale.y;
        if (z == 10000f) z = t.localScale.z;
        t.localScale = new Vector3(x, y, z);
    }
}
