using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the player character in the overworld
public class PlayerController : MonoBehaviour {

	// Movement
	public float speed;
	public float jumpSpeed;
	public float gravity;
	private Vector3 moveDirection = Vector3.zero;
	float h;
	float v;

	// Animation
	string currentAnim;

	// References
	public Animator animator;
	public CharacterController controller;
	public GameObject item;

	void Start() {
		// Movement
		speed = 10.0f;
		jumpSpeed = 8.0f;
		gravity = 20.0f;

		animator = this.gameObject.GetComponentInChildren<Animator> ();
		currentAnim = "Idle";
		controller = GetComponent<CharacterController>();
		item = this.gameObject.transform.Find("Item").gameObject;
	}

	void Update() {
		// Check if character is on the ground
		if(controller.isGrounded) {
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			// Allows player to jump
			if (Input.GetButton("Jump"))
				moveDirection.y = jumpSpeed;
		}

		h = Input.GetAxis("Horizontal");
		v = Input.GetAxis("Vertical");

		// Idle animation
		if (currentAnim != "Idle" && h == 0 && v == 0) {
			animator.SetTrigger("Idle");
			currentAnim = "Idle";
		}
		// Left animation
		else if (currentAnim != "Left" && (h < 0)) {
			animator.SetTrigger("Left");
			currentAnim = "Left";
		}
		// Right animation
		else if (currentAnim != "Right" && (h > 0)) {
			animator.SetTrigger("Right");
			currentAnim = "Right";
		}

		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}


} // End
