using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

	bool isMoving = false;
	Vector3 target;
	public float speed = 5;
	Vector3[] path = new Vector3[0];
	int targetIndex;

	// Use this for initialization
	void Start() {
		target = transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)) {
			SetTargetPostion ();
			Debug.Log ("setting isMoving to true...");
			isMoving = true;
		}
		if (isMoving) {
			Move ();
		}
	}

	void Move() {
		PathRequestManager.RequestPath(transform.position, target, OnPathFound);
	}

	//sets position using raycasting and mouse click location
	void SetTargetPostion() {
		//var targetPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		//Physics.Raycast sends ray out into environment and returns a hit
		//point if it collides with an object, 1000 is max distance of ray
		if (Physics.Raycast (ray, out hit, 1000)) {
			target = hit.point;
		}
	}

	public void OnPathFound(Vector3[] newPath, bool pathFound) {
		if (pathFound) {
			path = newPath;
			targetIndex = 0;
			StopCoroutine ("FollowPath");
			StartCoroutine ("FollowPath");
		}
	}

	IEnumerator FollowPath() {
		if (isMoving) {
			
			Debug.Log ("following path..");
			Vector3 currentWaypoint = transform.position;
			if (path.Length > 0) {
				currentWaypoint = path [0];
			}
			while (true) {
				if (transform.position == currentWaypoint) {
					targetIndex++;
					if (targetIndex >= path.Length) {
						yield break;
					}
					currentWaypoint = path [targetIndex];
				}
				Vector3 finalLocation = new Vector3 (currentWaypoint.x, transform.position.y, currentWaypoint.z);
				transform.position = Vector3.MoveTowards (transform.position, finalLocation, speed);
				Debug.Log ("transforming..");
				yield return null;
			}
		}
	}
}


//Debug.Log("setting isMoving to false...");
//isMoving = false;