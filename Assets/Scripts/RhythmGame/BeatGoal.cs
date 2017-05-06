using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Controls the target for all beats/notes visually
public class BeatGoal : MonoBehaviour {

	private RhythmGameController rhythmGameController;
	private CombatController combatController;

	private Vector3 scaleOriginal, scalePulse;
	public Image goalSprite;

	public Sprite spriteBlack;
	public Sprite spriteWhite;

	void Start () {
		rhythmGameController = GameObject.FindGameObjectWithTag ("RhythmGameController").GetComponent<RhythmGameController>();
		combatController = GameObject.FindGameObjectWithTag ("CombatController").GetComponent<CombatController> ();

		goalSprite = this.transform.FindChild ("GoalSprite").gameObject.GetComponent<Image> ();
		scaleOriginal = goalSprite.transform.localScale;
		scalePulse = scaleOriginal * 1.25f;
	}

	void OnDisable () {
		// Removes Event when Rhythm Game is inactive
		rhythmGameController.BeatEventVisual.RemoveListener (Pulse);
	}

	void OnEnable () {
		// Adds Event when Rhythm Game is active
		rhythmGameController.BeatEventVisual.AddListener (Pulse);
		if (combatController.activeCharacter.tag == "PC") {
			SetAnimation ("PlayerTurn");
		} else {
			SetAnimation ("Defend");
		}
	}

	// Wrapper method that starts coroutine to pulse the BeatGoal sprite
	void Pulse () {
		StartCoroutine (PulseImage());
	}

	IEnumerator PulseImage () {
		goalSprite.transform.localScale = scalePulse;
		yield return new WaitForSeconds (0.1f);
		goalSprite.transform.localScale = scaleOriginal;
	}

	public void Flash () {
		StartCoroutine (FlashImage());
	}

	public IEnumerator FlashImage () {
		//goalSprite.sprite = spriteWhite;
		SetAnimation ("Flash");		//yield return null;
		yield return new WaitForSeconds (0.1f);
		SetAnimation ("Flash");
	}

	private void SetAnimation(string anim) {
		goalSprite.gameObject.GetComponent<Animator> ().SetTrigger (anim);
	}

} // End