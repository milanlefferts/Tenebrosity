using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script shows the Dialogue Icon when a player is within the collider
public class DialogueCollider : MonoBehaviour {

	Interactable_Dialogue interactable;

	void Start () {
		interactable = transform.parent.GetComponent<Interactable_Dialogue> ();
	}
		
	public virtual void OnTriggerEnter (Collider collision) {
		SetIcon (collision, true);
	}

	public virtual void OnTriggerExit (Collider collision) {
		SetIcon (collision, false);
	}

	public virtual void SetIcon(Collider collision, bool state) {
		if (collision.tag == "Player") {
			interactable.dialogueIcon.SetActive (state);
		}
	}

}
