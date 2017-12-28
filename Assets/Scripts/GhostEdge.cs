using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEdge : MonoBehaviour
{
	public void Move(Vector3 from, Vector3 to)
	{
		transform.position = (from + to) / 2f;
		transform.right = from - to;
		(transform as RectTransform).sizeDelta = new Vector2(Vector2.Distance(from, to), 10);
	}

}
