using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatIndicatorCollider : MonoBehaviour {

	BeatIndicator beatIndicator;
	// Use this for initialization
	void Start () {
		beatIndicator = transform.parent.gameObject.GetComponent<BeatIndicator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.name == this.name) {
			beatIndicator.SetTouchingTrue(this.name);
		}
	}

	void OnTriggerExit (Collider collider) {
		if (collider.name == this.name) {
			beatIndicator.SetTouchingFalse(this.name);
		}
	}
}
