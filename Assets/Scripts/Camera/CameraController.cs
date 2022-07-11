using System;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;
    
    private Vector3 startPos;
    private void Start() {
        startPos = transform.position;
    }

    private void LateUpdate() {
        Vector3 pos = target.position;
        pos.z = -10;
        transform.position = startPos + pos;
    }
}
