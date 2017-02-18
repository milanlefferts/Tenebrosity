using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyEncounter : MonoBehaviour {
	
	GameController gameController;



	AudioSource audioSource;
	AsyncOperation async;
	bool isLoading;

	GameObject loadingScreen;

	void Start () {

		audioSource = GetComponent<AudioSource> ();
		isLoading = false;
		loadingScreen = GameObject.Find ("LoadingScreen");
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();


		StartCoroutine(LoadScene());
	}

	void Update () {

	}



	void GetDialogue () {
		string words;
		switch (this.name) {
		case "Lakedweller": 
			words = "Oh it's you again..";
			// assign sound
			break;
		case "Bob": 
			words = "Have you seen any feet \n lying around?";
			// assign sound
			break;
		case "Grave": 
			words = "Here lies Roderick, \n a man of few words \n and fewer deeds";
			// assign sound
			break;
		default:
			words = "Nothing";
			// assign sound
			break;
		}
	}

	void OnTriggerEnter() {
		if (!isLoading) {
			// Set this object as the Enemy Encounter and saved player position
			gameController.SaveSceneState(this.gameObject.name);

			// play enemy battlecry
			audioSource.Play ();
			isLoading = true;
			// Load combat scene
			//StartCoroutine(LoadScene());

			StartCoroutine(TransitionAnimation());

		}
	}

	IEnumerator LoadScene() {
		

		// Play transition animation
		//StartCoroutine(TransitionAnimation());

		// Start Async loading operation
		Debug.LogWarning("ASYNC LOAD STARTED - " +
			"DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
		async = SceneManager.LoadSceneAsync ("Scene_Battle");
		async.allowSceneActivation = false;

		yield return async;

	}

	IEnumerator TransitionAnimation() {
		
		gameController.PausePlayer ();

		// play animation
		loadingScreen.GetComponent<Animator>().SetTrigger("LoadCombat");

		// Wait until animation is finished to activate scene
		yield return new WaitForSeconds (2.0f);
		print ("Animation finished!");

		// load combat scene
		async.allowSceneActivation = true;

	}


}
