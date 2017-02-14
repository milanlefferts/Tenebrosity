using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BeatGoal : MonoBehaviour {

	RhythmGameController rhythmGameController;

	Vector3 scaleOriginal;
	Vector3 scalePulse;
	Image goalSprite;

	public Sprite spriteBlack;
	public Sprite spriteWhite;

	void Awake () {

	}

	void Start () {
		rhythmGameController = GameObject.FindGameObjectWithTag ("RhythmGameController").GetComponent<RhythmGameController>();


		goalSprite = this.transform.FindChild ("GoalSprite").gameObject.GetComponent<Image> ();
		scaleOriginal = goalSprite.transform.localScale;
		scalePulse = scaleOriginal * 1.25f;
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
		goalSprite.transform.localScale = scalePulse;
	
		yield return new WaitForSeconds (0.1f);
		goalSprite.transform.localScale = scaleOriginal;

	}

	public void Flash () {
		StartCoroutine (FlashImage());

	}

	public IEnumerator FlashImage () {
		//goalSprite.sprite = spriteWhite;
		goalSprite.gameObject.GetComponent<Animator>().SetTrigger("Flash");
		//yield return null;
		yield return new WaitForSeconds (0.1f);
		goalSprite.gameObject.GetComponent<Animator>().SetTrigger("Flash");

		//goalSprite.sprite = spriteBlack;
	}


}
