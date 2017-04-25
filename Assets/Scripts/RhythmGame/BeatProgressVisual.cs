using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatProgressVisual : MonoBehaviour {
	
	CombatController combatController;
	RhythmGameController rhythmGameController;
	Animator animator;
	public float stateSwitchThreshold;
	public float stateSwitchThresholdTotal;
	float stateS;
	bool firstHit;
	bool isReset;

	void Start () {
		rhythmGameController = GameObject.FindGameObjectWithTag ("RhythmGameController").GetComponent<RhythmGameController>();
		combatController = GameObject.FindGameObjectWithTag ("CombatController").GetComponent<CombatController> ();
		animator = transform.FindChild ("Sprite").gameObject.GetComponent<Animator> ();
		isReset = false;
		firstHit = true;
		stateSwitchThreshold = 0;
		stateSwitchThresholdTotal = 0;

	}
	
	void Update () {
		//transform.FindChild ("Sprite").gameObject.GetComponent<RectTransform>().Rotate(Vector3.zero);
		// If object is active in hierarchy
		// a significant changes in beatHits cause a switch in animation
		if (this.gameObject.activeSelf) {
			if (rhythmGameController.beatHits > stateSwitchThresholdTotal) {
				if (firstHit) {
					stateSwitchThreshold = rhythmGameController.beatHitsRequired / rhythmGameController.beatVisualAnimationStates;
					firstHit = false;
				}
				animator.SetTrigger ("Next");
				stateSwitchThresholdTotal = stateSwitchThresholdTotal + stateSwitchThreshold;
			}
		} 
		// set rotation to 0
		if (!isReset && transform.FindChild ("Sprite").gameObject.GetComponent<RectTransform> ().eulerAngles != Vector3.zero) {
			transform.FindChild ("Sprite").gameObject.GetComponent<RectTransform> ().eulerAngles = Vector3.zero;
			isReset = true;

		}
	}

	void OnEnable () {
		firstHit = true;
		isReset = false;

		stateSwitchThreshold = 0;
		stateSwitchThresholdTotal = 0;

		if (combatController.activeCharacter.tag == "PC") {
			this.gameObject.GetComponentInChildren<Image> ().color = new Color32 (255, 255, 255, 255);
			animator = transform.FindChild ("Sprite").gameObject.GetComponent<Animator> ();

		} else {
			
			this.gameObject.GetComponentInChildren<Image> ().color = new Color32 (255, 255, 255, 0);
			animator = rhythmGameController.beatGoal.GetComponent<BeatGoal>().goalSprite.gameObject.GetComponent<Animator> ();
			//			goalSprite.gameObject.GetComponent<Animator> ().SetTrigger ("PlayerTurn");

		}


	}

	void OnDisable () {
		// Reset position
	}
}
