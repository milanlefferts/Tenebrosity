using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to objects that can be picked up by the player in the overworld
public class PickupObject : MonoBehaviour {
	
	private PlayerController player;
	private SpriteRenderer spriteRenderer;
	private GameController gameController;

	public float freezeTimer;

	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		spriteRenderer = this.GetComponentInChildren<SpriteRenderer> ();

		freezeTimer = 0.5f;
	}

	void OnTriggerEnter (Collider collision) {
		if (collision.tag == "Player") {
			this.GetComponent<BoxCollider>().enabled = false;
			StartCoroutine(PickedupObject ());
		}
	}

	// Freezes the player until the pickup animation has finished playing
	IEnumerator PickedupObject() {
		// Sets the item's  sprite as the player's item slot sprite
		player.item.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
		player.animator.SetTrigger ("Pickup");
		spriteRenderer.sprite = null;
		// Adds the object to the player's inventory
		gameController.PickupObject (this.name);
		gameController.PausePlayer ();

		yield return new WaitForSeconds (freezeTimer);
		player.item.GetComponent<SpriteRenderer> ().sprite = null;
		gameController.PausePlayer ();
		Destroy (this.gameObject);
	}
}
