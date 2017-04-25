using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for Combat characters
public class Character : MonoBehaviour {
	public CombatController combatController;
	public AbilityController abilityController;

	// Stats
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

	public bool isDead;

	// Icons
	public GameObject selectedPointer;
	public GameObject statusIcon;
	public GameObject deathThroesIcon;
	public GameObject textTemplate;

	// Sound
	public AudioSource audioSource;
	public AudioClip death;
	public AudioClip hit;

	// Placeholder for Death method
	public virtual void Death () {
	}

	// Sets weakness of this character
	public void SetWeakness () {
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

	// Gets ability array for this character
	public string[] GetAbilities () {
		return abilityArray;
	}

	// If character is weak vs Ability, inflict Death Throes
	public void CheckDeathThroes (string attackerAbilityType) {
		if (attackerAbilityType == weakness) {
			SpawnText ("Weakness!");
			deathThroes += 1;
			deathThroesIcon.GetComponent<SpriteRenderer>().sprite = abilityController.statusIcons [2];
		}
	}

	// Placeholder for Damage method
	public virtual void Damage (int dmg, int prc, string type) {
	}

	// Heals the character for X
	public void Heal (int heal) {
		health += heal;
		if (health > healthMax) {
			health = healthMax;
		}
		GameObject healIcon = Instantiate (abilityController.bloodDrop, this.transform.position, Quaternion.identity);
		healIcon.GetComponentInChildren<TextMesh> ().text = heal + "";

		UpdateUIText ();

		audioSource.clip = hit;
		audioSource.Play ();

	}

	// Placeholder for Damage method
	public virtual void UpdateUIText () {
	}

	// Placeholder for InflictStatus method
	public virtual void InflictStatus (string stat, int dur, string type) {
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
			statusIcon.GetComponent<SpriteRenderer>().sprite = abilityController.statusIcons [3];
			break;
		default:
			statusIcon.GetComponent<SpriteRenderer>().sprite = null;
			break;
		}
	}

	// Called at start of the Combat turn and resolves status effects
	public void ResolveStatus () {
		if (status != null && duration > 0) {

			duration -= 1;

			switch (status) {
			case "DefPlus":
				defenseModifier = 3;
				break;
			case "DefMin":
				defenseModifier = -3;
				break;
			case "DamPlus":
				damageModifier = 3;
				break;
			case "DamMin":
				damageModifier = -3;
				break;
			case "DoT":
				Damage (1, 0, "");
				break;
			case "HoT":
				Heal (1);
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

	// Spawns Status Text on this character
	public void SpawnText(string text) {
		GameObject textObject = Instantiate (textTemplate, statusIcon.transform.position, Quaternion.Euler(new Vector3(0f,180f,0f)));
		textObject.GetComponent<TextMesh> ().text = text;
	}
		
} // End
