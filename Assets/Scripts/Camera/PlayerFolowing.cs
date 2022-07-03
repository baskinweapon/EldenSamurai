using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFolowing : MonoBehaviour {
    public Transform target;

    private void LateUpdate() {
        Vector3 pos = target.position;
        pos.z = -1;
        transform.position = pos;
    }
}
