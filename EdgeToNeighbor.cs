using System.Collections;

public class EdgeToNeighbor{
	// private member variables
	private float cost;
	private Node neighbor;
	
	public EdgeToNeighbor(Node neighbor) : this (neighbor, 0) {}
	
	public EdgeToNeighbor(Node neighbor, float cost) {
		this.neighbor = neighbor;
		this.cost = cost;
	}
		
	public virtual float Cost {
		get {
			return cost;
		}
	}
		
	public virtual Node Neighbor {
		get {
			return neighbor;
		}
	}
}