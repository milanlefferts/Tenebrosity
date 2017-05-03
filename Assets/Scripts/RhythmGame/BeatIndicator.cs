using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;

// Attached to beat indicators, controlling their movement and colliders
public class BeatIndicator : MonoBehaviour {
	// Direction Indicator
	public Sprite beatIndicatorLeft, beatIndicatorRight, beatIndicatorUp, beatIndicatorDown, beatIndicatorEmpty;

	Image direction1, direction2;
	Sprite[] directionSprites;

	// Key Presses
	KeyCode[] keyCodes;
	public KeyCode key1, key2;
	bool twoKeys, key1Pressed, key2Pressed;

	// Movement of notes/beats
	GameObject beatGoal;
	public RectTransform beatTarget;

	// Colliders
	private bool touchingInner, touchingMid, touchingOuter;
	private bool isFirst;

	[SerializeField]
	private bool isBeingDestroyed, hasBeenHit;

	BeatSpawner beatSpawner;
	public UnityEvent KeyPress;

	RhythmGameController rhythmGameController;
	//CombatController combatController;

	KeyCode input;

	void Awake () {
		KeyPress = new UnityEvent ();
	}

	void Start () {
		rhythmGameController = GameObject.FindGameObjectWithTag ("RhythmGameController").GetComponent<RhythmGameController>();

		keyCodes = new KeyCode[] {KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow};
		directionSprites = new Sprite[] {beatIndicatorLeft, beatIndicatorUp, beatIndicatorDown, beatIndicatorRight, beatIndicatorEmpty};

		direction1 = this.GetComponent<RectTransform>().FindChild ("Direction1Sprite").GetComponent<Image> ();
		direction2 = this.GetComponent<RectTransform>().FindChild ("Direction2Sprite").GetComponent<Image> ();

		GenerateKeys ();
		beatSpawner = GameObject.FindGameObjectWithTag ("BeatSpawner").GetComponent<BeatSpawner>();

		beatGoal = GameObject.FindGameObjectWithTag ("BeatGoal");

		isBeingDestroyed = false;

		this.GetComponent<RectTransform> ().SetParent (beatSpawner.transform, true);
		this.GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 1f);
	}

	void Update () {
		if (this.GetComponent<RectTransform>().localPosition == beatGoal.GetComponent<RectTransform>().localPosition && !isBeingDestroyed) {
			MissedBeatIndicator();
		}
			
		// Reset Key Timers
		if (Time.time > rhythmGameController.buttonPressed) {
			key1Pressed = false;
			key2Pressed = false;
		}

		// If the BeatIndicator is in the first position of the stack, detects the keypresses
		if (isFirst) {
				// Two Keys
			if (twoKeys) {
				TwoKeysDown ();
			}
				// One Key
			else {
				OneKeyDown ();
			}
		}
		// Movement towards beatTarget
		float step = rhythmGameController.beatTravelSpeed * Time.deltaTime;
		transform.localPosition = Vector3.MoveTowards(this.GetComponent<RectTransform>().localPosition, beatTarget.localPosition, step);
	}

	void OneKeyDown() {
		if (Input.anyKeyDown) {
			if (input == key1) {
				print ("YEAH one");
				ResolveKeyPress ();
			} else if (input != key1) {
				print ("FAIL one");
				rhythmGameController.UpdateBeatHits (-1);
			}
		}
	}

	void TwoKeysDown() {
		// Detect second key
		if (Input.GetKeyDown (key2) && key1Pressed ||
			Input.GetKeyDown (key2) && Input.GetKeyDown (key1) ||
			Input.GetKeyDown (key1) && key2Pressed) {
			ResolveKeyPress ();
		}
		// Detect first key
		else if (Input.GetKeyDown (key1) && !key2Pressed) {
			key1Pressed = true;
			rhythmGameController.buttonPressed = Time.time + rhythmGameController.buttonWindow;
		} else if (Input.GetKeyDown (key2) && !key1Pressed) {
			key2Pressed = true;
			rhythmGameController.buttonPressed = Time.time + rhythmGameController.buttonWindow;
		} else if (Input.anyKeyDown) {
			print ("FAIL two");
			rhythmGameController.UpdateBeatHits (-1);
		}
	}

	// Detects the input of keycodes through events
	void OnGUI() {
		input = Event.current.keyCode;
	}

	// Generates required keypresses for this BeatIndicator
	void GenerateKeys() {
		// Determine the amount of keys (one or two)
		int rand = Random.Range (0, 10);
		if (rand > 7) {
			twoKeys = true;
		} else {
			twoKeys = false;
		}

		// Generate first random key
		int one = Random.Range (0, 4);
		direction1.sprite = directionSprites [one];
		direction2.sprite = directionSprites [4];
		key1 = keyCodes [one];

		// Check if a second key must be generated
		if (twoKeys) {
			// remove previously chosen key from array
			int[] intArray = {0, 1, 2, 3};
			intArray = intArray.Where(val => val != one).ToArray();
			rand = Random.Range(0, intArray.Length-1);

			// generate second random key
			int two = intArray[rand];
			direction2.sprite = directionSprites [two];
			key2 = keyCodes [two];
		}
	}

	// If the key press is resolved correctly
	void ResolveKeyPress () {
		if (isFirst) {
			if (touchingInner) {
				HitBeatIndicator (3);

			} else if (touchingMid) {
				HitBeatIndicator (2);

			} else if (touchingOuter) {
				HitBeatIndicator (1);
			}

			// if player mistake
			else {
				rhythmGameController.UpdateBeatHits (-1);
				print ("good key, too soon");
			}
		}
	}

	// When BeatIndicator is hit correctly
	void HitBeatIndicator (int i) {
		isBeingDestroyed = true;
		rhythmGameController.beatsPassed += 1;

		// Update progress
		rhythmGameController.UpdateBeatHits (i);

		// Flash Beat Goal White
		beatGoal.GetComponent<BeatGoal>().Flash();

		// Cycle to next Beat Indicator in the stack
		beatSpawner.beatStack.RemoveFirst ();
		GameObject obj = beatSpawner.beatStack.First.Value;
		obj.GetComponent<BeatIndicator> ().isFirst = true;

		// Destroy Beat Indicator
		Destroy (this.gameObject);
	}

	// When BeatIndicator is missed
	void MissedBeatIndicator () {
		isBeingDestroyed = true;
		rhythmGameController.beatsPassed += 1;
		beatSpawner.beatStack.RemoveFirst ();
		GameObject obj = beatSpawner.beatStack.First.Value;
		obj.GetComponent<BeatIndicator> ().isFirst = true;
		rhythmGameController.UpdateBeatHits (-1);
		Destroy (this.gameObject);
	}

	void OnTriggerEnter (Collider collider) {
		switch (collider.name) {
		case "ColliderInner":
			touchingInner = true;
			break;
		case "ColliderMid":
			touchingMid = true;
			break;
		case "ColliderOuter":
			touchingOuter = true;
			break;
		default:
			break;
		}
	}

	void OnDisable() {
		Destroy (this.gameObject);
	}

	public void SetTouchingTrue (string name) {
		switch (name) {
		case "ColliderInner":
			touchingInner = true;
			break;
		case "ColliderMid":
			touchingMid = true;
			break;
		case "ColliderOuter":
			touchingOuter = true;
			break;
		default:
			break;
		}
	}

	public void SetTouchingFalse (string name) {
		switch (name) {
		case "ColliderInner":
			touchingInner = false;
			break;
		case "ColliderMid":
			touchingMid = false;
			break;
		case "ColliderOuter":
			touchingOuter = false;
			if (!isBeingDestroyed) {
				MissedBeatIndicator ();
			}
			break;
		default:
			break;
		}
	}


} // End