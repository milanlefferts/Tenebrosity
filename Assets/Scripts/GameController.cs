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
	public AudioClip hiddentombMusic;

	public bool isTimePaused;
	public bool isPlayerPaused;

	// Save data before Enemy Encounter
	public string currentScene;
	public string enemyEncounter;
	public Vector3 playerPosition;

	// Encounter data
	public string[] reward;

	// Party Stats
	int bone;
	int essence;
	int tooth;

	int elizeHealthCurrent;
	int angeloHealthCurrent;
	int fredericHealthCurrent;

	int elizeHealthMax;
	int angeloHealthMax;
	int fredericHealthMax;

	// Inventory
	public GameObject inventory;


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
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();

		if (player == null) {
			currentState = GameState.Combat;

		} else {
			currentState = GameState.Overworld;

		}

		backgroundMusic = this.GetComponent<AudioSource> ();
		//inventory = GameObject.FindGameObjectWithTag ("Inventory");
		//inventory.SetActive (false);
	}
	
	void Update () {

		// Open inventory

		if (Input.GetKeyDown (KeyCode.Tab)) {
			ToggleInventory ();
		}

		// Restart Game
		if (Input.GetKeyDown (KeyCode.R)) {
			SceneManager.LoadScene (0);
		}

		// Pause the Game
		if (Input.GetKeyDown (KeyCode.P)) {
			PauseTime ();
		}

	}

	void OnEnable() {
		SceneManager.sceneLoaded += this.OnLoadLevelFinishedLoading;
	}

	void OnLoadLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		inventory = GameObject.FindGameObjectWithTag ("Inventory");
		inventory.SetActive (false);
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();

	}

	void ToggleInventory() {
		
		//inventory = GameObject.FindGameObjectWithTag ("UI").GetComponentInChildren<Inventory>().gameObject;
		

		if (inventory.activeSelf) {
			inventory.SetActive (false);
			PausePlayer ();

		} else {
			inventory.SetActive (true);
			PausePlayer ();
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

	public void LoadOverworld() {
		SceneManager.LoadScene (currentScene);
	}

	public void ReloadCombat() {
		SceneManager.LoadScene ("Scene_Battle");
	}


	public void SwitchGameState (GameState newState){
		if (currentState != newState) {
			currentState = newState;

			switch(currentState) {
			case GameState.Overworld:

				// Load scene changes
				LoadSceneState ();

				// Area music
				backgroundMusic.clip = music;
				backgroundMusic.Play ();

				// Find inventory
				inventory = GameObject.FindGameObjectWithTag ("Inventory");
				inventory.SetActive (false);

				break;

			case GameState.Combat:

				//!commented out for testing combat
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

	public void SaveSceneState (string enemy, string[] spoils) {
		reward = spoils;
		enemyEncounter = enemy;
		playerPosition = player.gameObject.transform.position;
		currentScene = SceneManager.GetActiveScene().name;

	}

	public void LoadSceneState () {
		Destroy (GameObject.Find(enemyEncounter));
		enemyEncounter = null;
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		player.gameObject.transform.position = playerPosition;

	}



	public void SaveRewards() {
		foreach (string rew in reward) {
			switch (rew) {
			case "Bone":
				bone += 1;
				break;
			case "Essence":
				essence += 1;
				break;
			case "Tooth":
				tooth += 1;
				break;
			default:
				// add item to inventory
				print(reward + " was added to inventory");
				break;
			}
		}

		reward = null;
	}

	public void SavePlayerStats () {

	}

	void ResetGameController () {
		isTimePaused = false;
		isPlayerPaused = false;
	}


}
