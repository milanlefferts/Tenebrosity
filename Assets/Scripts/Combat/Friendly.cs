using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Extends Character base class for friendly members
public class Friendly : Character {

	PartyController partyController;
	GameObject UI;
	Text UItext;

	void Start () {
		partyController = GameObject.FindGameObjectWithTag ("PartyController").GetComponent<PartyController> ();
		UI = GameObject.FindGameObjectWithTag ("UI");

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
		case "Elize":
			health = partyController.elizeHealthMax;
			abilityArray = partyController.elizeAbilities;
			type = "Tainted";
			UItext = UI.transform.FindChild ("Elize").GetComponent<Text> ();
			break;
		case "Angelo":
			health = partyController.angeloHealthMax;
			abilityArray =  partyController.angeloAbilities;
			type = "Undying";
			UItext = UI.transform.FindChild ("Angelo").GetComponent<Text> ();
			break;
		case "Frederic":
			health = partyController.fredericHealthMax;
			abilityArray =  partyController.fredericAbilities;
			type = "Organic";
			UItext = UI.transform.FindChild ("Frederic").GetComponent<Text> ();
			break;
		default:
			break;
		}
		SetWeakness ();

		UpdateUIText ();
	}

	public override void Death () {
		print (this.name + " died horribly!");
		isDead = true;
		// store relevant data

		// save experience / item drops

		// Death animation
		this.transform.FindChild("Sprite").gameObject.GetComponent<Animator>().SetTrigger("Death");

		// Death sound
		audioSource.clip = death;
		audioSource.Play ();

		// Adjust turn order
		this.gameObject.tag = "Untagged";
		combatController.UpdateTurnOrder ("PC");


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

		UpdateUIText ();

	}
		
	public override void InflictStatus (string stat, int dur, string type) {
		if (combatController.activeCharacter.tag == "NPC") {
			CheckDeathThroes (type);
		}
		status = stat;
		duration = dur;
		SpawnText (stat);
		UpdateStatusIcon ();

		audioSource.clip = hit;
		audioSource.Play ();
	}

	public override void UpdateUIText () {
		UItext.text = health + " / " + healthMax;
	}

} // End
