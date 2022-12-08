using System;
using UnityEngine;
using UnityEngine.Events;

namespace Architecture {
	public class OnTriggerEvent : MonoBehaviour {
		public UnityEvent<Collider2D> OnTriggerEnter2DEvent;
		public UnityEvent<Collider2D> OnTriggerExit2DEvent;

		private void OnTriggerEnter2D(Collider2D col) {
			OnTriggerEnter2DEvent?.Invoke(col);
		}

		private void OnTriggerExit2D(Collider2D other) {
			OnTriggerExit2DEvent?.Invoke(other);
		}
	}
}