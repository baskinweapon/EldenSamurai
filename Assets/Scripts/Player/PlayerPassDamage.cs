using System;
using UnityEngine;

public class PlayerPassDamage : MonoBehaviour {
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private Collider2D colider;
	
	public void PassDamage() {
		var forceVector = new Vector2();
		Collider2D[] cols = new Collider2D[5];
		colider.GetContacts(cols);
		Debug.Log(cols.Length);
		foreach (var col in cols) {
			Debug.Log(col);
			if (col == null) continue;
			if (col.gameObject.layer == LayerMask.GetMask("Damager")) {
				Debug.Log("Pass damage from: " + col.gameObject);
			}
		}
	}
}
