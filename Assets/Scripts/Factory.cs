using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Factory : MonoBehaviour, IPointerClickHandler
{
	public GameObject nodePrefab;
	public GameObject edgePrefab;
	public GhostEdge ghostEdge;

	public static Factory instance;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
			return;
		CreateNode(eventData.position);
	}

	public void CreateNode(Vector3 position)
	{
		GameObject node = Instantiate(nodePrefab);
		node.transform.SetParent(GameObject.Find("Nodes").transform);
		node.transform.position = position;
		node.name = "Node";
		node.SetActive(true);
	}

	public void CreateEdge(Node from, Node to)
	{
		Edge edge = Instantiate(edgePrefab).GetComponent<Edge>();
		edge.transform.SetParent(GameObject.Find("Edges").transform);
		edge.gameObject.name = "Edge";
		edge.gameObject.SetActive(true);
		edge.from = from;
		edge.to = to;
	}

	private void Awake()
	{
		instance = this;
	}
}
