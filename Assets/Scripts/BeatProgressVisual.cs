﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatProgressVisual : MonoBehaviour {
	
	//CombatController combatController;
	RhythmGameController rhythmGameController;
	Animator animator;
	public float stateSwitchThreshold;
	public float stateSwitchThresholdTotal;
	float stateS;
	bool firstHit;

	void Start () {
		rhythmGameController = GameObject.FindGameObjectWithTag ("RhythmGameController").GetComponent<RhythmGameController>();
		//combatController = GameObject.FindGameObjectWithTag ("CombatController").GetComponent<CombatController> ();
		animator = transform.FindChild ("Sprite").gameObject.GetComponent<Animator> ();

		firstHit = true;
		stateSwitchThreshold = 0;
		stateSwitchThresholdTotal = 0;

	}
	
	void Update () {
		// If object is active in hierarchy
		// a significant changes in beatHits cause a switch in animation
		if (this.gameObject.activeSelf) {
			if (rhythmGameController.beatHits > stateSwitchThresholdTotal) {
				if (firstHit) {
					stateSwitchThreshold = rhythmGameController.beatHitsRequired / rhythmGameController.beatVisualAnimationStates;
					firstHit = false;
				}
				animator.SetTrigger("Next");
				stateSwitchThresholdTotal = stateSwitchThresholdTotal + stateSwitchThreshold;
			} /*
			else if (rhythmGameController.beatHits < stateSwitchThreshold) {
				animator.SetTrigger("Previous");
				stateSwitchThreshold = stateSwitchThreshold / 2;
			}*/
		}

	}

	void OnEnable () {
		firstHit = true;
		stateSwitchThreshold = 0;
		stateSwitchThresholdTotal = 0;

		// Sets new threshold for animation switching
		//stateSwitchThreshold = rhythmGameController.beatHitsRequired / rhythmGameController.beatVisualAnimationStates;
		//stateSwitchThreshold = rhythmGameController.beatHitsRequired / rhythmGameController.beatVisualAnimationStates;

	}

	void OnDisable () {

	}
}
