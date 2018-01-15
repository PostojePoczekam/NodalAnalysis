using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System;

public class NodeBehaviour : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
	public int? index { get; set; }

	public float? value
	{
		set
		{
			Text label = GetComponentInChildren<Text>();
			if (value == null)
				label.text = "";
			else
				label.text = ((float)value).ToString("0.0") + "V";
		}
	}

	//Workaround not to use FindObjectsOfType cuz it's slow
	public static List<NodeBehaviour> nodes { get; private set; } = new List<NodeBehaviour>();

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
		NodeBehaviour node = results[0].gameObject.GetComponent<NodeBehaviour>();
		if (node == null || node == this)
			return;
		if (FindObjectsOfType<EdgeBehaviour>().Where(e => (e.from == this && e.to == node) || e.from == node && e.to == this).ToArray().Length != 0)
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
		if (!nodes.Contains(this))
			nodes.Add(this);
	}

	private void OnDestroy()
	{
		if (nodes.Contains(this))
			nodes.Remove(this);
	}
}
