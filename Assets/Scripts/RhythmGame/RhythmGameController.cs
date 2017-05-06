using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Controls the Rhythm Game and it's UI, as well as the general tempo/bpm of beat indicators
public class RhythmGameController : MonoBehaviour {
	// Tempo
	public UnityEvent BeatEvent, BeatEventVisual;

	[SerializeField]
	private float bpm, tempo;
	[HideInInspector]
	public float beatTravelSpeed, beatSpawnSpeed;
	public int beatSpawnedTotal;

	// Nr to determine end of turn / failure
	public int beatsPassed;
	[HideInInspector]
	public float beatVisualAnimationStates;

	// Rhythm Game Success
	public float beatHits, beatHitsRequired;
	private CombatController combatController;

	public float buttonPressed, buttonWindow;

	// Visual
	public GameObject beatProgressVisual;
	public GameObject beatText;
	public GameObject beatGoal;
	private GameObject beatInterface;

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

		StartCoroutine(OnBPM(BeatEvent, beatSpawnSpeed));
		StartCoroutine(OnBPM(BeatEventVisual, 0f));

		buttonWindow = 0.3f;

		turnEnded = true;
		// Rhythm Game Success
		combatController = GameObject.FindGameObjectWithTag ("CombatController").GetComponent<CombatController> ();
		beatGoal = GameObject.FindGameObjectWithTag ("BeatGoal");
		beatInterface = GameObject.FindGameObjectWithTag ("BeatInterface");
		beatProgressVisual = GameObject.FindGameObjectWithTag ("BeatProgressVisual");
		beatInterface.SetActive (false);
	}

	void Update() {
		if (beatsPassed >= beatSpawnedTotal && !turnEnded) {
			turnEnded = true;
			// Failed
			if (combatController.activeCharacter.tag == "PC") {
				StartCoroutine(combatController.EndPlayerTurn());
			} else {
				combatController.selectedAbility.effect();
			}
		}
	}

	// Default: Spawns a new beat indicator on every Beat
	// If beatspawn equals 0: Pulses the beat target in the middle of the screen on each beat
	public IEnumerator OnBPM (UnityEvent uevent, float beatspawn) {
		if (beatspawn == 0f) {
			beatspawn = 1f;
		}
		while(true) {
			uevent.Invoke ();
			yield return new WaitForSeconds(tempo / beatspawn);
		}
	}
		
	// The amount of correctly hit notes/beats is updated here
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
		// Set beat hits to 0 if updated below 0
		if (beatHits < 0) {
			beatHits = 0;
		}
		// Ends the Rhythm Game due to having reached the required number of hits/beats
		if (beatHits >= beatHitsRequired) {
			if (!turnEnded) {
				combatController.selectedAbility.effect();
				turnEnded = true;
			}
		}
	}
		
	// Used to fade the rhythm games interface in and out
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

} // End