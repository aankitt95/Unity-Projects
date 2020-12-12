using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTargetMover : MonoBehaviour {
	//Define the possible states by enum
	public enum motionDirections
	{Spin,
	Horizontal,
	Vertical};
	public motionDirections motionState = motionDirections.Horizontal;

	public float spinSpeed = 180.0f;
	public float motionMagnitude = 0.1f;

	public bool doSpin = true;
	public bool doMotion = false;

	// Update is called once per frame
	void Update () {
		switch(motionState){
		case motionDirections.Spin:		
			//Rotate around y-axis
			gameObject.transform.Rotate (Vector3.up * spinSpeed * Time.deltaTime);
			break;

		case motionDirections.Horizontal:
			//move left and right over time
			gameObject.transform.Translate (Vector3.right * Mathf.Cos (Time.timeSinceLevelLoad) * motionMagnitude);
			break;

		case motionDirections.Vertical:
			//move up and down over time
			gameObject.transform.Translate (Vector3.up * Mathf.Cos (Time.timeSinceLevelLoad) * motionMagnitude);
			break;
		
		}
	}
}
