using System;
using UnityEngine;
using UnityEngine.WSA;

public enum SlotsType {
	consumables,
	stats,
	updates
}

namespace Pedestal {
	
	public class Pedestal : MonoBehaviour {

		public SlotsType type;
		public GameObject UIpanel;

		public GameObject leftSlot;
		public GameObject rightSlot;

		public void Triggered(Collider2D col) {
			if (col.gameObject.layer == LayerMask.NameToLayer("Player")) {
				Activate();
			}
		}
		
		public void TriggerExit(Collider2D col) {
			if (col.gameObject.layer == LayerMask.NameToLayer("Player")) {
				Disactivate();
			}
		}

		public void Activate() {
			
			UIpanel.SetActive(true);
			leftSlot.SetActive(true);
			rightSlot.SetActive(true);
		}
		
		public void Disactivate() {
			
			UIpanel.SetActive(false);
			leftSlot.SetActive(false);
			rightSlot.SetActive(false);
		}

	}
	
}