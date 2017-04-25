using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

// Regulates the effects of using Abilities in combat.
// Abilities with 'rS' are used to determine their 'charging' animation frames during Combat. 
public class AbilityController : MonoBehaviour {
	public CombatController combatController;
	public RhythmGameController rhythmGameController;

	public Dictionary<string, Ability> abilityDictionary = new Dictionary<string, Ability>();

	public Sprite[] statusIcons;
	// Blooddrop icon spawned on ability use -> move somewhere else?
	public GameObject bloodDrop;

	void Awake () {
		
	// Assigns methods to all actions
	// ! Must be a better way to mass assign these
		// Elize actions
		Action drainBlood = DrainBlood;
		Action drainBlood_rS = DrainBlood_rS;
		Action unholyMending = UnholyMending;
		Action unholyMending_rS = UnholyMending_rS;
		Action debilitate = Debilitate;
		Action debilitate_rS = Debilitate_rS;
		Action quickeningPulse = QuickeningPulse;
		Action quickeningPulse_rS = QuickeningPulse_rS;

		// Angelo actions
		Action graveInsult = GraveInsult;
		Action ribCage = RibCage;
		Action shoulderBlade = ShoulderBlade;
		Action skullCracker = Skullcracker;
		Action graveInsult_rS = GraveInsult_rS;
		Action ribCage_rS = RibCage_rS;
		Action shoulderBlade_rS = ShoulderBlade_rS;
		Action skullCracker_rS = Skullcracker_rS;

		// Frederic actions
		Action tear = Tear;
		Action pounce = Pounce;
		Action lickWounds = LickWounds;
		Action howl = Howl;
		Action tear_rS = Tear_rS;
		Action pounce_rS = Pounce_rS;
		Action lickWounds_rS = LickWounds_rS;
		Action howl_rS = Howl_rS;

		// Enemy actions
		Action slam = Slam;
		Action slam_rS = Slam_rS;
		Action peck = Peck;
		Action peck_rS = Peck_rS;
	
	// Adds party/enemy abilities to the ability dictionary
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

		abilityDictionary.Add ("Skullcracker", new Ability(
			"Skullcracker", 
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
		combatController = GameObject.FindGameObjectWithTag ("CombatController").GetComponent<CombatController> ();
		rhythmGameController = GameObject.FindGameObjectWithTag ("RhythmGameController").GetComponent<RhythmGameController>();
	}

	// Inflicts damage on the target(s)
	public void InflictDamage (GameObject[] targets, int damage, int pierce, string type) {

		// Calculate damage reduction based on beats hit
		int defense = (int)rhythmGameController.beatHits;

		// Shows damage blocked by NPCs
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
			else {
				target.GetComponent<Enemy> ().Damage (damage, pierce, type);
			} 
		}
		EndTurn ();
	}

	// Heals the target(s)
	public void Heal (GameObject[] targets, int healing) {
		foreach (GameObject target in targets) {
			// Player Character
			if (target.tag == "PC") {
				target.GetComponent<Friendly> ().Heal (healing);
			}
			// Enemy Character
			else {
				target.GetComponent<Enemy> ().Heal (healing);
			}
		}
		EndTurn ();
	}

	// Inflicts Status on the target(s)
	public void InflictStatus (GameObject[] targets, string status, int duration, string type) {
		foreach (GameObject target in targets) {
			// Player Character
			if (target.tag == "PC") {
				target.GetComponent<Friendly> ().InflictStatus (status, duration, type);
			}
			// Enemy Character
			else {
				target.GetComponent<Enemy> ().InflictStatus (status, duration, type);
			}
		}
		EndTurn ();
	}

	// End Turn after using an Ability
	void EndTurn() {
		if (combatController.activeCharacter.tag == "PC") {
			StartCoroutine(combatController.EndPlayerTurn());
		} else {
			StartCoroutine(combatController.EndEnemyTurn());
		}
	}

	// This method is used to set the Ability attributes for the current combat round.
	public void SetBeatInformation (float beatHitsRequired, int beatSpawnedTotal, float beatSpawnSpeed, float beatTravelSpeed, string resourceName, float beatVisualAnimationStates) {
		// Set Beat information
		rhythmGameController.beatHitsRequired = beatHitsRequired;
		rhythmGameController.beatSpawnedTotal = beatSpawnedTotal;
		rhythmGameController.beatSpawnSpeed = beatSpawnSpeed;
		rhythmGameController.beatTravelSpeed = beatTravelSpeed;

		// Load correct Rhythm Game Animator and set Animation frames
		rhythmGameController.beatProgressVisual.GetComponentInChildren<Animator>().runtimeAnimatorController = Resources.Load(resourceName) as RuntimeAnimatorController;
		rhythmGameController.beatVisualAnimationStates = beatVisualAnimationStates;
	}

	// Elize Abilities
	// -------------------------------------------------------------------------------------------------------------

	public void DrainBlood () {
		InflictDamage (combatController.targets, 10, 0, "Tainted");
		//Heal (combatController.targets, 5);
	}

	public void DrainBlood_rS () {
		SetBeatInformation(5f, 10, 1f, 160f, "Animators/DrainBlood", 5f);
	}

	public void UnholyMending () {
		Heal (combatController.targets, 10);
	}

	public void UnholyMending_rS () {
		SetBeatInformation(8f, 12, 1f, 160f, "Animators/UnholyMending", 6f);
	}

	public void Debilitate () {
		InflictStatus (combatController.targets, "Debuff: Defense", 3, "Tainted");
	}

	public void Debilitate_rS() {
	}

	public void QuickeningPulse () {
		InflictStatus (combatController.targets, "Buff: Haste", 1, "Tainted");
	}

	public void QuickeningPulse_rS () {
	}

	// Angelo Abilities
	// -------------------------------------------------------------------------------------------------------------

	public void ShoulderBlade () {
		InflictDamage (combatController.targets, 10, 5, "Undying");
	}

	public void GraveInsult () {
		InflictStatus (combatController.targets, "Taunt", 3, "Organic");
	}

	public void Skullcracker () {
		InflictDamage (combatController.targets, 10, 5, "Undying");
		InflictStatus (combatController.targets, "Debuff: Defense", 3, "Undying");
	}

	public void Skullcracker_rS () {
		SetBeatInformation(5f, 10, 1f, 180f, "Animators/Skullcracker", 1f);
	}
		
	public void RibCage () {
		InflictStatus (combatController.targets, "DefPlus", 3, "Undying");
	}

	public void RibCage_rS () {
		SetBeatInformation(4f, 5, 1f, 160f, "Animators/RibCage", 4f);
	}
		
	public void ShoulderBlade_rS () {
		InflictDamage (combatController.targets, 10, 5, "Undying");
	}

	public void GraveInsult_rS () {
		InflictStatus (combatController.targets, "Taunt", 3, "Undying");
	}

	// Frederic Abilities
	// -------------------------------------------------------------------------------------------------------------

	public void Tear () {
		InflictStatus (combatController.targets, "DoT", 3, "Organic");
		InflictDamage (combatController.targets, 4, 0, "Tainted");
	}

	public void Tear_rS () {
		SetBeatInformation(4f, 8, 0.5f, 120f, "Animators/Tear", 2f);
	}

	public void LickWounds () {
		Heal (combatController.targets, 20);
		InflictStatus (combatController.targets, "Debuff: Defense", 1, "Organic");
	}

	public void LickWounds_rS () {
	}

	public void Pounce () {
		InflictStatus (combatController.targets, "Charge", 1, "Organic");
	}

	public void Pounce_rS () {
	}

	public void Howl () {
		InflictStatus (combatController.targets, "DefMin", 3, "Organic");
	}

	public void Howl_rS () {
		SetBeatInformation(6f, 12, 1f, 160f, "Animators/Howl", 3f);
	}

	// Enemy Abilities
	// -------------------------------------------------------------------------------------------------------------

	// LakeDweller
	public void Slam () {
		InflictDamage (combatController.targets, 15, 0, "Organic");
	}

	public void Slam_rS () {
		SetBeatInformation(6f, 6, 1f, 160f, "Animators/Slam", 6f);
	}

	// Heartseeker
	public void Peck () {
		InflictDamage (combatController.targets, 10, 0, "Undying");
	}

	public void Peck_rS () {
		SetBeatInformation(6f, 10, 1f, 160f, "Animators/Howl", 6f);
	}
		
} // End

