using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyController : MonoBehaviour {

	public int elizeHealthMax;
	public int angeloHealthMax;
	public int fredericHealthMax;

	// Learned Abilities
	public string[] elizeAbilities;
	public string[] angeloAbilities;
	public string[] fredericAbilities;

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






}
