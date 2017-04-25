using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Extends Character base class for enemies
public class Enemy : Character {

	void Start () {
		combatController = GameObject.FindGameObjectWithTag ("CombatController").GetComponent<CombatController> ();
		abilityController = GameObject.FindGameObjectWithTag ("AbilityController").GetComponent<AbilityController> ();

		selectedPointer = transform.FindChild ("Selected").gameObject;
		selectedPointer.SetActive (false);

		statusIcon = transform.FindChild ("Status").gameObject;
		deathThroesIcon = transform.FindChild ("DeathThroes").gameObject;

		audioSource = this.GetComponent<AudioSource> ();

		healthMax = health;

		status = null;
		duration = 0;
		damageModifier = 0;
		defenseModifier = 0;
		deathThroes = 0;

		SetAbilities ();
	}

	void Update () {
		if (!isDead) {
			if (health <= 0) {
				Death ();
			} else if (health > healthMax) {
				health = healthMax;
			}
		}
	}

	void SetAbilities () {
		switch (this.name) {
		case "Lakedweller":
			health = 4;
			type = "Tainted";
			abilityArray = new string[] {"Slam"};
			break;
		case "Heartseeker":
			health = 1;
			type = "Undying";
			abilityArray = new string[] {"Peck"};
			break;
		default:
			break;
		}

		SetWeakness ();
	}

	public override void Death () {
		isDead = true;

		// Death animation
		this.transform.FindChild("Sprite").gameObject.GetComponent<Animator>().SetTrigger("Death");
		statusIcon.GetComponent<SpriteRenderer>().sprite = null;
		deathThroesIcon.GetComponent<SpriteRenderer>().sprite = null;

		// Death sound
		audioSource.clip = death;
		audioSource.Play ();

		// Adjust turn order
		this.gameObject.tag = "Untagged";
		combatController.UpdateTurnOrder ("NPC");
	}

	public override void Damage (int dmg, int prc, string type) {
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

		audioSource.clip = hit;
		audioSource.Play ();
	}

	public override void InflictStatus (string stat, int dur, string type) {
		if (combatController.activeCharacter.tag == "PC") {
			CheckDeathThroes (type);
		}
		status = stat;
		duration = dur;
		SpawnText (stat);
		UpdateStatusIcon ();

		audioSource.clip = hit;
		audioSource.Play ();
	}
		
} // End
