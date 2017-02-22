using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;

public class BeatIndicator : MonoBehaviour {

	// Direction Indicator
	public Sprite beatIndicatorLeft;
	public Sprite beatIndicatorRight;
	public Sprite beatIndicatorUp;
	public Sprite beatIndicatorDown;
	public Sprite beatIndicatorEmpty;

	Image direction1;
	Image direction2;
	Sprite[] directionSprites;

	// left, up, down, right
	KeyCode[] keyCodes;
	public KeyCode key1;
	public KeyCode key2;
	bool twoKeys;

	bool key1Pressed;
	bool key2Pressed;


	// Movement
	GameObject beatGoal;
	public RectTransform beatTarget;

	// Colliders
	bool touchingInner;
	bool touchingMid;
	bool touchingOuter;
	public bool isFirst;
	bool isBeingDestroyed;

	bool hasBeenHit;

	//
	BeatSpawner beatSpawner;

	//
	public UnityEvent KeyPress;

	RhythmGameController rhythmGameController;
	//CombatController combatController;

	KeyCode input;

	void Awake () {
		KeyPress = new UnityEvent ();
	}

	void Start () {

		rhythmGameController = GameObject.FindGameObjectWithTag ("RhythmGameController").GetComponent<RhythmGameController>();
		//combatController = GameObject.FindGameObjectWithTag ("CombatController").GetComponent<CombatController> ();


		// left, up, down, right
		keyCodes = new KeyCode[] {KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow};
		directionSprites = new Sprite[] {beatIndicatorLeft, beatIndicatorUp, beatIndicatorDown, beatIndicatorRight, beatIndicatorEmpty};

		direction1 = this.GetComponent<RectTransform>().FindChild ("Direction1Sprite").GetComponent<Image> ();
		direction2 = this.GetComponent<RectTransform>().FindChild ("Direction2Sprite").GetComponent<Image> ();

		GenerateKeys ();
		beatSpawner = GameObject.FindGameObjectWithTag ("BeatSpawner").GetComponent<BeatSpawner>();

		beatGoal = GameObject.FindGameObjectWithTag ("BeatGoal");
		//beatTarget = beatSpawner.GetComponent<RectTransform>().FindChild("BeatMaxTravel").GetComponent<RectTransform>();

		isBeingDestroyed = false;


		this.GetComponent<RectTransform> ().SetParent (beatSpawner.transform, true);
		this.GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 1f);
	}

	void Update () {

		if (this.GetComponent<RectTransform>().localPosition == beatGoal.GetComponent<RectTransform>().localPosition && !isBeingDestroyed) {
			DestroyBeatIndicator();
		}
			
		// Reset Key Timers
		if (Time.time > rhythmGameController.buttonPressed) {
			key1Pressed = false;
			key2Pressed = false;
		}

		if (Input.anyKeyDown) {

		}

		if (isFirst) {
				// Two Keys
			if (twoKeys) {
				// detect second key
				if (Input.GetKeyDown (key2) && key1Pressed ||
				    Input.GetKeyDown (key2) && Input.GetKeyDown (key1) ||
				    Input.GetKeyDown (key1) && key2Pressed) {
					ResolveKeyPress ();
				}
				// detect first key
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
				// One Key
			else {
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
			
		} // end isFirst

		// Movement towards beatTarget
		float step = rhythmGameController.beatTravelSpeed * Time.deltaTime;
		transform.localPosition = Vector3.MoveTowards(this.GetComponent<RectTransform>().localPosition, beatTarget.localPosition, step);


	}

	void OnGUI() {
		input = Event.current.keyCode;
	}

	void GenerateKeys() {
		// Determine the amount of keys (one or two)
		int rand = Random.Range (0, 10);

		if (rand > 7) {
			twoKeys = true;
		} else {
			twoKeys = false;
		}

		// generate first random key
		int one = Random.Range (0, 4);
		direction1.sprite = directionSprites [one];
		direction2.sprite = directionSprites [4];
		key1 = keyCodes [one];

		// check if a second key must be generated
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

	void ResolveKeyPress () {
		// If key press is resolved correctly: 
		if (isFirst) {
			if (touchingInner) {
				ResolveKeyPress_RemoveObj (3);

			} else if (touchingMid) {
				ResolveKeyPress_RemoveObj (2);

			} else if (touchingOuter) {
				ResolveKeyPress_RemoveObj (1);
			}

			// if player mistake
			else {
				rhythmGameController.UpdateBeatHits (-1);
				print ("good key, too soon");
			}
		}
	}

	void ResolveKeyPress_RemoveObj (int i) {
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

	void DestroyBeatIndicator () {

		isBeingDestroyed = true;
		rhythmGameController.beatsPassed += 1;

		beatSpawner.beatStack.RemoveFirst ();
		GameObject obj = beatSpawner.beatStack.First.Value;
		Debug.Log (obj.name);
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

	void OnTriggerExit (Collider collider) {

	}

	void OnDisable() {
		Destroy (this.gameObject);
	}

	public void SetTouchingTrue (string name) {
		switch (name) {
		case "ColliderInner":
			touchingInner = true;
			//Debug.Log ("ColliderInner enter");
			break;
		case "ColliderMid":
			touchingMid = true;
			//Debug.Log ("ColliderMid enter");
			break;
		case "ColliderOuter":
			touchingOuter = true;
			//Debug.Log ("ColliderOuter enter");
			break;
		default:
			break;
		}
	}

	public void SetTouchingFalse (string name) {
		switch (name) {
		case "ColliderInner":
			touchingInner = false;
			//Debug.Log ("ColliderInner exit");

			break;
		case "ColliderMid":
			touchingMid = false;
			//Debug.Log ("ColliderInner exit");

			break;
		case "ColliderOuter":
			touchingOuter = false;
			//Debug.Log ("ColliderInner exit");
			if (!isBeingDestroyed) {
				DestroyBeatIndicator ();
			}
			break;
		default:
			break;
		}
	}


}
