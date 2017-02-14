
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class AbilityMenu : MonoBehaviour {

	GameObject ability1;
	GameObject ability2;
	GameObject ability3;
	GameObject ability4;
	GameObject ability5;
	GameObject ability6;

	public List<GameObject> abilities = new List<GameObject>();

	public GameObject currentAbility;

	public int currentAbilityNr;
	CombatController combatController;

	void Start () {
		//
		combatController = GameObject.FindGameObjectWithTag ("CombatController").GetComponent<CombatController> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			Up ();
		}

		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			Down ();
		}
	}

	public void DetermineAbilities () {
		
		ability1 = transform.FindChild("1").gameObject;
		ability2 = transform.FindChild("2").gameObject;
		ability3 = transform.FindChild("3").gameObject;
		ability4 = transform.FindChild("4").gameObject;
		ability5 = transform.FindChild("5").gameObject;
		ability6 = transform.FindChild("6").gameObject;

		GameObject[] menuEntries = new GameObject[] {ability1, ability2, ability3, ability4, ability5, ability6};

		foreach (GameObject obj in menuEntries) {
			if (obj.activeSelf) {
				abilities.Add (obj);
			}
		}

		currentAbilityNr = 0;
		currentAbility = abilities [currentAbilityNr];
		currentAbility.GetComponent<Image> ().color = new Color32(0, 0, 0, 255);
		currentAbility.transform.FindChild("AbilityName").GetComponent<Text>().color = new Color32(255, 255, 255, 255);

	}

	void Down () {
		currentAbility.GetComponent<Image> ().color = new Color32(255, 255, 255, 100);
		currentAbility.transform.FindChild("AbilityName").GetComponent<Text>().color = new Color32(0, 0, 0, 255);
		if (currentAbilityNr < abilities.Count - 1) {
			currentAbilityNr += 1;
		} else {
			currentAbilityNr = 0;
		}
		currentAbility = abilities [currentAbilityNr];
		currentAbility.GetComponent<Image> ().color = new Color32(0, 0, 0, 255);
		currentAbility.transform.FindChild("AbilityName").GetComponent<Text>().color = new Color32(255, 255, 255, 255);
		combatController.ChangeAbilityDescription ();
	}

	void Up () {
		currentAbility.GetComponent<Image> ().color = new Color32(255, 255, 255, 100);
		currentAbility.transform.FindChild("AbilityName").GetComponent<Text>().color = new Color32(0, 0, 0, 255);
		if (currentAbilityNr > 0) {
			currentAbilityNr -= 1;
		} else {
			currentAbilityNr = abilities.Count - 1;
		}
		currentAbility = abilities [currentAbilityNr];
		currentAbility.GetComponent<Image> ().color = new Color32(0, 0, 0, 255);
		currentAbility.transform.FindChild("AbilityName").GetComponent<Text>().color = new Color32(255, 255, 255, 255);
		combatController.ChangeAbilityDescription ();
	}



}
