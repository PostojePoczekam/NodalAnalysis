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

	private void Update()
	{
		int nodesCount = Recalculate();
		GenerateMatrix(nodesCount);
	}
	public float[] v;

	private void GenerateMatrix(int nodesCount)
	{
		if (nodesCount == 0)
			return;

		float[,] g = new float[nodesCount, nodesCount];
		float[] i = new float[nodesCount];
		v = new float[nodesCount];

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
				i[edge.from] += edge.value;
				i[edge.to] += edge.value;
			}
		}

		float[,] w = new float[nodesCount - 1, nodesCount - 1];
		for (int x = 1; x < nodesCount; x++)
			for (int y = 1; y < nodesCount; y++)
				w[x - 1, y - 1] = g[x, y];

		float determinant = LinearEquationSolver.MatrixDeterminant(w);
		// for (int x = 1; x < nodesCount; x++)
		// {
		float[,] vw = (float[,])w.Clone();
		for (int y = 1; y < nodesCount; y++)
			vw[1 - 1, y - 1] = i[1];
		v[1] = LinearEquationSolver.MatrixDeterminant(vw) / determinant;
		//}

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
