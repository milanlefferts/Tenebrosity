﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeatSpawner : MonoBehaviour {

	public bool isSpawning;

	public GameObject beatIndicator;

	public LinkedList<GameObject> beatStack;
	GameObject newestBeat;
	bool firstSpawn;
	public int spawned;

	GameObject beatSpawnLocation1;
	GameObject beatSpawnLocation2;
	RectTransform beatSpawnTarget1;
	RectTransform beatSpawnTarget2;


	RhythmGameController rhythmGameController;
	CombatController combatController;

	void Start () {
		isSpawning = false;
		spawned = 0;
		firstSpawn = true;

		combatController = GameObject.FindGameObjectWithTag ("CombatController").GetComponent<CombatController> ();

		rhythmGameController = GameObject.FindGameObjectWithTag ("RhythmGameController").GetComponent<RhythmGameController>();

		beatSpawnLocation1 = this.transform.FindChild ("BeatSpawnLocation1").gameObject;
		beatSpawnLocation2 = this.transform.FindChild ("BeatSpawnLocation2").gameObject;
		beatSpawnTarget1 = this.GetComponent<RectTransform>().FindChild("BeatSpawnTarget1").GetComponent<RectTransform>();
		beatSpawnTarget2 = this.GetComponent<RectTransform>().FindChild("BeatSpawnTarget2").GetComponent<RectTransform>();


		rhythmGameController.BeatEvent.AddListener (SpawnBeatIndicator);
		beatStack = new LinkedList<GameObject> ();



	}

	void Update () {
		if (spawned == rhythmGameController.beatSpawnedTotal && isSpawning) {
			isSpawning = false;
		}
	}

	IEnumerator LastBeatSpawned () {
		yield return new WaitForSeconds (2f);
		combatController.rhythmGameActive = false;
	}
	void OnDisable () {
		beatStack.Clear();
		firstSpawn = true;
	}

	GameObject ChooseBeatSpawnLocation () {
		int rand = Random.Range (0, 2);
		if (rand == 0) {
			return beatSpawnLocation1;
		} else {
			return beatSpawnLocation2;
		}
	}

	RectTransform ChooseBeatTarget (GameObject loc) {
		if (loc.name == "BeatSpawnLocation1") {
			return beatSpawnTarget1;
		} else {
			return beatSpawnTarget2;
		}
	}


	public void SpawnBeatIndicator () {
		if (isSpawning) {
			spawned += 1;

			GameObject loc = ChooseBeatSpawnLocation ();
			newestBeat = Instantiate (beatIndicator, loc.transform.position, loc.transform.rotation);

			RectTransform tar = ChooseBeatTarget (loc);
			newestBeat.GetComponent<BeatIndicator> ().beatTarget = tar;

			beatStack.AddLast (newestBeat);
			if (firstSpawn) {
				newestBeat.GetComponent<BeatIndicator> ().isFirst = true;
				firstSpawn = false;
			}
		}
	}
}
