using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Friendly : MonoBehaviour {
	CombatController combatController;
	AbilityController abilityController;
	PartyController partyController;
	GameObject UI;

	Text UItext;

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

	bool isDead;

	public GameObject selectedPointer;
	GameObject statusIcon;
	GameObject deathThroesIcon;
		
	public GameObject textTemplate;

	// Sound
	AudioSource audioSource;
	public AudioClip death;
	public AudioClip hit;

	void Start () {
		combatController = GameObject.FindGameObjectWithTag ("CombatController").GetComponent<CombatController> ();
		abilityController = GameObject.FindGameObjectWithTag ("AbilityController").GetComponent<AbilityController> ();
		partyController = GameObject.FindGameObjectWithTag ("PartyController").GetComponent<PartyController> ();
		UI = GameObject.FindGameObjectWithTag ("UI");

		selectedPointer = transform.FindChild ("Selected").gameObject;
		selectedPointer.SetActive (false);

		statusIcon = transform.FindChild ("Status").gameObject;
		deathThroesIcon = transform.FindChild ("DeathThroes").gameObject;

		audioSource = this.GetComponent<AudioSource> ();

		SetAbilitiesAndStats ();

		status = null;
		duration = 0;
		damageModifier = 0;
		defenseModifier = 0;
		deathThroes = 0;

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
		

	void SetAbilitiesAndStats () {
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



		healthMax = health;


		UpdateUIText ();

	}

	void UpdateUIText () {
		UItext.text = health + " / " + healthMax;
	}

	public string[] GetAbilities () {
		return abilityArray;
	}

	void Death () {
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

		audioSource.clip = hit;
		audioSource.Play ();

		UpdateUIText ();

	}


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

	public void InflictStatus (string stat, int dur) {
		status = stat;
		duration = dur;
		//print (this.name + " was inflicted by " + stat);
		SpawnText (stat);
		UpdateStatusIcon ();

		audioSource.clip = hit;
		audioSource.Play ();
	}

	public void UpdateStatusIcon () {

		switch (status) {
		case "DoT":
			statusIcon.GetComponent<SpriteRenderer>().sprite = abilityController.statusIcons [0];
			break;
		case "DefPlus":
			statusIcon.GetComponent<SpriteRenderer>().sprite = abilityController.statusIcons [1];
			break;
			/*
		case "DefMin":
			statusIcon.GetComponent<SpriteRenderer>().sprite = abilityController.statusIcons [0];
			break;
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

	public void SpawnText(string text) {
		GameObject textObject = Instantiate (textTemplate, statusIcon.transform.position, Quaternion.Euler(new Vector3(0f,180f,0f)));
		//textObject.transform.localScale = new Vector3 (0.05f, 0.05f, 0.05f);

		textObject.GetComponent<TextMesh> ().text = text;
	}


}
