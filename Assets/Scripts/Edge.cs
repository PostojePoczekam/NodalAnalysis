using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Edge : MonoBehaviour, IPointerClickHandler
{
	public Node from, to;
	public EdgeType type = EdgeType.None;

	private void Update()
	{
		if (!from || !to)
			Destroy(gameObject);
		else
			RecalculatePosition();

	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
			RecalculateType();

		if (eventData.button == PointerEventData.InputButton.Right)
			Destroy(gameObject);
	}

	private void RecalculatePosition()
	{
		transform.position = (from.transform.position + to.transform.position) / 2f;
		transform.right = from.transform.position - to.transform.position;
		(transform as RectTransform).sizeDelta = new Vector2(Vector2.Distance(from.transform.position, to.transform.position), 10);
	}

	private void RecalculateType()
	{
		type = (EdgeType)Mathf.Repeat((int)type + 1, 4);
		for (int x = 0; x < transform.childCount; x++)
			transform.GetChild(x).gameObject.SetActive((int)type == x);
	}

	public enum EdgeType
	{
		Resistor = 0,
		Voltage,
		Current,
		None
	}
}
