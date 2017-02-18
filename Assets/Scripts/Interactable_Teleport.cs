using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable_Teleport : MonoBehaviour {

	public GameObject interactionIcon;
	public GameObject locationName;
	string teleportLocation;

	bool speaking;

	void Start () {

		interactionIcon = transform.Find ("InteractionIcon").gameObject;
		interactionIcon.SetActive (false);
		locationName = transform.Find ("LocationName").gameObject;
		locationName.SetActive (false);
		GetTeleport ();


	}
	
	void Update () {
		if (interactionIcon.activeSelf) {
			if (Input.GetKeyDown (KeyCode.E)) {
				//StartCoroutine (Speak ());
				SceneManager.LoadScene(teleportLocation);
			}
		}


	}


	void GetTeleport () {
		switch (this.name) {
		case "Hidden Tomb": 
			teleportLocation = "HiddenTomb";
			locationName.GetComponent<TextMesh> ().text = this.name;

			break;
		default:
			teleportLocation = "";

			break;
		}

	}



}
