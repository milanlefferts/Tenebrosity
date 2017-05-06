using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Separate class for the BeatIndicator/note collider
public class BeatIndicatorCollider : MonoBehaviour {
	private BeatIndicator beatIndicator;

	void Start () {
		beatIndicator = transform.parent.gameObject.GetComponent<BeatIndicator> ();
	}

	// Set collider touching to true
	void OnTriggerEnter (Collider collider) {
		if (collider.name == this.name) {
			beatIndicator.SetTouchingTrue(this.name);
		}
	}

	// Set collider touching to false
	void OnTriggerExit (Collider collider) {
		if (collider.name == this.name) {
			beatIndicator.SetTouchingFalse(this.name);
		}
	}
}
