using UnityEngine;

public class CopyPosRot : MonoBehaviour {
    public Transform target;
    
    private void LateUpdate() {
        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
