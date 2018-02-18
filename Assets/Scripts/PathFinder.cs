using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//code based on Sebastian Lague's work
//taken from his A* Finding tutorial

public class PathFinder : MonoBehaviour {

	PathRequestManager requestManager;
	Grid grid;

	void Awake() {
		grid = GetComponent<Grid> ();
		requestManager = GetComponent<PathRequestManager> ();
	}

	public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
		StartCoroutine(FindPath(startPos, targetPos));
	}

	Node findNewTarget(Node oldTarget) {
		Node newTarget = oldTarget;
		int x = oldTarget.gridX;
		int z = oldTarget.gridY;

		while (!newTarget.walkable) {
			x -= 1; 
			z -= 1; 
			if (x >= 0 && z >= 0) {
				newTarget = grid.getNodeFromXY (x, z);
			} else {
				return oldTarget;
			}
		}
		return newTarget;
	}
		
	IEnumerator FindPath (Vector3 startPosition, Vector3 targetPosition) {
		Vector3[] waypoints = new Vector3[0];
		bool pathFound = false;

		Node startNode = grid.NodeFromWorldPoint (startPosition);
		Node targetNode = grid.NodeFromWorldPoint (targetPosition);

		//if target is unwakable then update to a target
		//that is closer to the camera
		if (!targetNode.walkable) {
			targetNode = findNewTarget (targetNode);
		}

		if (startNode.walkable && targetNode.walkable) {
			List<Node> openSet = new List<Node> ();
			HashSet<Node> closedSet = new HashSet<Node> ();

			openSet.Add (startNode);

			while (openSet.Count > 0) {
				Node currentNode = openSet [0];
				for (int i = 1; i < openSet.Count; i++) {
					if (openSet [i].fCost < currentNode.fCost || (openSet [i].fCost == currentNode.fCost && openSet [i].hCost < currentNode.hCost)) {
						currentNode = openSet [i];
					}
				}

				openSet.Remove (currentNode);
				closedSet.Add (currentNode);

				if (currentNode == targetNode) {
					pathFound = true;
					break;
				}

				foreach (Node neighbour in grid.getNodeNeighbors(currentNode)) {
					//loop through neighbors and check whether the neighbor is not wakable 
					// or is in closed list.
					if (!neighbour.walkable || closedSet.Contains (neighbour)) {
						continue;
					}

					int newMovementCostToNeighbour = currentNode.gCost + GetDistance (currentNode, neighbour);
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains (neighbour)) {
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance (neighbour, targetNode);
						neighbour.parent = currentNode;

						if (!openSet.Contains (neighbour)) {
							openSet.Add (neighbour);
						} 
					}
				}
			}
		}
		yield return null;
		if (pathFound) {
			waypoints = RetracePath (startNode, targetNode);
		}
		requestManager.FinishedProcessingPath (waypoints, pathFound);
	}


	Vector3[] RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node> ();
		Node currentNode = endNode; 

		while (currentNode != startNode) {
			path.Add (currentNode);
			currentNode = currentNode.parent;
		}
		Vector3[] waypoints = SimplifyPath (path);
		Array.Reverse(waypoints);
		return waypoints;
	}

	Vector3[] SimplifyPath(List<Node> path) {
		List<Vector3> waypoints = new List<Vector3> ();
		Vector2 directionOld = Vector2.zero;

		for (int i = 1; i < path.Count; i++) {
			Vector2 directionNew = new Vector2 (path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
			if (directionNew != directionOld) {
				waypoints.Add (path [i].worldPosition);
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}

	int GetDistance(Node nodeA, Node nodeB) {
		int distX = Mathf.Abs (nodeA.gridX - nodeB.gridX);
		int distY = Mathf.Abs (nodeA.gridY - nodeB.gridY);

		if (distX > distY) {
			return 14 * distY + 10 * (distX - distY);
		} else {
			return 14 * distX + 10 * (distY - distX);
		}
	}
}