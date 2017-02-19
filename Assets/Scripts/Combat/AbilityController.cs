using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Ability {

	public string name;
	public string text;
	public string target;
	public string type;
	public Action effect;
	public Action rhythmSetup;

	public Ability (string aName, string aText, string aTarget, string aType, Action aEffect, Action rhythm) {
		name = aName;
		text = aText;
		target = aTarget;
		type = aType;
		effect = aEffect;
		rhythmSetup = rhythm;

	}

}


public class AbilityController : MonoBehaviour {

	PartyController partyController;
	CombatController combatController;
	RhythmGameController rhythmGameController;

	// Rhythm Progress Visual sprites
	public Sprite[] rPV_drainBlood;

	public Dictionary<string, Ability> abilityDictionary = new Dictionary<string, Ability>();

	public GameObject bloodDrop;

	public Sprite[] statusIcons;

	void Awake () {

		Action slam = Slam;
		Action slam_rS = Slam_rS;
		Action peck = Peck;
		Action peck_rS = Peck_rS;

		Action drainBlood = DrainBlood;
		Action drainBlood_rS = DrainBlood_rS;
		Action unholyMending = UnholyMending;
		Action unholyMending_rS = UnholyMending_rS;
		Action debilitate = Debilitate;
		Action debilitate_rS = Debilitate_rS;
		Action quickeningPulse = QuickeningPulse;
		Action quickeningPulse_rS = QuickeningPulse_rS;

		Action graveInsult = GraveInsult;
		Action ribCage = RibCage;
		Action shoulderBlade = ShoulderBlade;
		Action skullCracker = SkullCracker;
		Action graveInsult_rS = GraveInsult_rS;
		Action ribCage_rS = RibCage_rS;
		Action shoulderBlade_rS = ShoulderBlade_rS;
		Action skullCracker_rS = SkullCracker_rS;

		Action tear = Tear;
		Action pounce = Pounce;
		Action lickWounds = LickWounds;
		Action howl = Howl;
		Action tear_rS = Tear_rS;
		Action pounce_rS = Pounce_rS;
		Action lickWounds_rS = LickWounds_rS;
		Action howl_rS = Howl_rS;

		// HeartSeeker Abilities
		abilityDictionary.Add ("Peck", new Ability(
			"Peck", 
			"Peck an eye out, KRAWW",
			"Enemy",
			"Rotten",
			peck,
			peck_rS
		));

		// LakeDweller Abilities
		abilityDictionary.Add ("Slam", new Ability(
			"Slam", 
			"Whip the enemy with tentacles",
			"Enemy",
			"Organic",
			slam,
			slam_rS
		));

	// Elize Abilities
		abilityDictionary.Add ("DrainBlood", new Ability(
			"DrainBlood", 
			"Bite the enemy for low damage and restores a small amount of health",
			"Enemy",
			"Rotten",
			drainBlood,
			drainBlood_rS
		));

		abilityDictionary.Add ("UnholyMending", new Ability(
			"UnholyMending", 
			"Call upon the spirits to heal your wounds, at a price..",
			"Friendly",
			"Tainted",
			unholyMending,
			unholyMending_rS
		));

		abilityDictionary.Add ("Debilitate", new Ability(
			"Debilitate", 
			"Curses your enemy, leaving them unable to defend themselves",
			"Enemy",
			"Tainted",
			debilitate,
			debilitate_rS
		));

		abilityDictionary.Add ("QuickeningPulse", new Ability(
			"QuickeningPulse", 
			"Act twice in succession next round",
			"Self",
			"Organic",
			quickeningPulse,
			quickeningPulse_rS
		));
			

	// Angelo Abilities
		abilityDictionary.Add ("ShoulderBlade", new Ability(
			"ShoulderBlade", 
			"Pierce enemy armor with a protruding shoulder blade",
			"Enemy",
			"Rotten",
			shoulderBlade,
			shoulderBlade_rS
		));

		abilityDictionary.Add ("GraveInsult", new Ability(
			"GraveInsult", 
			"Hurl grim insults at your foe, causing them to attack you",
			"Enemy",
			"Tainted",
			graveInsult,
			graveInsult_rS
		));

		abilityDictionary.Add ("RibCage", new Ability(
			"RibCage", 
			"Enclose an ally within a protective shield of bone",
			"Friendly",
			"Tainted",
			ribCage,
			ribCage_rS
		));

		abilityDictionary.Add ("SkullCracker", new Ability(
			"SkullCracker", 
			"Cause alarming fractures, increasing enemy damage taken",
			"Enemy",
			"Organic",
			skullCracker,
			skullCracker_rS
		));


	// Frederic Abilities
		abilityDictionary.Add ("Tear", new Ability(
			"Tear", 
			"Rip and shred flesh, causing heavy laceration",
			"Enemy",
			"Organic",
			tear,
			tear_rS
		));

		abilityDictionary.Add ("Howl", new Ability(
			"Howl", 
			"A fierce howl that demoralizes the enemy, decreasing their strength",
			"Enemy",
			"Tainted",
			howl,
			howl_rS
		));

		abilityDictionary.Add ("LickWounds", new Ability(
			"LickWounds", 
			"Retreat to recover your wounds, leaving you open to attack",
			"Self",
			"Organic",
			lickWounds,
			lickWounds_rS
		));

		abilityDictionary.Add ("Pounce", new Ability(
			"Pounce", 
			"Ready your self to ambush a foe, attacking ferociously next turn",
			"Enemy",
			"Organic",
			pounce,
			pounce_rS
		));
			
	}
		
