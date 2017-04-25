using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns, moves and then destroys a blood drop after taking damage.
public class DamageBlooddrop : MonoBehaviour {
	void Start () {
		StartCoroutine (Implode());
		StartCoroutine (MoveDamage());
	}
	
	IEnumerator Implode () {
		yield return new WaitForSeconds (1.0f);
		Destroy (this.gameObject);
	}

	IEnumerator MoveDamage() {
		Vector3 startPosition;
		while(true)
		{
			startPosition = transform.position;
			transform.position = Vector3.MoveTowards(startPosition, new Vector3 (startPosition.x, startPosition.y + 5, startPosition.z), 10.0f*Time.deltaTime);
			yield return null;
		}
	}
}
