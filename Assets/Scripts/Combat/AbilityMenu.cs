using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AbilityMenu : MonoBehaviour {
	// Abilities
	private GameObject ability1, ability2, ability3, ability4, ability5, ability6;
	public GameObject[] abilityEntries;
	public List<GameObject> abilities = new List<GameObject>();
	public GameObject currentAbility;
	public int currentAbilityNr;

	private CombatController combatController;

	void Awake() {
		AssignAbilities();
		abilityEntries = new GameObject[] {ability1, ability2, ability3, ability4, ability5, ability6};
	}

	void Start () {
		combatController = GameObject.FindGameObjectWithTag ("CombatController").GetComponent<CombatController> ();
	}

	void Update () {
		//!!! Must centralized all input
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			Up ();
		}

		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			Down ();
		}
	}

	void AssignAbilities () {
		// !!! Can be changed to manual assignment for clearer code
		ability1 = transform.FindChild("1").gameObject;
		ability2 = transform.FindChild("2").gameObject;
		ability3 = transform.FindChild("3").gameObject;
		ability4 = transform.FindChild("4").gameObject;
		ability5 = transform.FindChild("5").gameObject;
		ability6 = transform.FindChild("6").gameObject;
	}

	// Shows the available abilities of the current character in the menu
	public void DetermineAbilities () {
		AssignAbilities ();
		abilities.Clear ();

		GameObject[] menuEntries = new GameObject[] {ability1, ability2, ability3, ability4, ability5, ability6};

		foreach (GameObject obj in menuEntries) {
			if (obj.activeSelf) {
				abilities.Add (obj);
			}
		}
		currentAbilityNr = 0;
		currentAbility = abilities [currentAbilityNr];
		SetColorSelect ();
	}

	// Move down in the menu
	void Down () {
		SetColorDeselect ();
		if (currentAbilityNr < abilities.Count - 1 && abilities.Count > 1) {
			currentAbilityNr += 1;
		} else {
			currentAbilityNr = 0;
		}
		HighlightSelectedAbility ();
	}

	// Move up in the menu
	void Up () {
		SetColorDeselect ();
		if (abilities.Count == 1) {
			currentAbilityNr = 0;
		}
		else if (currentAbilityNr > 0) {
			currentAbilityNr -= 1;
		} else {
			currentAbilityNr = abilities.Count - 1;
		}
		HighlightSelectedAbility ();
	}

	void HighlightSelectedAbility() {
		currentAbility = abilities [currentAbilityNr];
		SetColorSelect ();
		combatController.ChangeAbilityDescription ();
		combatController.audioSource.Play ();
	}

	void SetColorDeselect() {
		currentAbility.GetComponent<Image> ().color = new Color32(255, 255, 255, 100);
		currentAbility.transform.FindChild("AbilityName").GetComponent<Text>().color = new Color32(0, 0, 0, 255);
	}

	void SetColorSelect() {
		currentAbility.GetComponent<Image> ().color = new Color32(0, 0, 0, 255);
		currentAbility.transform.FindChild("AbilityName").GetComponent<Text>().color = new Color32(255, 255, 255, 255);
	}
		
} // End
