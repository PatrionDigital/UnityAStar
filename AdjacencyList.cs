using System.Collections;

public class AdjacencyList : CollectionBase {
	protected internal virtual void Add(EdgeToNeighbor e) {
		base.InnerList.Add(e);
	}
	
	protected internal virtual void Remove(EdgeToNeighbor e) {
		base.InnerList.Remove(e);
	}
		
	public virtual EdgeToNeighbor this [int index] {
		get {
			return (EdgeToNeighbor) base.InnerList[index];
		}
		set {
			base.InnerList[index] = value;
		}
	}
}