	void Start () {
		partyController = GameObject.FindGameObjectWithTag ("PartyController").GetComponent<PartyController> ();
		combatController = GameObject.FindGameObjectWithTag ("CombatController").GetComponent<CombatController> ();
		rhythmGameController = GameObject.FindGameObjectWithTag ("RhythmGameController").GetComponent<RhythmGameController>();
	}

	// Ability General Functions
	public void InflictDamage (GameObject[] targets, int damage, int pierce, string type) {

		// Calculate damage reduction based on beats hit
		int defense = (int)rhythmGameController.beatHits;

		if (combatController.activeCharacter.tag == "NPC") {
			if (defense > 0) {
				combatController.target.GetComponent<Friendly>().SpawnText("Blocked " + defense);
			}

			damage -= defense;
			if (damage < 0) {
				damage = 0;
			}
		}

		foreach (GameObject target in targets) {
			// Player Character
			if (target.tag == "PC") {
				target.GetComponent<Friendly> ().Damage (damage, pierce, type);
			}
			 
			// Enemy Character
			else if (target.tag == "NPC") {
				target.GetComponent<Enemy> ().Damage (damage, pierce, type);
			} 
		}

		// End Turn
		if (combatController.activeCharacter.tag == "PC") {
			StartCoroutine(combatController.EndPlayerTurn());

		} else {
			StartCoroutine(combatController.EndEnemyTurn());
		}
	}

	public void Heal (GameObject[] targets, int healing) {
		foreach (GameObject target in targets) {
			// Player Character
			if (target.tag == "PC") {
				target.GetComponent<Friendly> ().Heal (healing);
			}

			// Enemy Character
			else if (target.tag == "NPC") {
				target.GetComponent<Enemy> ().Heal (healing);
			}
		}

		// End Turn
		if (combatController.activeCharacter.tag == "PC") {
			StartCoroutine(combatController.EndPlayerTurn());
		} else {
			StartCoroutine(combatController.EndEnemyTurn());
		}
	}

