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

	public List<Edge> _edges = new List<Edge>();

	private void Update()
	{
		int nodesCount = Recalculate();
		GenerateMatrix(nodesCount);
	}

	private void GenerateMatrix(int nodesCount)
	{
		if (nodesCount == 0)
			return;

		float[,] matrix = new float[nodesCount + 1, nodesCount];
		foreach (var edge in _edges)
		{
			if (edge.type == EdgeType.Resistor)
			{
				matrix[edge.from, edge.from] += 1f / edge.value;
				matrix[edge.to, edge.to] += 1f / edge.value;
				matrix[edge.from, edge.to] -= 1f / edge.value;
				matrix[edge.to, edge.from] -= 1f / edge.value;
			}
			if (edge.type == EdgeType.Current)
			{
				matrix[nodesCount, edge.from] += edge.value;
				matrix[nodesCount, edge.to] += edge.value;

			}
		}

		string output = "";
		for (int y = 0; y < nodesCount; y++)
		{
			for (int x = 0; x < nodesCount + 1; x++)
				output = string.Join("|", output, matrix[x, y].ToString());
			output = string.Concat(output, "|\n");
		}

		Debug.Log(output);
	}

	private int Recalculate()
	{
		_edges.Clear();
		int index = 0;
		foreach (var node in NodeBehaviour.nodes)
		{
			node.index = null;
			foreach (var edge in EdgeBehaviour.edges)
				if (edge.type != EdgeType.None && (edge.from == node || edge.to == node))
					if (node.index == null)
						node.index = index++;
		}

		foreach (var edge in EdgeBehaviour.edges)
			if (edge.type != EdgeType.None)
				_edges.Add(new Edge() { from = (int)edge.from.index, to = (int)edge.to.index, type = edge.type, value = edge.value });

		return index;
	}
}
