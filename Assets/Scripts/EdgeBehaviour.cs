using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EdgeBehaviour : MonoBehaviour, IPointerClickHandler
{
	public NodeBehaviour from, to;
	public EdgeType type = EdgeType.None;
	public float value = 1;

	//Workaround not to use FindObjectsOfType cuz it's slow
	public static List<EdgeBehaviour> edges { get; private set; } = new List<EdgeBehaviour>();

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
			RecalculateType();

		if (eventData.button == PointerEventData.InputButton.Right)
			Destroy(gameObject);

		if (eventData.button == PointerEventData.InputButton.Middle)
			value = Mathf.Repeat(value, 20) + 1;

		UpdateLabel();
	}

	private void RecalculatePosition()
	{
		transform.position = (from.transform.position + to.transform.position) / 2f;
		transform.right = from.transform.position - to.transform.position;
		(transform as RectTransform).sizeDelta = new Vector2(Vector2.Distance(from.transform.position, to.transform.position), 20);
	}

	private void RecalculateType()
	{
		type = (EdgeType)Mathf.Repeat((int)type + 1, 3);
		foreach (Transform child in transform)
			child.gameObject.SetActive(type != EdgeType.None);

		UpdateLabel();
	}

	private void UpdateLabel()
	{
		if (type == EdgeType.Resistor)
			transform.GetChild(1).GetComponent<Text>().text = value.ToString("0") + "k";
		if (type == EdgeType.Current)
			transform.GetChild(1).GetComponent<Text>().text = value.ToString("0") + "mA";
	}

	private void Update()
	{
		if (!from || !to)
			Destroy(gameObject);
		else
			RecalculatePosition();
	}

	private void Awake()
	{
		if (!edges.Contains(this))
			edges.Add(this);
	}

	private void OnDestroy()
	{
		if (edges.Contains(this))
			edges.Remove(this);
	}
}
