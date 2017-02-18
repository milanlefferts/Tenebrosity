using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCollider : MonoBehaviour {

	Interactable_Teleport interactable;

	void Start () {
		interactable = transform.parent.GetComponent<Interactable_Teleport> ();

	}
		
	void OnTriggerEnter (Collider collision) {
		if (collision.tag == "Player") {
			interactable.interactionIcon.SetActive (true);
			interactable.locationName.SetActive (true);
		}
	}

	void OnTriggerExit (Collider collision) {
		if (collision.tag == "Player") {
			interactable.interactionIcon.SetActive (false);
			interactable.locationName.SetActive (false);

		}
	}


}
