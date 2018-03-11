using UnityEngine;

public partial class Node {
		
	#region Public Properties
	public string Key {get; private set;}
	public object Data {get; set;}
	public AdjacencyList Neighbors {get; private set;}
	public Node PathParent {get; set;}
	public float X {get; set;}
	public float Y {get; set;}
	public double Lattitude {get; set;}
	public double Longitude {get; set;}
	#endregion
		
	#region Constructors
	public Node (string key, object data) : this (key, data, null) {}
	
	public Node (string key, object data, float x, float y) : this (key, data, x, y, null) {}
	
	public Node (string key, object data, double latitude, double longitude) : this (key, data, latitude, longitude, null) {}
		
	public Node (string key, object data, AdjacencyList neighbors) {
		Key = key;
		Data = data;
		
		if (neighbors == null) {
			Neighbors = new AdjacencyList();
		} else {
			Neighbors = neighbors;
		}
	}
		
	public Node (string key, object data, float x, float y, AdjacencyList neighbors) {
		Key = key;
		Data = data;
		X = x;
		Y = y;
		
		if (neighbors == null) {
			Neighbors = new AdjacencyList();
		} else {
			Neighbors = neighbors;
		}
	}
		
	public Node (string key, object data, double latitude, double longitude, AdjacencyList neighbors) {
		Key = key;
		Data = data;
		Lattitude = latitude;
		Longitude = longitude;
			
		if (neighbors == null) {
			Neighbors = new AdjacencyList();
		} else {
			Neighbors = neighbors;
		}
	}
	#endregion
		
	#region Public methods
		
	#region Add methods
	internal void AddDirected(Node n) {
		AddDirected(new EdgeToNeighbor(n));
	}
		
	internal void AddDirected(Node n, float cost) {
		AddDirected(new EdgeToNeighbor(n, cost));
	}
	/*
	internal void AddDirected(Node n, double cost) {
		AddDirected(new EdgeToNeighbor(n, cost));
	}
	*/
	internal void AddDirected(EdgeToNeighbor e) {
		Neighbors.Add(e);
	}
	#endregion
		
	public override string ToString ()
	{
		return string.Format ("[Node: Key={0}, Data={1}, Neighbors={2}, PathParent={3}, X={4}, Y={5}, Lattitude={6}, Longitude={7}]", Key, Data, Neighbors, PathParent, X, Y, Lattitude, Longitude);
	}
		
	#endregion
}