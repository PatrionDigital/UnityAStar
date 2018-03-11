using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AStar : MonoBehaviour{
	static public Graph nodeGraph = new Graph();
	
	static public Path<Node> FindPath<Node> (Node start, Node destination, Func<Node,Node,double> distance, Func<Node,double> estimate) where Node : IHasNeighbors<Node> {
		var closed = new List<Node>();
				
		var queue = new PriorityQueue<double, Path<Node>>();
				
		queue.Enqueue(0, new Path<Node>(start));
				
		while (!queue.IsEmpty) {
			var path = queue.Dequeue();
			if (closed.Contains(path.LastStep))
				continue;
			if (path.LastStep.Equals(destination))
            {
                return path;
            }

            closed.Add(path.LastStep);
					
			foreach (Node n in path.LastStep.Neighbours) {
				double d = distance(path.LastStep, n);
						
				var newPath = path.AddStep(n, d);
						
				queue.Enqueue(newPath.TotalCost + estimate(n), newPath);	
			}
		}
		return null;
	}

    /// <summary>
    /// Adds the tile node to the graph.
    /// </summary>
    /// <param name='col'>
    /// Column position
    /// </param>
    /// <param name='row'>
    /// Row position
    /// </param>
    /// <param name='name'>
    /// Name of the tile node.
    /// </param>
    static public void AddTileNode(int col, int row, GameObject tile) {
        Node graphNode = new Node(tile.transform.name, tile, col, row);
        if (!nodeGraph.Contains(graphNode))
        {
            nodeGraph.AddNode(graphNode);
        }
        return;
	}

	/// <summary>
	/// Removes the tile node from the graph.
	/// </summary>
	/// <param name='name'>
	/// Name of the tile to be removed.
	/// </param>
	static public void RemoveTileNode(string name) {
		if (nodeGraph.Contains(name)) {
			Node toBeRemoved = nodeGraph.Nodes[name];
			nodeGraph.Nodes.Remove(toBeRemoved);
		}
	}
	/// <summary>
	/// Creates the vertices between floor tiles.
	/// </summary>
	/// <param name='name'>
	/// Name of the tile to connect the neighbours too.
	/// </param>
	/// <param name='neighbours'>
	/// ArrayList of the neighbour tiles.
	/// </param>
	static public void AddTileNeighbours(string name, ArrayList neighbours) {
		foreach (GameObject tile in neighbours) {
			if (nodeGraph.Contains(tile.transform.name)) {
				nodeGraph.AddUndirectedEdge(nodeGraph.Nodes[name], nodeGraph.Nodes[tile.transform.name], 1);
			}
		}
	}
	
	/// <summary>
	/// Removes the tile neighbours.
	/// </summary>
	/// <param name='name'>
	/// Name.
	/// </param>
	/// <param name='neighbours'>
	/// Neighbours.
	/// </param>
	static public void RemoveTileNeighbours(string name) {
		Node n = AStar.nodeGraph.Nodes[name]; 	// This is the node we want to remove from the graph.
		
		if (n.Neighbors != null) {
			foreach (EdgeToNeighbor etn in n.Neighbors) {
				//Debug.Log(etn.Neighbor);
				// Pull out the neighbor list from each of these nodes.
				Node nn = etn.Neighbor;
				for (int i = 0; i < nn.Neighbors.Count; i++) {
					if ( nn.Neighbors[i].Neighbor.Key == name ) {
						nn.Neighbors.Remove(nn.Neighbors[i]);
					}
				}
			}
			n.Neighbors.Clear();
		}
	}
	
}
	
sealed partial class Node : IHasNeighbors<Node> {
	public IEnumerable<Node> Neighbours {
		get {
			List<Node> nodes = new List<Node>();
			foreach(EdgeToNeighbor etn in Neighbors) {
				nodes.Add(etn.Neighbor);
			}
			return nodes;
		}
	}
}