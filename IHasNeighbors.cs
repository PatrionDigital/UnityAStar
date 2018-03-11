using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IHasNeighbors<N> {
	IEnumerable<N> Neighbours {get;}
}