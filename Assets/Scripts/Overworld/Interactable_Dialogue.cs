using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows for interaction with the object and playing dialogue
public class Interactable_Dialogue : MonoBehaviour {
	// Dialogue
	public GameObject dialogueIcon, dialogue;
	string[] lines;
	bool speaking;

	AudioSource audioSource;

	void Start () {
		audioSource = GetComponent<AudioSource> ();

		// Dialogue
		dialogueIcon = transform.Find ("DialogueIcon").gameObject;
		dialogueIcon.SetActive (false);
		dialogue = transform.Find ("Dialogue").gameObject;
		dialogue.SetActive (false);
	}
	
	void Update () {
		// If player interacts with this character, play dialogue if none is playing
		if (dialogueIcon.activeSelf && !audioSource.isPlaying && !speaking) {
			if (Input.GetKeyDown (KeyCode.E)) {
				StartCoroutine (Speak ());
			}
		}
	}

	// Makes the interactable SPEAK (surprise, surprise)
	IEnumerator Speak () {
		// !!! Assign dialogue at Start() to save the constant search for dialogue options whenever player interacts.
		GetDialogue ();
		speaking = true;
		// Moves through all dialogue lines
		foreach (string line in lines) {
			dialogueIcon.SetActive (false);
			dialogue.GetComponent<TextMesh> ().text = line;
			audioSource.Play (); // currently plays a single grunting noise
			dialogue.SetActive (true);

			yield return new WaitForSeconds(0.1f);

			dialogueIcon.SetActive (true);
			// Wait for player input to continue conversation
			while (!Input.GetKeyDown (KeyCode.E) && audioSource.isPlaying) {
				yield return null;
			}

			dialogue.SetActive (false);

			yield return new WaitForSeconds (0.1f);
		}
		speaking = false;
	}

	// Assigns the dialogue to the interactable. This method will work for small amount of characters however
	// must be stored differently (Excel?) and/or efficiently (database?) when more NPCs are added
	// !!! Missing sound/voices
	void GetDialogue () {
		switch (this.name) {
		case "Angelo": 
			lines = new string[] {"I'm afraid this one\nisn't deep enough", "Guess I'll have to go\na little deeper.."};
			break;
		case "WolfHead": 
			lines = new string[] {"Do not fear me,\ndo not love me", "Feed me and\nreceive your boon"};
			break;
		case "Zomboy": 
			lines =  new string[] {"Have you seen any feet \n lying around?", "Mine is stuck in this\nfence, I could use another"};
			break;
		case "Grave0": 
			lines =  new string[] {"Here lies Roderick, \n a man of few words \n and fewer deeds"};
			break;
		case "Grave1": 
			lines =  new string[] {"RIP Klink\nSkilled at gunning,\nnot so much at running"};
			break;
		case "Grave2": 
			lines =  new string[] {"Who was I?\nI still don't know"};
			break;
		case "Grave3": 
			lines =  new string[] {"Do not fear death\nLook at me\nI accepted it!"};
			break;
		default:
			lines =  new string[] { "Nothing"};
			break;
		}
	}
		
} // End