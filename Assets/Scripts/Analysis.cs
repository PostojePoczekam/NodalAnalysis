using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

	public int val = 0;
	private void Update()
	{
		int nodesCount = CountNodes();
		CalculateNodes(nodesCount);
	}

	private void CalculateNodes(int nodesCount)
	{
		if (nodesCount == 0)
			return;

		float[,] g = new float[nodesCount, nodesCount];
		float[] i = new float[nodesCount];


		foreach (var edge in _edges)
		{
			if (edge.type == EdgeType.Resistor)
			{
				g[edge.from, edge.from] += 1f / edge.value;
				g[edge.to, edge.to] += 1f / edge.value;
				g[edge.from, edge.to] -= 1f / edge.value;
				g[edge.to, edge.from] -= 1f / edge.value;
			}
			if (edge.type == EdgeType.Current)
			{
				i[edge.from] -= edge.value;
				i[edge.to] += edge.value;
			}
		}

		float[,] gx = new float[nodesCount - 1, nodesCount - 1];
		float[] ix = new float[nodesCount - 1];
		for (int x = 1; x < nodesCount; x++)
		{
			for (int y = 1; y < nodesCount; y++)
			{
				gx[x - 1, y - 1] = g[x, y];
			}
			ix[x - 1] = i[x];
		}

		float[] v = LinearSolver.Solve(gx, ix);
		if (v == null)
			return;

		for (int x = 1; x < nodesCount; x++)
			foreach (var node in NodeBehaviour.nodes)
				if (node.index == x)
					node.value = v[x - 1];
	}

	private int CountNodes()
	{
		_edges.Clear();
		int index = 0;
		foreach (var node in NodeBehaviour.nodes)
		{
			node.index = null;
			node.value = null;
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
