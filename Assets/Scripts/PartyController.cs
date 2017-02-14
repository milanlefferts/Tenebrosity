using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyController : MonoBehaviour {

	// UI
	GameObject UI;

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
		angeloAbilities = new string[] {"RibCage", "GraveInsult"};
		fredericAbilities = new string[] {"Tear", "Howl"};
	}

	void Start () {
		// UI
		UI = GameObject.FindGameObjectWithTag ("UI");




		/*
		elizeAbilities = new string[] {"DrainBlood", "Debilitate", "UnholyMending", "QuickeningPulse"};
		angeloAbilities = new string[] {"ShoulderBlade", "GraveInsult", "RibCage", "SkullCracker"};
		fredericAbilities = new string[] {"Tear", "PrimalRoar", "LickWounds", "Pounce"};
*/
	}


	/*
	public string[] GetAbilities (string characterName) {
		switch (characterName) {
		case "Elize":
			return elizeAbilities;
		case "Angelo":
			return angeloAbilities;
		case "Frederic":
			return fredericAbilities;
		default:
			return new string[]{};
		}
	}*/




}
