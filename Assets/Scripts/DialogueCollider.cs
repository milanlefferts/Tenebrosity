﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCollider : MonoBehaviour {

	Interactable interactable;

	// Use this for initialization
	void Start () {
		interactable = transform.parent.GetComponent<Interactable> ();
	}
	// Update is called once per frame
	void Update () {
		
	}
		
	void OnTriggerEnter (Collider collision) {
		if (collision.tag == "Player") {
			interactable.dialogueIcon.SetActive (true);

		}
	}

	void OnTriggerExit (Collider collision) {
		if (collision.tag == "Player") {
			interactable.dialogueIcon.SetActive (false);

		}
	}


}
