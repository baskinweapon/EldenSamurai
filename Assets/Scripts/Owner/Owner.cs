using System;
using UnityEngine;

public class Owner : MonoBehaviour {
    
    private Owner _owner;
    public Owner GetOwner => _owner;

    private void Awake() {
        _owner = GetComponent<Owner>();
    }
}
