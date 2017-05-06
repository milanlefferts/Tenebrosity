using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Sets the text and pulsing animation of the Rhythm Game's intro screen
public class BeatKeyPressText : MonoBehaviour {
	private RhythmGameController rhythmGameController;
	private CombatController combatController;

	Vector3 scaleOriginal, scalePulse;

	public Sprite spriteBlack;
	public Sprite spriteWhite;

	void Start () {
		rhythmGameController = GameObject.FindGameObjectWithTag ("RhythmGameController").GetComponent<RhythmGameController>();
		combatController = GameObject.FindGameObjectWithTag ("CombatController").GetComponent<CombatController> ();

		scaleOriginal = this.transform.localScale;
		scalePulse = scaleOriginal * 1.1f;
	}

	void OnDisable () {
		rhythmGameController.BeatEventVisual.RemoveListener (Pulse);
	}

	void OnEnable () {
		rhythmGameController.BeatEventVisual.AddListener (Pulse);
	}

	void Pulse () {
		StartCoroutine (PulseImage());
	}

	IEnumerator PulseImage () {
		this.transform.localScale = scalePulse;
		yield return new WaitForSeconds (0.1f);
		this.transform.localScale = scaleOriginal;
	}

	public void ChangeState(string character) {
		if (character == "NPC") {
			this.GetComponent<Text> ().text = "Defend!";
		} else {
			this.GetComponent<Text> ().text = combatController.selectedAbility.name;
		}
	}
}
