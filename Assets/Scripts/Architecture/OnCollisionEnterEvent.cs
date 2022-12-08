using UnityEngine;
using UnityEngine.Events;

namespace Architecture {
	public class OnCollisionEnterEvent : MonoBehaviour {

		public UnityEvent<Collision2D> OnCollissionEnter2D;

		private void OnCollisionEnter2D(Collision2D col) {
			OnCollissionEnter2D?.Invoke(col);
		}
	}
}