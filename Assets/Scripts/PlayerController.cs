using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float speed;
	public float jumpSpeed;
	public float gravity;
	private Vector3 moveDirection = Vector3.zero;

	float h;
	float v;
	public Animator animator;
	string currentAnim;

	CharacterController controller;
	public GameObject item;

	void Start() {
		speed = 10.0f;
		jumpSpeed = 8.0f;
		gravity = 20.0f;
		animator = this.gameObject.GetComponentInChildren<Animator> ();
		currentAnim = "Idle";
		controller = GetComponent<CharacterController>();
		item = this.gameObject.transform.Find("Item").gameObject;
	}

	void Update() {
		if(controller.isGrounded) {
		//if (true) {
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			if (Input.GetButton("Jump"))
				moveDirection.y = jumpSpeed;
			// play jump animation
		}

		h = Input.GetAxis("Horizontal");
		v = Input.GetAxis("Vertical");
		//print (h);

		if (currentAnim != "Idle" && h == 0 && v == 0) {
			// idle
			animator.SetTrigger("Idle");
			currentAnim = "Idle";
		}

		else if (currentAnim != "Left" && (h < 0)) {
			// left
			animator.SetTrigger("Left");
			currentAnim = "Left";
		}

		else if (currentAnim != "Right" && (h > 0)) {
			//right
			animator.SetTrigger("Right");
			currentAnim = "Right";
		}

		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}

	void OnControllerColliderHit(ControllerColliderHit hit){
		if (hit.transform.tag == "Interactable"){
			//hit.transform.SendMessage("SomeFunction", SendMessageOptions.DontRequireReceiver);
			print("hit");
		}

	}
}
