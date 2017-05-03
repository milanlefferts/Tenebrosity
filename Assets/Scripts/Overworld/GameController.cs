using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Controls the game flow in the overworld, including saving states and loading combat
public class GameController : MonoBehaviour {

	public static GameController Instance;
	private PlayerController player;

	// GameState
	public enum GameState {Overworld, Combat, Inventory, Menu};
	public GameState currentState;

	// Music
	AudioSource backgroundMusic;
	public AudioClip music;
	public AudioClip hiddentombMusic;

	// Pausing
	public bool isTimePaused;
	public bool isPlayerPaused;

	// Save data before Enemy Encounter
	public string currentScene;
	public string enemyEncounter;
	public Vector3 playerPosition;

	// Encounter data
	public string[] reward;

	// Party Stats
	public int bone, essence, tooth;
	public int elizeHealthCurrent, angeloHealthCurrent, fredericHealthCurrent;
	public int elizeHealthMax, angeloHealthMax, fredericHealthMax;

	// Inventory
	public GameObject inventory;

	void Awake () {
		// Singleton pattern for GameController
		if (Instance) {
			DestroyImmediate (this.gameObject);
		} else {
			DontDestroyOnLoad(transform.gameObject);
			Instance = this;
		}
	}

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		backgroundMusic = this.GetComponent<AudioSource> ();

		// Set correct gamestate by checking if the player is in the scene
		currentState = player == null ? GameState.Combat : GameState.Overworld;
	}

	public static bool Press<T>(KeyCode key) where T : struct {
		return Input.GetKeyDown (key);
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
		if (inventory.activeSelf) {
			inventory.SetActive (false);
		} else {
			inventory.SetActive (true);
		}
		PausePlayer ();
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

	// Allows switching of GameState. Executes associated methods on switch.
	public void SwitchGameState (GameState newState){
		if (currentState != newState) {
			
			currentState = newState;

			switch(currentState) {
			case GameState.Overworld:
				SwitchToOverworldGameState ();
				break;
			case GameState.Combat:
				SwitchToCombatGameState ();
				break;
			case GameState.Inventory:
				// SwitchToInventoryGameState ();
				break;
			case GameState.Menu:
				// SwitchToMenuGameState ();
				break;
			default:
				break;
			}
			UnpauseGame ();
		}
	}

	private void SwitchToCombatGameState () {
		backgroundMusic.Stop ();
	}

	private void SwitchToOverworldGameState () {
		// Load scene changes
		LoadSceneState ();

		// Area music
		backgroundMusic.clip = music;
		backgroundMusic.Play ();

		// Find inventory
		inventory = GameObject.FindGameObjectWithTag ("Inventory");
		inventory.SetActive (false);
	}

	// Saves player location on the overworld
	public void SaveSceneState (string enemy, string[] spoils) {
		reward = spoils;
		enemyEncounter = enemy;
		playerPosition = player.gameObject.transform.position;
		currentScene = SceneManager.GetActiveScene().name;

	}

	// After combat, returns player to the correct position on scene load
	public void LoadSceneState () {
		Destroy (GameObject.Find(enemyEncounter));
		enemyEncounter = null;
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		player.gameObject.transform.position = playerPosition;

	}

	// Adds items to player inventory depending on what was picked up
	// !!! May change to a function on the item that awards itself
	public void SaveRewards() {
		reward = new string[] { name };
		foreach (string rew in reward) {
			//print(rew + " was added to inventory");
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
				break;
			}
		}
		reward = null;
	}

	public void PickupObject(string name) {
		SaveRewards ();
	}

	public void SavePlayerStats () {

	}

	void UnpauseGame () {
		isTimePaused = false;
		isPlayerPaused = false;
	}


}
