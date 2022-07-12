using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class Pathfinding : MonoBehaviour
{

	

	Grid grid;

	public LineRenderer line;

	public GameObject stopButton;
	private bool isPathBuilding;
	void Awake()
	{
		grid = GetComponent<Grid>();
	}

	private double distance;
	public Text distanceText;

	public void FindPath(Vector3 startPos, Vector3 targetPos)
	{
			distance = 0;
			Node startNode = grid.NodeFromWorldPoint(startPos);
			Node targetNode = grid.NodeFromWorldPoint(targetPos);

			Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{
				Node currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);

				if (currentNode == targetNode)
				{
					RetracePath(startNode, targetNode);
					distanceText.text = "Расстояние - " + ((int)distance).ToString() + " м.";
					return;
				}

				foreach (Node neighbour in grid.GetNeighbours(currentNode))
				{
					if (!neighbour.walkable || closedSet.Contains(neighbour))
					{
						continue;
					}

					int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
					{
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = currentNode;

						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
						else
						{
							openSet.UpdateItem(neighbour);
						}
					}
				}

			}
	

	}

	Vector3[] RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();

		Node currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();
	


		grid.path = path;

		Vector3[] waypoints = SimplifyPath(path);
		Array.Reverse(waypoints);
		DisplayPath(waypoints, endNode, startNode);
		return waypoints;

	}

	Vector3[] SimplifyPath(List<Node> path) {
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;

		for (int i = 1; i < path.Count; i++)
		{
			distance += 0.8;
			Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
			if (directionNew != directionOld)
			{
				waypoints.Add(path[i].worldPosition);
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}

	int GetDistance(Node nodeA, Node nodeB)
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}



	private void DisplayPath(Vector3[] path, Node targetNode, Node startNode)
	{


		Vector3[] points = path;

		line.startWidth = 4f;
		line.endWidth = 4f;

		line.positionCount = points.Length + 1;


		line.SetPosition(0, new Vector3(targetNode.worldPosition.x, 0.9f, targetNode.worldPosition.z));
		for (int i = 1; i < points.Length; i++)
		{
			line.SetPosition(i, new Vector3(points[i - 1].x, 0.9f, points[i - 1].z));
		}

		line.SetPosition(points.Length, new Vector3(startNode.worldPosition.x, 0.9f, startNode.worldPosition.z));

	}

	
}
