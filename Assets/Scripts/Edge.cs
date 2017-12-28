using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Edge : MonoBehaviour, IPointerClickHandler
{
	public Node from, to;

	private void Update()
	{
		if (!from || !to)
			Destroy(gameObject);
		else
			RecalculatePosition();

	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
			Destroy(gameObject);
	}

	private void RecalculatePosition()
	{
		transform.position = (from.transform.position + to.transform.position) / 2f;
		transform.right = from.transform.position - to.transform.position;
		(transform as RectTransform).sizeDelta = new Vector2(Vector2.Distance(from.transform.position, to.transform.position), 10);
	}
}
