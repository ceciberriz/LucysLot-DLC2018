using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//code based on Sebastian Lague's work
//taken from his A* Finding tutorial

public class PlayerMovement : MonoBehaviour {

	[SerializeField]
	float moveSpeed = 10f;

	Vector3 forward, right;

	//for mouse click
	Vector3 targetPosition;
	Vector3 lookAtTarget;
	Quaternion playerRot;
	float rotSpeed = 4f;
	bool moving = false;
	float distanceFromMouse;

	Grid grid;

	void Awake() {
		grid = GetComponent<Grid> ();
	}

	// Use this for initialization
	void Start () {
		forward = Camera.main.transform.forward;
		forward.y = 0;
		forward = Vector3.Normalize (forward);
		//z, y, x
		right = Quaternion.Euler (new Vector3 (0, 90, 0)) * forward;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)) {
			SetTargetPostion ();
		}
		else if (Input.anyKey) { 
			Move ();
		}

		//move until you reach mouse point
		if (moving) {
			MoveToMouse ();
		}
	}

	//rotates character towards mouseClick
	void SetTargetPostion() {
		//var targetPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		//Physics.Raycast sends ray out into environment and returns a hit
		//point if it collides with an object, 1000 is max distance of ray
		if (Physics.Raycast (ray, out hit, 1000)) {
			targetPosition = hit.point;
			//this.transform.LookAt (targetPosition);

			//find vector pointing to mouse click point
			lookAtTarget = new Vector3 (targetPosition.x - transform.position.x, 
				transform.position.y, 
				targetPosition.z - transform.position.z);
			playerRot = Quaternion.LookRotation (lookAtTarget);
			distanceFromMouse = Vector3.Distance (transform.position, targetPosition);
			moving = true;
		}
	}

	void MoveToMouse() {
		transform.rotation = Quaternion.Slerp (transform.rotation,
			playerRot, 
			rotSpeed * Time.deltaTime);

		transform.position = Vector3.MoveTowards (transform.position, 
			targetPosition,
			moveSpeed * Time.deltaTime);
		if (transform.position == targetPosition) {
			moving = false;
		}
	}

	void Move(){
		//positive d = 1.0, a = -1.0
		Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("HorizontalKey");
		Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis ("VerticalKey");

		Vector3 heading = Vector3.Normalize (rightMovement + upMovement);

		//rotation, transform.forward is z axis in world space
		transform.forward = heading;

		//movement
		transform.position += rightMovement;
		transform.position += upMovement;
	}


}