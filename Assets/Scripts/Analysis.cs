using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analysis : MonoBehaviour
{

	public static Analysis instance
	{
		get
		{
			if (!_instance)
				_instance = FindObjectOfType<Analysis>();
			return _instance;
		}
	}
	private static Analysis _instance;

	private List<NodeBehaviour> _nodes = new List<NodeBehaviour>();
	private List<EdgeBehaviour> _edges = new List<EdgeBehaviour>();

	private void OnGUI()
	{
		Event e = Event.current;
		if (e.type != EventType.MouseDown)
			return;
		Recalculate();
		GenerateMatrix();
	}

	private void GenerateMatrix()
	{
		if (_nodes.Count == 0)
			return;
		float[,] matrix = new float[_nodes.Count, _nodes.Count];
		for (int x = 0; x < _edges.Count; x++)
		{
			if (_edges[x].type == EdgeType.Resistor)
			{
				// matrix[_edges[x].from.index, _edges[x].from.index] += 1f;
				// matrix[_edges[x].to.index, _edges[x].to.index] += 1f;
				// matrix[_edges[x].from.index, _edges[x].to.index] += 1f;
				// matrix[_edges[x].to.index, _edges[x].from.index] += 1f;
			}
		}

		string output = "";
		for (int y = 0; y < _nodes.Count; y++)
		{
			for (int x = 0; x < _nodes.Count; x++)
				output = string.Join("|", output, matrix[x, y].ToString());
			output = string.Concat(output, "|\n");
		}

		Debug.Log(output);
	}

	private void Recalculate()
	{
		Debug.Log(1);
		// int? index = 0;
		// foreach (var node in Node.nodes)
		// {
		// 	node.index = null;
		// 	foreach (var edge in _edges)
		// 	{
		// 		if (edge.type != Edge.EdgeType.None && (edge.from == node || edge.to == node))
		// 		{
		// 			node.index = index++;
		// 			continue;
		// 		}
		// 	}
		// }
	}
}
