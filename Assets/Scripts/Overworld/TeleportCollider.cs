using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Shows the interactable icon when a player enters the collider
public class TeleportCollider : DialogueCollider {

	Interactable_Teleport interactable;

	void Start () {
		interactable = transform.parent.GetComponent<Interactable_Teleport> ();
	}
		
	public override void OnTriggerEnter (Collider collision) {
		SetIcon (collision, true);
	}

	public override void OnTriggerExit (Collider collision) {
		SetIcon (collision, false);
	}

	public override void SetIcon (Collider collision, bool state) {
		if (collision.tag == "Player") {
			interactable.interactionIcon.SetActive (state);
			interactable.locationName.SetActive (state);
		}
	}

}
