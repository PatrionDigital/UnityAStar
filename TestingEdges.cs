using UnityEngine;
using System.Collections;

public class TestingEdges : MonoBehaviour {
	public bool listGraphToggle = false;
	
	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		if (listGraphToggle) {
			listGraphToggle = false;
			/*
			foreach (Node n in AStar.nodeGraph.Nodes) {
				//Debug.Log(n.Neighbors);
				Debug.Log(n.Neighbors[0]);
				EdgeToNeighbor etn = n.Neighbors[0];
				Debug.Log(etn.Neighbor);
			}
			*/
			Node n = AStar.nodeGraph.Nodes["c8r7"]; 	// This is the node we want to remove from the graph.

			foreach (EdgeToNeighbor etn in n.Neighbors) {
				//Debug.Log(etn.Neighbor);
				// Pull out the neighbor list from each of these nodes.
				Node nn = etn.Neighbor;
				for (int i = 0; i < nn.Neighbors.Count; i++) {
					if ( nn.Neighbors[i].Neighbor.Key == "c8r7" ) {
						nn.Neighbors.Remove(nn.Neighbors[i]);
					}
				}
			}
			n.Neighbors.Clear();
			
			
			
			/*

			for (int i = 0; i < n.Neighbors.Count; i++) {
				EdgeToNeighbor etn = n.Neighbors[i];
				Debug.Log(etn.Neighbor);
			}
			*/
		}
	}
}
