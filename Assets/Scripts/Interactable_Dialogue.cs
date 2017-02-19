using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Dialogue : MonoBehaviour {

	public GameObject dialogueIcon;
	public GameObject dialogue;
	AudioSource audioSource;

	string[] lines;

	bool speaking;

	void Start () {

		audioSource = GetComponent<AudioSource> ();
		dialogueIcon = transform.Find ("DialogueIcon").gameObject;
		dialogueIcon.SetActive (false);

		dialogue = transform.Find ("Dialogue").gameObject;
		dialogue.SetActive (false);

	}
	
	void Update () {
		// If player interacts with this character, play dialogue
		if (dialogueIcon.activeSelf && !audioSource.isPlaying && !speaking) {
			if (Input.GetKeyDown (KeyCode.E)) {
				StartCoroutine (Speak ());
			}
		}


	}


	IEnumerator Speak () {

		GetDialogue ();

		speaking = true;

		// Moves through all dialogue
		foreach (string line in lines) {
			// deactivate dialogue icon
			dialogueIcon.SetActive (false);

			// set new line as text
			dialogue.GetComponent<TextMesh> ().text = line;
			audioSource.Play ();
			dialogue.SetActive (true);

			yield return new WaitForSeconds(0.1f);

			dialogueIcon.SetActive (true);
			// wait for player input to continue conversation
			while (!Input.GetKeyDown (KeyCode.E) && audioSource.isPlaying) {
				yield return null;
			}

			dialogue.SetActive (false);

		
			yield return new WaitForSeconds (0.1f);

		}

		speaking = false;

	}

	void GetDialogue () {
		string[] words;
		switch (this.name) {
		case "Angelo": 
			lines = new string[] {"I'm afraid this one\nisn't deep enough", "Guess I'll have to go\na little deeper.."};
			// assign sound
			break;
		case "WolfHead": 
			lines = new string[] {"Do not fear me,\ndo not love me", "Feed me and\nreceive your boon"};
			// assign sound
			break;
		case "Zomboy": 
			lines =  new string[] {"Have you seen any feet \n lying around?", "Mine is stuck in this\nfence, I could use another"};
			// assign sound
			break;
		case "Grave0": 
			lines =  new string[] {"Here lies Roderick, \n a man of few words \n and fewer deeds"};
			// assign sound
			break;
		case "Grave1": 
			lines =  new string[] {"RIP Klink\nSkilled at gunning,\nnot so much at running"};
			// assign sound
			break;
		case "Grave2": 
			lines =  new string[] {"Who was I?\nI still don't know"};
			// assign sound
			break;
		case "Grave3": 
			lines =  new string[] {"Do not fear death\nLook at me\nI accepted it!"};
			// assign sound
			break;
		default:
			lines =  new string[] { "Nothing"};
			// assign sound
			break;
		}

	}



}