	public void InflictStatus (GameObject[] targets, string status, int duration) {
		foreach (GameObject target in targets) {
			// Player Character
			if (target.tag == "PC") {
				target.GetComponent<Friendly> ().InflictStatus (status, duration);
			}

			// Enemy Character
			else if (target.tag == "NPC") {
				target.GetComponent<Enemy> ().InflictStatus (status, duration);

			}
		}
		// End Turn
		if (combatController.activeCharacter.tag == "PC") {
			StartCoroutine(combatController.EndPlayerTurn());
		} else {
			StartCoroutine(combatController.EndEnemyTurn());
		}
	}


	// ENEMY Abilities
	// -------------------------------------------------------------------------------------------------------------


	// LakeDweller

	public void Slam () {
		InflictDamage (combatController.targets, 15, 0, "Organic");

		//Heal (combatController.targets, 5);
	}

	public void Slam_rS () {
		// Set Beat information
		rhythmGameController.beatHitsRequired = 6f;
		rhythmGameController.beatSpawnedTotal = 6;
		rhythmGameController.beatSpawnSpeed = 1f;
		rhythmGameController.beatTravelSpeed = 160f;

		// Load correct Rhythm Game Animator
		rhythmGameController.beatProgressVisual.GetComponentInChildren<Animator>().runtimeAnimatorController = null;

		//rhythmGameController.beatHitsRequired / 4;
		rhythmGameController.beatVisualAnimationStates = 6f;
	}

	// Heartseeker
	public void Peck () {
		InflictDamage (combatController.targets, 10, 0, "Undying");

		//Heal (combatController.targets, 5);
	}

	public void Peck_rS () {
		// Set Beat information
		rhythmGameController.beatHitsRequired = 6f;
		rhythmGameController.beatSpawnedTotal = 10;
		rhythmGameController.beatSpawnSpeed = 1f;
		rhythmGameController.beatTravelSpeed = 160f;

		// Load correct Rhythm Game Animator
		//rhythmGameController.beatProgressVisual.GetComponentInChildren<Animator>().runtimeAnimatorController = Resources.Load("Animators/Defend") as RuntimeAnimatorController;

		rhythmGameController.beatProgressVisual.GetComponentInChildren<Animator>().runtimeAnimatorController = null;
		rhythmGameController.beatVisualAnimationStates = 6f;
	}


// Elize Abilities
// -------------------------------------------------------------------------------------------------------------

	public void DrainBlood () {
		InflictDamage (combatController.targets, 10, 0, "Tainted");

		//Heal (combatController.targets, 5);
	}

	public void DrainBlood_rS () {
		// Set Beat information
		rhythmGameController.beatHitsRequired = 5f;
		rhythmGameController.beatSpawnedTotal = 10;
		rhythmGameController.beatSpawnSpeed = 1f;
		rhythmGameController.beatTravelSpeed = 160f;

		// Load correct Rhythm Game Animator
		rhythmGameController.beatProgressVisual.GetComponentInChildren<Animator>().runtimeAnimatorController = Resources.Load("Animators/DrainBlood") as RuntimeAnimatorController;
		rhythmGameController.beatVisualAnimationStates = 5f;

		//combatController.activeCharacter.transform.FindChild("Sprite").gameObject.GetComponent<Animator>().SetTrigger(combatController.selectedAbility.name);

	}

	public void UnholyMending () {
		Heal (combatController.targets, 10);
	}

	public void UnholyMending_rS () {
		// Set Beat information
		rhythmGameController.beatHitsRequired = 8f;
		rhythmGameController.beatSpawnedTotal = 12;
		rhythmGameController.beatSpawnSpeed = 1f;
		rhythmGameController.beatTravelSpeed = 160;

		// Load correct Rhythm Game Animator
		rhythmGameController.beatProgressVisual.GetComponentInChildren<Animator>().runtimeAnimatorController = Resources.Load("Animators/UnholyMending") as RuntimeAnimatorController;
		rhythmGameController.beatVisualAnimationStates = 6f;

	}

	public void Debilitate () {
		InflictStatus (combatController.targets, "Debuff: Defense", 3);
	}

