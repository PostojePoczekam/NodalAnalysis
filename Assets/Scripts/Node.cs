using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System;

public class Node : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
	public int index { get; private set; }
	private static event Action onNodesChanged;
	private static int _globalIndexer;

	public void OnDrag(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Middle)
			transform.position += (Vector3)eventData.delta;
		else if (eventData.button == PointerEventData.InputButton.Left)
			Factory.instance.ghostEdge.Move(transform.position, eventData.position);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
			Factory.instance.ghostEdge.gameObject.SetActive(true);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
			return;

		Factory.instance.ghostEdge.gameObject.SetActive(false);
		List<RaycastResult> results = new List<RaycastResult>();
		Canvas canvas = FindObjectOfType<Canvas>();
		canvas.GetComponent<GraphicRaycaster>().Raycast(eventData, results);
		if (results.Count == 0)
			return;
		Node node = results[0].gameObject.GetComponent<Node>();
		if (node == null)
			return;
		if (node == this)
			return;
		if (FindObjectsOfType<Edge>().Where(e => (e.from == this && e.to == node) || e.from == node && e.to == this).ToArray().Length != 0)
			return;
		Factory.instance.CreateEdge(this, node);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
			Destroy(gameObject);
	}

	private void Awake()
	{
		_globalIndexer = 0;
		onNodesChanged += RecalcilateIndex;
		Analysis.instance.RegisterNode(this);
		onNodesChanged?.Invoke();
	}

	private void OnDestroy()
	{
		_globalIndexer = 0;
		onNodesChanged -= RecalcilateIndex;
		Analysis.instance.UnregisterNode(this);
		onNodesChanged?.Invoke();
	}

	private void RecalcilateIndex()
	{
		index = _globalIndexer;
		_globalIndexer++;
		GetComponentInChildren<Text>().text = "V" + index.ToString();
	}
}
