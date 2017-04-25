using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Stores party's health and abilities in between combat.
// Will also store items and stats when they are implemented.
public class PartyController : MonoBehaviour {

	// Health Max
	public int elizeHealthMax, angeloHealthMax, fredericHealthMax;

	// Learned Abilities
	public string[] elizeAbilities, angeloAbilities, fredericAbilities;

	void Awake () {
		// Health Setup
		elizeHealthMax = 40;
		angeloHealthMax = 45;
		fredericHealthMax = 65;

		// Ability Setup
		elizeAbilities = new string[] {"DrainBlood", "UnholyMending"};
		angeloAbilities = new string[] {"RibCage", "Skullcracker"};
		fredericAbilities = new string[] {"Tear", "Howl"};
	}

} // End
