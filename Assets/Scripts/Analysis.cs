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
	private List<Node> _nodes = new List<Node>();
	private List<Edge> _edges = new List<Edge>();

	public void RegisterNode(Node node)
	{
		if (!_nodes.Contains(node))
			_nodes.Add(node);
	}

	public void UnregisterNode(Node node)
	{
		if (_nodes.Contains(node))
			_nodes.Remove(node);
	}

	public void RegisterEdge(Edge edge)
	{
		if (!_edges.Contains(edge))
			_edges.Add(edge);
	}

	public void UnregisterEdge(Edge edge)
	{
		if (_edges.Contains(edge))
			_edges.Remove(edge);
	}

	private void Update()
	{
		GenerateMatrix();
	}

	private void GenerateMatrix()
	{
		if (_nodes.Count == 0)
			return;
		float[,] matrix = new float[_nodes.Count, _nodes.Count];
		for (int x = 0; x < _edges.Count; x++)
		{
			if (_edges[x].type != Edge.EdgeType.Resistor)
				continue;
			matrix[_edges[x].from.index, _edges[x].from.index] += 1f;
			matrix[_edges[x].to.index, _edges[x].to.index] += 1f;
			matrix[_edges[x].from.index, _edges[x].to.index] += 1f;
			matrix[_edges[x].to.index, _edges[x].from.index] += 1f;
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
}
