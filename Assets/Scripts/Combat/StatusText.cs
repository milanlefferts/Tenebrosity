using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusText : MonoBehaviour {
	void Start () {
		StartCoroutine (Implode());
		StartCoroutine (MoveText());

		//this.transform.SetParent (beatSpawner.transform, true);
		//this.GetComponent<RectTransform> ().localScale = new Vector3 (0.15f, 0.15f, 1f);
	}

	IEnumerator Implode () {
		yield return new WaitForSeconds (1.5f);
		Destroy (this.gameObject);
	}

	IEnumerator MoveText() {
		Vector3 startPosition;
		while(true)
		{
			startPosition = transform.position;
			transform.position = Vector3.MoveTowards(startPosition, new Vector3 (startPosition.x, startPosition.y + 1, startPosition.z), 5.0f*Time.deltaTime);
			yield return null;
		}
	}

	void OnDisable() {
		Destroy (this.gameObject);
	}


}
