using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour {

	public static GameController Instance;


	public enum GameState {Overworld, Combat, Inventory, Menu};
	public GameState currentState;

	PlayerController player;

	AudioSource backgroundMusic;
	public AudioClip music;

	public bool isTimePaused;
	public bool isPlayerPaused;

	// Save data before Enemy Encounter
	public string enemyEncounter;
	public Vector3 playerPosition;

	void Awake () {
		if (Instance) {
			DestroyImmediate (this.gameObject);
		} else {
			DontDestroyOnLoad(transform.gameObject);
			Instance = this;
		}

	}

	// Use this for initialization
	void Start () {
		currentState = GameState.Overworld;

		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		backgroundMusic = this.GetComponent<AudioSource> ();
	}
	
	void Update () {
		// Restart Game
		if (Input.GetKeyDown (KeyCode.R)) {
			SceneManager.LoadScene (0);
		}

		// Pause the Game
		if (Input.GetKeyDown (KeyCode.P)) {
			PauseTime ();
		}
	}


	// Freeze the game
	public void PauseTime () {
		if (isTimePaused) {
			Time.timeScale = 1.0f;
			isTimePaused = false;
		} else {
			Time.timeScale = 0.0f;
			isTimePaused = true;
		}
	}


	// Remove control from player
	public void PausePlayer () {

			// Return control
			if (isPlayerPaused) {
				player.enabled = true;
				isPlayerPaused = false;
				// Remove control
			} else {
				player.enabled = false;
				isPlayerPaused = true;
			}
	
	}


	public void SwitchGameState (GameState newState){
		if (currentState != newState) {
			currentState = newState;

			switch(currentState) {
			case GameState.Overworld:

				//PausePlayer ();
				// repositions player, destroys enemy
				LoadSceneState ();
				backgroundMusic.clip = music;
				backgroundMusic.Play ();
				break;

			case GameState.Combat:


				backgroundMusic.Stop ();

				break;
			case GameState.Inventory:
				break;

			case GameState.Menu:
				break;
			
			default:
				break;
			}

			ResetGameController ();

		}
	}

	public void SaveSceneState (string enemy) {
		enemyEncounter = enemy;
		playerPosition = player.gameObject.transform.position;
	}

	public void LoadSceneState () {
		Destroy (GameObject.Find(enemyEncounter));
		enemyEncounter = null;
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		player.gameObject.transform.position = playerPosition;

	}

	void ResetGameController () {
		isTimePaused = false;
		isPlayerPaused = false;
	}


}
