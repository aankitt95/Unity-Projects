using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicController : MonoBehaviour {

	public float moveSpeed = 3.0f;
	public float gravity = 9.81f;

	private CharacterController myController;

	void Start(){

		myController = gameObject.GetComponent<CharacterController> ();
	}

	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("Horizontal Input=" + Input.GetAxis ("Horizontal"));
		// Determine how much should move in the z-direction
		Vector3 movementZ = Input.GetAxis ("Vertical") * Vector3.forward * moveSpeed * Time.deltaTime;
		// Determine how much should move in the x-direction
		Vector3 movementX = Input.GetAxis ("Horizontal") * Vector3.right * moveSpeed * Time.deltaTime;
		// Convert combined Vector3 from local space to world space based on the position of the current gameobject (player)
		Vector3 movement = transform.InverseTransformDirection(movementZ+movementX);
		// Apply gravity (so the object will fall if not grounded)
		movement.y -= gravity * Time.deltaTime;

		Debug.Log ("Movement Vector =" + movement);
		// Actually move the character controller in the movement direction
		myController.Move(movement);
	}
}
