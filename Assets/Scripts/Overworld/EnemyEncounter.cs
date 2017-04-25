using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Starts loading combat at the start of the scene and
// initiates combat on contact with enemy. 
public class EnemyEncounter : MonoBehaviour {

	// Loading
	AsyncOperation async;
	bool isLoading;

	// References
	private GameObject loadingScreen;
	GameController gameController;
	AudioSource audioSource;

	// Loot
	string[] rewards;

	void Start () {

		audioSource = GetComponent<AudioSource> ();
		isLoading = false;
		loadingScreen = GameObject.Find ("LoadingScreen");
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();

		GetEncounterData ();
		StartCoroutine(LoadScene());
	}

	// Set the rewards that are gained after combat
	void GetEncounterData () {
		switch (this.name) {
		case "EnemyEncounter": 
			rewards = new string[] {"Bone", "Bone", "Bone", "Tooth", "Essence", "Frozen Heart"};
			break;
		default:
			break;
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (!isLoading && collider.tag == "Player") {
			// Set this object as the Enemy Encounter and saves player position
			gameController.SaveSceneState(this.gameObject.name, rewards);
			// Plays enemy battle cry
			audioSource.Play ();
			isLoading = true;
			// Starts transition animation
			StartCoroutine(TransitionAnimation());
		}
	}

	private IEnumerator LoadScene() {

		// !!! Save Encounter info

		// Start Async loading operation
		Debug.LogWarning("DANGER: async load started..");
		async = SceneManager.LoadSceneAsync ("Scene_Battle");
		async.allowSceneActivation = false;

		yield return async;
	}

	// 
	private IEnumerator TransitionAnimation() {
		gameController.PausePlayer ();
		loadingScreen.GetComponent<Animator>().SetTrigger("LoadCombat");

		// Wait until animation is finished to start combat
		yield return new WaitForSeconds (2.0f);

		// Start combat
		async.allowSceneActivation = true;
	}

} // End
