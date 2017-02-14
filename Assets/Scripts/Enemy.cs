using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	CombatController combatController;
	AbilityController abilityController;

	public int health;
	public int healthMax;
	public string[] abilityArray;
	public string status;
	public int duration;
	public string type;
	public string weakness;
	public int deathThroes;

	public int damageModifier;
	public int defenseModifier;

	public GameObject selectedPointer;
	GameObject statusIcon;
	GameObject deathThroesIcon;

	bool isDead;

	public GameObject textTemplate;

	void Start () {
		combatController = GameObject.FindGameObjectWithTag ("CombatController").GetComponent<CombatController> ();
		abilityController = GameObject.FindGameObjectWithTag ("AbilityController").GetComponent<AbilityController> ();

		selectedPointer = transform.FindChild ("Selected").gameObject;
		selectedPointer.SetActive (false);

		statusIcon = transform.FindChild ("Status").gameObject;
		deathThroesIcon = transform.FindChild ("DeathThroes").gameObject;

		SetAbilities ();

		healthMax = health;
		status = null;
		duration = 0;
		damageModifier = 0;
		defenseModifier = 0;
		deathThroes = 0;

	}

	void Update () {
		if (!isDead) {
			if (health < 0) {
				Death ();
			} else if (health > healthMax) {
				health = healthMax;
			}
		}
	}
		

	void SetAbilities () {
		switch (this.name) {
		case "Lakedweller":
			health = 10;
			type = "Tainted";
			abilityArray = new string[] {"Slam"};
			break;
		case "Heartseeker":
			health = 5;
			type = "Undying";
			abilityArray = new string[] {"Peck"};
			break;
		default:
			break;
		}

		// Determine weakness
		switch (type) {
		case "Tainted":
			weakness = "Organic";
			break;
		case "Organic":
			weakness = "Undying";		
			break;
		case "Undying":
			weakness = "Tainted";
			break;
		}

	}



	public string[] GetAbilities () {
		return abilityArray;
	}

	void Death () {
		//SpawnText ("Death");

		isDead = true;
		// store relevant data

		// save experience / item drops

		// Death animation
		this.transform.FindChild("Sprite").gameObject.GetComponent<Animator>().SetTrigger("Death");
		statusIcon.GetComponent<SpriteRenderer>().sprite = null;
		deathThroesIcon.GetComponent<SpriteRenderer>().sprite = null;


		// Adjust turn order
		this.gameObject.tag = "Untagged";
		combatController.UpdateTurnOrder ("NPC");


	}

	public void CheckDeathThroes (string attackerAbilityType) {
		// If character is weak, inflict Death Throes
		if (attackerAbilityType == weakness) {
			SpawnText ("Weakness!");
			deathThroes += 1;
			deathThroesIcon.GetComponent<SpriteRenderer>().sprite = abilityController.statusIcons [2];

		}
	}

	public void Damage (int dmg, int prc, string type) {

		CheckDeathThroes (type);

		int dam = 0;
		if (combatController.activeCharacter.tag == "PC") {
			dam = dmg 
				- defenseModifier 
				+ combatController.activeCharacter.GetComponent<Friendly>().damageModifier
				* deathThroes;
		} 
		// Self damage or DoT
		else {
			dam = dmg;
		}
		if (dam < 0) {
			dam = 0;
		}
		health -= dam;
		GameObject damageIcon = Instantiate (abilityController.bloodDrop, this.transform.position, Quaternion.identity);
		damageIcon.GetComponentInChildren<TextMesh> ().text = dam + "";
	}

	public void Heal (int heal) {
		health += heal;
		if (health > healthMax) {
			health = healthMax;
		}
		GameObject healIcon = Instantiate (abilityController.bloodDrop, this.transform.position, Quaternion.identity);
		healIcon.GetComponentInChildren<TextMesh> ().text = heal + "";
	}

	public void InflictStatus (string stat, int dur) {
		status = stat;
		duration = dur;
		SpawnText (stat);
		//print (this.name + " was inflicted by " + stat);
		UpdateStatusIcon ();
	}

	public void UpdateStatusIcon () {

		switch (status) {
		case "DoT":
			statusIcon.GetComponent<SpriteRenderer>().sprite = abilityController.statusIcons [0];
			break;
		case "DefPlus":
			statusIcon.GetComponent<SpriteRenderer>().sprite = abilityController.statusIcons [1];
			break;
		case "DefMin":
			statusIcon.GetComponent<SpriteRenderer>().sprite = abilityController.statusIcons [1];
			break;
			/*

		case "DamPlus":
			statusIcon.GetComponent<SpriteRenderer>().sprite = abilityController.statusIcons [0];

			break;
		case "DamMin":
			statusIcon.GetComponent<SpriteRenderer>().sprite = abilityController.statusIcons [0];

			break;

		case "HoT":
			statusIcon.GetComponent<SpriteRenderer>().sprite = abilityController.statusIcons [0];
			break;
			*/
		default:
			statusIcon.GetComponent<SpriteRenderer>().sprite = null;

			break;
		}
	}

	public void ResolveStatus () {
		if (status != null && duration > 0) {
			
			duration -= 1;

			switch (status) {
			case "DefPlus":
				defenseModifier = 3;
				print ("+3 Defense");
				break;
			case "DefMin":
				defenseModifier = -3;
				print ("-3 Defense");
				break;
			case "DamPlus":
				damageModifier = 3;
				print ("+3 Damage");
				break;
			case "DamMin":
				damageModifier = -3;
				print ("-3 Damage");
				break;
			case "DoT":
				Damage (1, 0, "");
	
				print ("Rot does 1 damage");
				break;
			case "HoT":
				Heal (1);

				print ("Regenerated 1 health");
				break;
			default:
				break;
			}
		}

		if (duration < 1) {
			print (status + " no longer effects " + this.name);
			status = null;
			damageModifier = 0;
			defenseModifier = 0;
			UpdateStatusIcon ();
		}

	}

	public void SpawnText(string text) {
		GameObject textObject = Instantiate (textTemplate, statusIcon.transform.position, Quaternion.Euler(new Vector3(0f,180f,0f)));
		//textObject.transform.localScale = new Vector3 (0.05f, 0.05f, 0.05f);
		textObject.GetComponent<TextMesh> ().text = text;
	}


}
