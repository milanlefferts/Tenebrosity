using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CombatController : MonoBehaviour {
	
	bool combat;
	public GameObject activeCharacter;
	GameObject[] enemies;
	GameObject[] friendlies;

	Vector3 activeCharacterPos;

	// Turn Order
	public bool turnInProgress;
	int enemyCounter;
	int enemyCounterMax;
	int friendlyCounter;
	int friendlyCounterMax;

	//Ability Use
	GameObject abilityMenu;
	PartyController partyController;
	public bool selectingAbility;
	GameObject selectedAbilityBar;
	public Ability selectedAbility;
	GameObject abilityDescription;
	AbilityController abilityController;

	// Selection
	public string targetingMode;

	public bool selectingTarget;
	public int currentEnemyNr = 0;
	public GameObject target;

	// Combat Positions
	Vector3 PCPos;
	Vector3 NPCPos;

	// Enemy Targeting
	List<GameObject> targetList = new List<GameObject>();
	List<GameObject> characterList = new List<GameObject>();
	public GameObject[] targets;
	bool targetIsMoving;

	// Camera movement
	bool cameraIsMoving;
	//GameObject UI;
	public bool abilityConfirmed;

	//public GameObject[] targets;

	// Beat Game
	GameObject beatInterface;
	RhythmGameController rhythmGameController;
	BeatProgressVisual beatProgressVisual;
	Vector3 oldTargetPos;
	public bool rhythmGameActive;
	BeatSpawner beatSpawner;
	bool awaitingKeyPress;
	GameObject beatKeyPressText;

	void Start () {
		StartCoroutine (Combat ());
		partyController = GameObject.FindGameObjectWithTag ("PartyController").GetComponent<PartyController> ();
		abilityController = GameObject.FindGameObjectWithTag ("AbilityController").GetComponent<AbilityController> ();
		abilityMenu = GameObject.Find ("AbilityMenu");
		abilityMenu.SetActive (false);
		selectedAbilityBar = GameObject.Find ("SelectedAbility");
		selectedAbilityBar.SetActive (false);
		abilityDescription = GameObject.Find ("AbilityDescription");
		abilityDescription.SetActive (false);
		//UI = GameObject.FindGameObjectWithTag ("UI");

		// Combat Positions
		PCPos = new Vector3 (-45f, 1f, -2f);
		NPCPos = new Vector3 (-55f, 1.3f, -2.5f);

		// BeatGame
		beatInterface = GameObject.FindGameObjectWithTag ("BeatInterface");
		beatSpawner = GameObject.FindGameObjectWithTag ("BeatSpawner").GetComponent<BeatSpawner>();
		rhythmGameController = GameObject.FindGameObjectWithTag ("RhythmGameController").GetComponent<RhythmGameController>();
		beatProgressVisual = GameObject.FindGameObjectWithTag ("BeatProgressVisual").GetComponent<BeatProgressVisual>();
		awaitingKeyPress = false;
		beatKeyPressText = beatInterface.transform.FindChild ("BeatKeyPressText").gameObject;

	}
		
	// Determines what targets should be selected by the Ability
	void SetTargetingMode (string type) {
		// Friendly, Self, Enemy
		targetingMode = type;
	}

	void Update () {

		if (awaitingKeyPress && Input.anyKeyDown) {
			awaitingKeyPress = false;
		}
		
		// Selects an ability and starts target selection
		if (selectingAbility) {
			if (Input.GetKeyDown(KeyCode.Return)) {
				abilityMenu.SetActive (false);
				selectedAbilityBar.transform.FindChild ("AbilityName").GetComponent<Text> ().text = abilityMenu.GetComponent<AbilityMenu> ().currentAbility.transform.FindChild ("AbilityName").GetComponent<Text> ().text;

				selectedAbility = abilityController.abilityDictionary [selectedAbilityBar.transform.FindChild ("AbilityName").GetComponent<Text> ().text];
				selectedAbilityBar.transform.FindChild ("AbilityType").GetComponent<Image> ().color = ChangeAbilityTypeColor (selectedAbility.type);

				selectedAbilityBar.SetActive (true);

				// Reset AbilityMenu color
				foreach (GameObject obj in abilityMenu.GetComponent<AbilityMenu>().abilities) {
					obj.GetComponent<Image> ().color = new Color32(255, 255, 255, 100);
					obj.transform.FindChild("AbilityName").GetComponent<Text>().color = new Color32(0, 0, 0, 255);
				}


				SetTargetingMode (selectedAbility.target);

				InitiateTargetSelection ();
			}
		}

		// Cycles through targets and selects on Enter
		else if (selectingTarget) {
			if (Input.GetKeyDown(KeyCode.LeftArrow)) {
				PreviousTarget ();
			}

			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				NextTarget ();
			}

			if (Input.GetKeyDown (KeyCode.Backspace)) {

				abilityMenu.SetActive (true);

				selectingAbility = true;
				selectingTarget = false;

				// Clear pointers Targets
				foreach (GameObject enemy in enemies) {
					enemies [currentEnemyNr].GetComponent<Enemy> ().selectedPointer.SetActive (false);
				}

				foreach (GameObject enemy in enemies) {
					friendlies [currentEnemyNr].transform.FindChild("Selected").gameObject.SetActive(false);
				}
				selectedAbilityBar.SetActive (false);
			}
	
			if (Input.GetKeyDown(KeyCode.Return)) {
					// Clear pointers Targets
					foreach (GameObject enemy in enemies) {
						enemy.GetComponent<Enemy> ().selectedPointer.SetActive (false);
					}

					foreach (GameObject friend in friendlies) {
						friend.transform.FindChild("Selected").gameObject.SetActive(false);
					}

					// Setup targets
					targets = new GameObject[] { target };

					// # Beat Game #
				StartCoroutine(InitiateBeatGame ());

			}

		}

	} // End of Update



	IEnumerator Combat () {

		// Starts combat
		combat = true;

		// Finds combat particants and adjusts turn order accordingly
		enemies = GameObject.FindGameObjectsWithTag ("NPC").OrderBy (enemy => enemy.transform.position.z).ToArray();
		friendlies = GameObject.FindGameObjectsWithTag ("PC").OrderBy (enemy => enemy.transform.position.z).ToArray();;

		enemyCounter = 0;
		enemyCounterMax = enemies.Length;
		friendlyCounter = 0;
		friendlyCounterMax = friendlies.Length;

		// Sets starting character as always being first friendly
		activeCharacter = friendlies [0];
		friendlyCounter += 1;

		while (combat) {

			turnInProgress = true;
			print(activeCharacter.name);
			// Player Turn
			if (activeCharacter.tag == "PC") {
				StartCoroutine (StartPlayerTurn ());
				print ("Player Turn");
			}

			// Enemy Turn
			else {
				StartCoroutine (StartEnemyTurn ());
				print ("Enemy Turn");
			}

			while (turnInProgress) {
				yield return null;
			}

			// Sets the current active character to the next in line
			// Order is as follows: PC > NPC > PC > NPC > etc.
			if (activeCharacter.tag == "PC") {
				activeCharacter = enemies [enemyCounter];
				if (enemyCounter < enemyCounterMax-1) {
					enemyCounter += 1;
				} else {
					enemyCounter = 0;
				}
			} else {
				activeCharacter = friendlies [friendlyCounter];
				if (friendlyCounter < friendlyCounterMax-1) {
					friendlyCounter += 1;
				} else {
					friendlyCounter = 0;
				}
			}
		}
	}

	// Occurs on Removal of a character from combat
	// ! currently RESETS turn order on kill and does not take into account who has taken a turn that round
	public void UpdateTurnOrder (string tag) {
		if (tag == "NPC") {
			enemies = GameObject.FindGameObjectsWithTag ("NPC");
			enemyCounter = 0;
			enemyCounterMax = enemies.Length;
		} else {
			friendlies = GameObject.FindGameObjectsWithTag ("PC");
			friendlyCounter = 0;
			friendlyCounterMax = friendlies.Length;
		}
	}



	IEnumerator InitiateBeatGame () {


		//disable target selection and UI elements
		target.transform.FindChild("Selected").gameObject.SetActive(false);
		selectingTarget = false;

		// Store old position of target
		oldTargetPos = target.transform.position;

		// Position target for Beat Game
		if (activeCharacter.tag == "PC") {
			// if PC targeting PC
			if (target.tag == "PC") {
				// Do not move
			} 
			// if PC targeting NPC
			else {
				// Otherwise move enemy targeted by Player to right side
				StartCoroutine (MoveCharacter (target, NPCPos));
			}

		} else {
			StartCoroutine (MoveCharacter (target, PCPos));
		}

		// Reset progress
		rhythmGameController.beatHits = 0;
		rhythmGameController.beatsPassed = 0;
		rhythmGameController.turnEnded = false;

		// Set new ability's progress/visual effects
		selectedAbility.rhythmSetup();

		// Fadeout backdrop
		StartCoroutine (rhythmGameController.FadeRhythmGame (true));

			
		// Show beat game interface
		beatInterface.SetActive (true);
		beatProgressVisual.gameObject.SetActive(false);

		// Asks for player key input and set correct text
		awaitingKeyPress = true;
		beatKeyPressText.GetComponent<BeatKeyPressText> ().ChangeState (activeCharacter.tag);
		beatKeyPressText.SetActive (true);

		while (awaitingKeyPress) {
			yield return null;
		}
		beatKeyPressText.SetActive (false);

		// Start character animation
		activeCharacter.transform.FindChild("Sprite").gameObject.GetComponent<Animator>().SetTrigger(selectedAbility.name);


			beatProgressVisual.gameObject.SetActive (true);
	

		// Allows spawning of Beat Indicators
		beatSpawner.spawned = 0;
		rhythmGameActive = true;
		beatSpawner.isSpawning = true;



	}

	IEnumerator StartEnemyTurn() {
		
		yield return new WaitForSeconds (0.5f);

		// Move enemy to correct position and store previous position
		activeCharacterPos = activeCharacter.transform.position;;

		StartCoroutine (MoveCharacter (activeCharacter, NPCPos));

		yield return new WaitForSeconds (0.5f);

		// Apply Status Effects
		activeCharacter.GetComponent<Enemy> ().ResolveStatus();


		// Select ability (no AI)
		string [] abilityArray = activeCharacter.GetComponent<Enemy>().GetAbilities();
		selectedAbility = abilityController.abilityDictionary [abilityArray [Random.Range (0, abilityArray.Length)]];
		Debug.Log (selectedAbility.name);
		selectedAbilityBar.SetActive(true);
		selectedAbilityBar.transform.FindChild ("AbilityName").GetComponent<Text> ().text = selectedAbility.name;
		selectedAbilityBar.transform.FindChild ("AbilityType").GetComponent<Image> ().color = ChangeAbilityTypeColor(selectedAbility.type);


		// Select target(s)
		SetupCharacterList();
		SelectTargetsNPC(selectedAbility.target);

		yield return new WaitForSeconds (1.5f);


		StartCoroutine(InitiateBeatGame());

	}

	public IEnumerator EndEnemyTurn () {

		yield return new WaitForSeconds (1.5f);

		// Stop enemy attack animations, if applicable
		activeCharacter.transform.FindChild("Sprite").gameObject.GetComponent<Animator>().SetTrigger(selectedAbility.name);

		selectedAbilityBar.SetActive(false);


		// Move character back to starting position
		//StartCoroutine (MoveCamera (cameraPos));
		StartCoroutine (MoveCharacter (target, oldTargetPos));
		StartCoroutine (MoveCharacter (activeCharacter, activeCharacterPos));

		// FadeIn Backdrop
		StartCoroutine (rhythmGameController.FadeRhythmGame (false));

		// Disable Rhythm game
		beatInterface.SetActive (false);
		rhythmGameActive = false;
		beatSpawner.isSpawning = false;

		// Reset variables
		selectedAbility = null;

		// End Turn
		yield return new WaitForSeconds(2f);
		turnInProgress = false;
	}

	void SetupCharacterList () {
		// Find characters
		GameObject[] characterArray = GameObject.FindGameObjectsWithTag("PC");

		//Add characters to characterList
		foreach (GameObject character in characterArray) {
			characterList.Add(character);
		}
	}

	void SelectTargetsNPC(string targetType) {
		int targetnr;
		if (targetType == "EnemyAll") {
			targetnr = 3;
		} else {
			targetnr = 1;
		}
		
		
		// Renew charactersLeft
		List<GameObject> charactersLeft = new List<GameObject>();
		foreach (GameObject character in characterList) {
			charactersLeft.Add(character);
		}

		// Empty target list
		targetList.Clear();

		// Add targets to targetList, make sure to not choose target twice
		for (int i = 0; i < targetnr; i++) {
			target = charactersLeft[Random.Range(0, charactersLeft.Count)];
			targetList.Add (target);
			charactersLeft.Remove(target);
			Debug.Log(target);
		}
		
		targets = new GameObject[] { target };

	}


	IEnumerator StartPlayerTurn() {

		yield return new WaitForSeconds (0.5f);

		// Move character to correct position and store previous position
		activeCharacterPos = activeCharacter.transform.position;

		StartCoroutine (MoveCharacter (activeCharacter, PCPos));

		// Apply Status Effects
		activeCharacter.GetComponent<Friendly> ().ResolveStatus();

		// Sets up correct abilities for active character
		SetupAbilityMenu(activeCharacter);


	}


	public IEnumerator EndPlayerTurn () {

		yield return new WaitForSeconds (1.5f);

		// Stop character animations
		activeCharacter.transform.FindChild("Sprite").gameObject.GetComponent<Animator>().SetTrigger(selectedAbility.name);


		// Move character back to starting position
		//StartCoroutine (MoveCamera (cameraPos));
		StartCoroutine (MoveCharacter (target, oldTargetPos));
		StartCoroutine (MoveCharacter (activeCharacter, activeCharacterPos));

		beatInterface.SetActive (false);

		// Resolve ability effects and Update UI

		// FadeIn Backdrop
		StartCoroutine (rhythmGameController.FadeRhythmGame (false));


		// Disable irrelevant UI
		abilityDescription.SetActive (false);

		// End Turn
		beatSpawner.isSpawning = false;
		selectedAbility = null;
		rhythmGameActive = false;
		selectingAbility = false;
		selectingTarget = false;
		abilityConfirmed = false;
		selectedAbilityBar.SetActive(false);

		yield return new WaitForSeconds (1.5f);

		turnInProgress = false;


	}

	void SetupAbilityMenu (GameObject character){
		selectingAbility = true;
		string[] abilityArray = activeCharacter.GetComponent<Friendly> ().abilityArray;
		//string[] abilityArray = partyController.GetAbilities (character.name);
		for (int i = 0; i < abilityArray.Length; i++) {
			GameObject menuItem = abilityMenu.transform.FindChild (i + 1 + "").gameObject;
			menuItem.transform.FindChild ("AbilityName").GetComponent<Text> ().text = abilityArray[i];
			menuItem.transform.FindChild ("AbilityType").GetComponent<Image> ().color = ChangeAbilityTypeColor(abilityController.abilityDictionary[abilityArray [i]].type);
			menuItem.SetActive (true);
		}
		abilityMenu.GetComponent<AbilityMenu> ().DetermineAbilities ();
		abilityMenu.SetActive (true);
		abilityDescription.SetActive (true);
	}

	public void ChangeAbilityDescription () {

		abilityDescription.transform.FindChild ("AbilityText").GetComponent<Text> ().text = abilityController.abilityDictionary[abilityMenu.GetComponent<AbilityMenu> ().currentAbility.transform.FindChild ("AbilityName").GetComponent<Text> ().text].text;
	}

	Color32 ChangeAbilityTypeColor (string type) {
		switch (type) {
		case "Rotten":
			return new Color32 (60, 150, 65, 255);
		case "Tainted":
			return new Color32 (125, 70, 220, 255);
		case "Organic":
			return new Color32 (170, 0, 60, 255);
		default:
			return new Color32 (0, 0, 0, 255);
		}
	}

	IEnumerator MoveCharacter(GameObject character, Vector3 target)
	{
		while (targetIsMoving) {
			yield return null;
		}
		targetIsMoving = true;
		Vector3 startPosition = character.transform.position;
		while(Vector3.Distance(startPosition, target)>0.005f)
		{
			startPosition = character.transform.position;
			character.transform.position = Vector3.MoveTowards(startPosition, target, 25.0f*Time.deltaTime);
			yield return null;
		}
		targetIsMoving = false;
	}

	// ## Player Target Selection ##

	void InitiateTargetSelection () {
		// Reset targetting state
		selectingAbility = false;
		selectingTarget = true;


		// Clear pointers Targets
		foreach (GameObject enemy in enemies) {
			enemies [currentEnemyNr].GetComponent<Enemy> ().selectedPointer.SetActive (false);
		}

		// Clear pointers Targets
		foreach (GameObject friendly in friendlies) {
			friendlies [currentEnemyNr].transform.FindChild("Selected").gameObject.SetActive(false);
		}

		if (targetingMode == "Enemy") {
			enemies [currentEnemyNr].GetComponent<Enemy> ().selectedPointer.SetActive (true);
			target = enemies [currentEnemyNr];
		}

		else {
			friendlies [currentEnemyNr].transform.FindChild("Selected").gameObject.SetActive(false);;
			target = friendlies [currentEnemyNr];
		}
			
		target.transform.FindChild("Selected").gameObject.SetActive(true);

	}

	void NextTarget () {
		target.transform.FindChild("Selected").gameObject.SetActive(false);

		if (targetingMode == "Enemy") {
			
			if (currentEnemyNr < enemies.Length - 1) {
				currentEnemyNr += 1;
			} else {
				currentEnemyNr = 0;
			}

			target = enemies [currentEnemyNr];

		} else if (targetingMode == "Self") {
			target = friendlies [0];

			// TargetingMode if Friendlies
		} else {
			
			if (currentEnemyNr > 0) {
				currentEnemyNr -= 1;
			} else {
				currentEnemyNr = friendlies.Length - 1;
			}

			target = friendlies [currentEnemyNr];

		}
			
		target.transform.FindChild("Selected").gameObject.SetActive(true);
	}

	void PreviousTarget () {
		target.transform.FindChild("Selected").gameObject.SetActive(false);

		if (targetingMode == "Enemy") {
			if (currentEnemyNr > 0) {
				currentEnemyNr -= 1;
			} else {
				currentEnemyNr = enemies.Length - 1;
			}
			target = enemies [currentEnemyNr];

		} else if (targetingMode == "Self") {
			target = friendlies [0];

		} else {
			
			if (currentEnemyNr < friendlies.Length - 1) {
				currentEnemyNr += 1;
			} else {
				currentEnemyNr = 0;
			}

			target = friendlies [currentEnemyNr];
		}

		target.transform.FindChild("Selected").gameObject.SetActive(true);
	}


}
