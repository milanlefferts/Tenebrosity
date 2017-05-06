using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Sets text and attributes of Text popups when hitting (or missing) BeatIndicators
public class BeatText : MonoBehaviour {
	BeatSpawner beatSpawner;
	Text textComponent;

	void Start () {
		beatSpawner = GameObject.FindGameObjectWithTag ("BeatSpawner").GetComponent<BeatSpawner>();
		this.GetComponent<RectTransform> ().SetParent (beatSpawner.transform, true);
		this.GetComponent<RectTransform> ().localScale = new Vector3 (0.15f, 0.15f, 1f);
		textComponent = this.GetComponent<Text> ();

		StartCoroutine (Implode());
		StartCoroutine (MoveDamage());
	}

	// Destroys object after 0.5 seconds
	IEnumerator Implode () {
		yield return new WaitForSeconds (0.5f);
		Destroy (this.gameObject);
	}

	// Changes position of text, making it float upwards
	IEnumerator MoveDamage() {
		Vector3 startPosition;
		while(true)
		{
			startPosition = transform.position;
			transform.position = Vector3.MoveTowards(startPosition, new Vector3 (startPosition.x, startPosition.y + 50, startPosition.z), 100.0f*Time.deltaTime);
			yield return null;
		}
	}

	// Depending on the player input, set a correct message for the popup text
	public void SetText (int update) {
		string txt = "";

		if (update < 0) {
			// Show Fail message
			txt = "Bad..";
		} else if (update == 1) {
			// Show Okay message
			txt = "Alright";
		} else if (update == 2) {
			// Show Good message
			txt = "Nice";
		} else if (update == 3) {
			// Show Perfect message
			txt = "Ace!";
		}
		textComponent.text = txt;
	}

	// Destroy object on Rhythm Game end
	void OnDisable() {
		Destroy (this.gameObject);
	}

} // End
