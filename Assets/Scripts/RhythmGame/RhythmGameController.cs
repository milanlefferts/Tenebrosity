using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RhythmGameController : MonoBehaviour {
	// Tempo
	public UnityEvent BeatEvent;
	public UnityEvent BeatEventVisual;


	public float bpm;
	public float tempo;
	public float beatTravelSpeed;
	public float beatSpawnSpeed;
	public int beatSpawnedTotal;

	// Nr to determine end of turn / failure
	public int beatsPassed;

	public float beatVisualAnimationStates;


	// Rhythm Game Success
	public float beatHits;
	public float beatHitsRequired;
	//AbilityController abilityController;
	CombatController combatController;

	public float buttonPressed;
	public float buttonWindow;

	// Visual

	public GameObject beatProgressVisual;

	public GameObject beatText;

	public GameObject beatGoal;
	GameObject beatInterface;
	//BeatSpawner beatSpawner;

	public bool turnEnded;

	void Awake () {
		BeatEvent = new UnityEvent ();
		BeatEventVisual = new UnityEvent ();

	}

	void Start () {
		// Tempo
		bpm = 115f;
		tempo = 60f / bpm;

		beatSpawnSpeed = 1f;
		beatTravelSpeed = 160f;

		StartCoroutine(spawnOnBPM());
		StartCoroutine(BPM());

		// 
		buttonWindow = 0.3f;

		turnEnded = true;
		// Rhythm Game Success
		//abilityController = GameObject.FindGameObjectWithTag ("AbilityController").GetComponent<AbilityController> ();
		combatController = GameObject.FindGameObjectWithTag ("CombatController").GetComponent<CombatController> ();
		beatGoal = GameObject.FindGameObjectWithTag ("BeatGoal");
		beatInterface = GameObject.FindGameObjectWithTag ("BeatInterface");
		beatProgressVisual = GameObject.FindGameObjectWithTag ("BeatProgressVisual");

		//beatSpawner = GameObject.FindGameObjectWithTag ("BeatSpawner").GetComponent<BeatSpawner>();
		beatInterface.SetActive (false);

	}

	void Update() {
		if (beatsPassed >= beatSpawnedTotal && !turnEnded) {

			turnEnded = true;
			// FAILED
			if (combatController.activeCharacter.tag == "PC") {
				StartCoroutine(combatController.EndPlayerTurn());

			} else {
				combatController.selectedAbility.effect();

			}

		}
	}

	public IEnumerator spawnOnBPM () {
		while(true) {
			BeatEvent.Invoke ();
			yield return new WaitForSeconds(tempo / beatSpawnSpeed);
		}

	}

	public IEnumerator BPM () {
		while(true) {
			BeatEventVisual.Invoke ();
			yield return new WaitForSeconds(tempo);
		}

	}



	public void UpdateBeatHits (int update) {


		// Show Beat Text feedback
		GameObject beatTxt = Instantiate (beatText, beatGoal.transform.position, this.transform.rotation);
		beatTxt.GetComponent<BeatText>().SetText(update);

		if (update > 0) {
			beatHits += 1;
			//beatHits += update;

		} else {
			//beatHits -= 1;
		}

		if (beatHits < 0) {
			beatHits = 0;
		}
				
		if (beatHits >= beatHitsRequired) {
			if (!turnEnded) {
				// Use Ability
				combatController.selectedAbility.effect();
				turnEnded = true;
			}
		
		}

	}
		
	public IEnumerator FadeRhythmGame(bool fadeIn) {
		float fadeVal;
		if (fadeIn) {
			fadeVal = -0.2f;
		} else {
			fadeVal = 0.2f;
		}

		GameObject[] backdrop = GameObject.FindGameObjectsWithTag ("Backdrop");

		for (int i = 0; i < 5; i++) {
			foreach (GameObject obj in backdrop) {
				Color oldColor = obj.GetComponent<SpriteRenderer> ().color;
				Color newColor = new Color (1, 1, 1, oldColor.a + fadeVal);
				obj.GetComponent<SpriteRenderer> ().color = newColor;
			}
			yield return new WaitForSeconds (0.2f);
		}
	}





}
