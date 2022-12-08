using System;
using System.Collections;
using UnityEngine;

namespace Levels.LevelLogic {
	public class OutOfZone : MonoBehaviour {
		public Transform resetPlace;
		public float waitToReset;

		private void OnTriggerEnter2D(Collider2D col) {
			if (col.gameObject.layer == LayerMask.NameToLayer("Player")) {
				StartCoroutine(ToResetPosition());
			}
		}

		IEnumerator ToResetPosition() {
			yield return new WaitForSeconds(waitToReset);
			Player.instance.rb.position = resetPlace.position;
		}
	}
}