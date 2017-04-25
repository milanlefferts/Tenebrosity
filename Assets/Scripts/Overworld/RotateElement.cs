using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rotates the object this is attached to.
public class RotateElement : MonoBehaviour {

	void Update () {
		transform.Rotate (Vector3.right * Time.deltaTime * 20f);
		transform.Rotate (Vector3.up * Time.deltaTime * 10f);
	}

}
