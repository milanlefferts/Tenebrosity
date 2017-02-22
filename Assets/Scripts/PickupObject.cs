using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour {
	PlayerController player;
	SpriteRenderer spriteRenderer;
	GameController gameController;
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();

		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		spriteRenderer = this.GetComponentInChildren<SpriteRenderer> ();
	}

	void OnTriggerEnter (Collider collision) {
		if (collision.tag == "Player") {
			this.GetComponent<BoxCollider>().enabled = false;
			StartCoroutine(PickedupObject ());
		}
	}

	IEnumerator PickedupObject() {
		
		player.item.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
		player.animator.SetTrigger ("Pickup");
		spriteRenderer.sprite = null;
		gameController.PickupObject (this.name);
		gameController.PausePlayer ();

		yield return new WaitForSeconds (0.5f);
		player.item.GetComponent<SpriteRenderer> ().sprite = null;

		//yield return new WaitForSeconds (1f);
		gameController.PausePlayer ();
		Destroy (this.gameObject);
	}
}
