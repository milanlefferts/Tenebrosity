using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sets the overworld's parameters, such as GameState
public class OverworldController : MonoBehaviour {
	GameController gameController;

	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		gameController.SwitchGameState (GameController.GameState.Overworld);
	}

}
