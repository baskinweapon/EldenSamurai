using System;
using UnityEngine;

public class MoveDirection : MonoBehaviour {
    public float speed;
    private int sign = 1;
    
    public void SetSign(int _sign) {
        sign = _sign;
    }
    
    private void Update() {
        transform.position += sign * speed * Time.deltaTime * Vector3.right;
    }
    
}
