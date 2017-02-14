using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BeatKeyPressText : MonoBehaviour {
	RhythmGameController rhythmGameController;
	CombatController combatController;

	Vector3 scaleOriginal;
	Vector3 scalePulse;

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
			//this.GetComponent<Text> ().text = combatController.selectedAbility.name;
		}
	}
}
