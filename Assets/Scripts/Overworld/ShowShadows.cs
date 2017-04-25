using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Activated ShadowCastingMode, enabling shadows for Sprites
public class ShowShadows : MonoBehaviour {

	void Start () {
		this.gameObject.GetComponent<SpriteRenderer> ().receiveShadows = true;
		this.gameObject.GetComponent<SpriteRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
	}
}