	public void Debilitate_rS() {
	}

	public void QuickeningPulse () {
		InflictStatus (combatController.targets, "Buff: Haste", 1);
	}

	public void QuickeningPulse_rS () {
	}

	// Angelo Abilities
	// -------------------------------------------------------------------------------------------------------------

	public void ShoulderBlade () {
		InflictDamage (combatController.targets, 10, 5, "Undying");
	}

	public void GraveInsult () {
		InflictStatus (combatController.targets, "Taunt", 3);
	}

	public void SkullCracker () {
		InflictStatus (combatController.targets, "Debuff: Defense", 3);
	}
		
	public void RibCage () {
		InflictStatus (combatController.targets, "DefPlus", 3);

		//Heal (combatController.targets, 5);
	}

	public void RibCage_rS () {
		// Set Beat information
		rhythmGameController.beatHitsRequired = 4f;
		rhythmGameController.beatSpawnedTotal = 5;
		rhythmGameController.beatSpawnSpeed = 1f;
		rhythmGameController.beatTravelSpeed = 160f;

		// Load correct Rhythm Game Animator
		rhythmGameController.beatProgressVisual.GetComponentInChildren<Animator>().runtimeAnimatorController = Resources.Load("Animators/RibCage") as RuntimeAnimatorController;
		rhythmGameController.beatVisualAnimationStates = 4f;

	
	}




	public void ShoulderBlade_rS () {
		InflictDamage (combatController.targets, 10, 5, "Undying");
	}

	public void GraveInsult_rS () {
		InflictStatus (combatController.targets, "Taunt", 3);
	}

	public void SkullCracker_rS () {
		InflictStatus (combatController.targets, "Debuff: Defense", 3);
	}



	// Frederic Abilities
	// -------------------------------------------------------------------------------------------------------------

	public void Tear () {
		InflictStatus (combatController.targets, "DoT", 3);
		InflictDamage (combatController.targets, 4, 0, "Tainted");

	}

	public void Tear_rS () {
		// Set Beat information
		rhythmGameController.beatHitsRequired = 4f;
		rhythmGameController.beatSpawnedTotal = 8;
		rhythmGameController.beatSpawnSpeed = 0.5f;
		rhythmGameController.beatTravelSpeed = 120f;

		// Load correct Rhythm Game Animator
		rhythmGameController.beatProgressVisual.GetComponentInChildren<Animator>().runtimeAnimatorController = Resources.Load("Animators/Tear") as RuntimeAnimatorController;
		rhythmGameController.beatVisualAnimationStates = 2f;

	
	}



	public void LickWounds () {
		Heal (combatController.targets, 20);
		InflictStatus (combatController.targets, "Debuff: Defense", 1);
	}

	public void Pounce () {
		// Self: Charge
		// Next turn: Enemy heavy damage
		//InflictStatus (combatController.targets, "Charge", 1);
	}


	public void Howl () {
		InflictStatus (combatController.targets, "DefMin", 3);
	}

	public void Howl_rS () {
		// Set Beat information
		rhythmGameController.beatHitsRequired = 6f;
		rhythmGameController.beatSpawnedTotal = 12;
		rhythmGameController.beatSpawnSpeed = 1f;
		rhythmGameController.beatTravelSpeed = 160f;

		// Load correct Rhythm Game Animator
		rhythmGameController.beatProgressVisual.GetComponentInChildren<Animator>().runtimeAnimatorController = Resources.Load("Animators/Howl") as RuntimeAnimatorController;
		rhythmGameController.beatVisualAnimationStates = 3f;

	}

	public void LickWounds_rS () {
		Heal (combatController.targets, 20);
		InflictStatus (combatController.targets, "Debuff: Defense", 1);
	}

	public void Pounce_rS () {
		// Self: Charge
		// Next turn: Enemy heavy damage
		//InflictStatus (combatController.targets, "Charge", 1);
	}









}

