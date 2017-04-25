using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Flashes a text above the target of an Ability to indicate a new status effect has been inflicted
public class StatusText : MonoBehaviour {
	public float countdown;
	
	void Start () {
		countdown = 1.5f;
		StartCoroutine (Implode());
		StartCoroutine (MoveText());
	}

	// Destroys text after countdown
	IEnumerator Implode () {
		yield return new WaitForSeconds (countdown);
		Destroy (this.gameObject);
	}

	// Moves text upwards
	IEnumerator MoveText() {
		Vector3 startPosition;
		while(true)
		{
			startPosition = transform.position;
			transform.position = Vector3.MoveTowards(startPosition, new Vector3 (startPosition.x, startPosition.y + 1, startPosition.z), 5.0f * Time.deltaTime);
			yield return null;
		}
	}
		
}
