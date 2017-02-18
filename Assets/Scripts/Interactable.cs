using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

	public GameObject dialogueIcon;
	public GameObject dialogue;
	AudioSource audioSource;

	void Start () {

		audioSource = GetComponent<AudioSource> ();
		dialogueIcon = transform.Find ("DialogueIcon").gameObject;
		dialogueIcon.SetActive (false);

		dialogue = transform.Find ("Dialogue").gameObject;
		dialogue.SetActive (false);
	}
	
	void Update () {
		// If player interacts with this character, play dialogue
		if (dialogueIcon.activeSelf && !audioSource.isPlaying) {
			if (Input.GetKeyDown (KeyCode.E)) {
				StartCoroutine (Speak ());
			}
		}


	}

	IEnumerator Speak () {
		dialogueIcon.SetActive (false);
		GetDialogue ();
		dialogue.SetActive (true);
		audioSource.Play ();

		//while (audioSource.isPlaying) {
		//	yield return null;
		//}

		yield return new WaitForSeconds (1.5f);

		dialogue.SetActive (false);
	}

	void GetDialogue () {
		string words;
		switch (this.name) {
		case "Lakedweller": 
			words = "Oh it's you again..";
			// assign sound
			break;
		case "Bob": 
			words = "Have you seen any feet \n lying around?";
			// assign sound
			break;
		case "Grave": 
			words = "Here lies Roderick, \n a man of few words \n and fewer deeds";
			// assign sound
			break;
		default:
			words = "Nothing";
			// assign sound
			break;
		}

		dialogue.GetComponent<TextMesh> ().text = words;
		// assign sound

	}



}
