using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// When interacting with this teleports player to a new location based on the object's name
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
				SceneManager.LoadScene(teleportLocation);
			}
		}
	}

	// Find location to teleport to based on object's name
	void GetTeleport () {
		switch (this.name) {
		case "Hidden Tomb": 
			teleportLocation = "Graveyard_HiddenTomb";
			locationName.GetComponent<TextMesh> ().text = this.name;
			break;
		case "Graveyard": 
			teleportLocation = "Graveyard";
			locationName.GetComponent<TextMesh> ().text = this.name;
			break;
		case "Kill(ed)Ville": 
			teleportLocation = "KilledVille";
			locationName.GetComponent<TextMesh> ().text = this.name;
			break;
		default:
			teleportLocation = "";
			break;
		}
	}

} // End