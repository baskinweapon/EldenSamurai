using System;
using UnityEngine;

public class StoneDialog : MonoBehaviour {
    
    public GameObject dialog;
    
    private void OnTriggerEnter2D(Collider2D col) {
        dialog.SetActive(true);    
    }

    private void OnTriggerExit2D(Collider2D other) {
        dialog.SetActive(false);
    }
}
