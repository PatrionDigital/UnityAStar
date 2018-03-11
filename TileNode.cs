using UnityEngine;
using System;
using System.Collections;

public class TileNode : MonoBehaviour {
	// Inspector Variables
	public enum TileType {FLOOR = 0, WALL = 1};										// Posible tile types.
	public TileType tileType = TileType.FLOOR; 										// Default to floor type
	
	public string tileName;															// Name of the tile, based on the ColRow
	public int colNum = 0;															// Tile's column position - default 0
	public int rowNum = 0;															// Tile row position - default 0
	
	public Material[] myMaterial = new Material[2]; 								// Materials to use depending on the tile type
	
	public bool amStart = false;													// Toggle to set if this tile is the start node
	public bool amGoal = false;														// Toggle to set if this tile is the goal node

	public ArrayList myNeighbours = new ArrayList();								// Internal list of the Tile's immediate neighbours
	public bool toggleTileType = false;											    // Internal toggle to use when switching tile type state
	
	public delegate void ChangePathHandler(bool pathChanged);						// Delegate to tell the listeners that a tile has been added or removed from the graph.
	public static event ChangePathHandler onPathChanged;

	//private GameObject robot;
	//private RobotScript searcherRobot;
	//private RobotControl searcherRobot;
	
	// Use this for initialization
	void Start () {
		tileName = "c" + colNum.ToString() + "r" + rowNum.ToString();
		this.transform.name = tileName;												// Set the tile's name to it's Row-Col position
		
		if (tileType == TileType.FLOOR) {
			myNeighbours = FindMyNeighbours();										// Get the list of the Tile's neighbours
			AStar.AddTileNode(colNum, rowNum, gameObject);							// Add the Tile to the map graph
			AStar.AddTileNeighbours(tileName, myNeighbours);						// Add the Tile's neighbours as connections.
		}
		
		
		// BAD BAD BAD BAD //
		//robot = GameObject.FindGameObjectWithTag("robot");
		//robot = GameObject.Find("Robot");											// This needs to be fixed to be more generic. Probably something like a list or a group of robots.
		//searcherRobot = robot.GetComponent<RobotScript>();
		//searcherRobot = robot.GetComponent<RobotControl>();
		
	}
	
	// Update is called once per frame
	void Update () {

		//Old code, will need it to work with the previous versions.
		//	if ( !amGoal && amStart ) {
				//AStar.IAmStart(tileName);											// Tell AStar that this is the Start node
				//searcherRobot.IAmStart(tileName);									// Tell the robot that this is the Start node
		//	} else if ( amGoal && !amStart ) {
				//AStar.IAmGoal(tileName);											// Tell Astar that this is the Goal node
				//searcherRobot.IAmGoal(tileName);									// Tell the robot that this is the Goal node
		//	}
		//	toggleGoalStartOn = false;

		if (toggleTileType) {
			switch (tileType) {
			    case TileType.FLOOR:
				    //GetComponent<Renderer>().material = myMaterial[(int)TileType.FLOOR];// Change the Tile's material to Floor material
				    myNeighbours = FindMyNeighbours();									// Get the list of the Tile's neighbours
				    AStar.AddTileNode(colNum, rowNum, gameObject);						// Add the Tile to the map graph
				    AStar.AddTileNeighbours(tileName, myNeighbours);					// Add the Tile's neighbours as connections.
                    if (onPathChanged != null)
				        onPathChanged(true);											// Signal the delegate's listeners that the graph has changed
				    toggleTileType = false;                                             // Switch toggleFileType off
                    break;
			    case TileType.WALL:
				    //GetComponent<Renderer>().material = myMaterial[(int)TileType.WALL];	// Change the Tile's material to Wall material
				    myNeighbours.Clear();												// Empty the list of the Tile's neighbours.
				    AStar.RemoveTileNeighbours(tileName);								// Break the connections to the neighbour tiles.
				    AStar.RemoveTileNode(tileName);										// Remove this tile from the graph.
                    if (onPathChanged != null)
				        onPathChanged(true);											// Signal the delegate's listeners that the graph has changed
				    toggleTileType = false;                                             // Switch toggleFileType off
                    break;
			}
		}
	}
	
	// OnDrawGizmos is called every fram to render debug draw
	void OnDrawGizmos() {
        Vector3 gizmoPosition = new Vector3(transform.localPosition.x - 0.5f, transform.localPosition.y, transform.localPosition.z);
        Gizmos.color = tileType == TileType.FLOOR ? Color.yellow : Color.green;

        Gizmos.DrawSphere(gizmoPosition, 0.15f);
	}
	/// <summary>
	/// Is the am start.
	/// </summary>
	/// <param name='name'>
	/// Name.
	/// </param>
	
	/*public void IAmStart(string name) {
		if (AStar.nodeGraph.Nodes.ContainsKey(name)) {
			start = AStar.nodeGraph.Nodes[name];
		} else {
			Debug.Log("That tile is not in the graph");
		}
	}
	
	/// <summary>
	/// Is the am goal.
	/// </summary>
	/// <param name='name'>
	/// Name.
	/// </param>
	public void IAmGoal(string name) {
		if (AStar.nodeGraph.Nodes.ContainsKey(name) ) {
			goal = AStar.nodeGraph.Nodes[name];
		} else {
			Debug.Log("That tile is not in the graph");
		}
	}
	*/
	private ArrayList FindMyNeighbours() {
		ArrayList neighbourList = new ArrayList();
		
		// This is the HEX-Grid code
		/*
		for ( int i = colNum - 1; i <= colNum + 1; i++) {
			for ( int j = rowNum - 1; j <= rowNum + 1; j++) {						// If the neighbouring tile is not a Wall tile, add it to the Neighbours list.
				if ( GameObject.Find( i.ToString() + j.ToString() ) != null && (i.ToString() + j.ToString()) != tileName && GameObject.Find( i.ToString() + j.ToString() ).GetComponent<TileNode>().tileType != TileType.wall) {
					neighbourList.Add( GameObject.Find( i.ToString() + j.ToString() ) );
				}
			}
		}
		*/
		// This is the SQUARE-Grid code
		for ( int i = colNum - 1; i <= colNum + 1; i++ ) {
			if ( GameObject.Find( "c" + i.ToString() + "r" + rowNum.ToString() ) != null && ( "c" + i.ToString() + "r" + rowNum.ToString()) != tileName && GameObject.Find( "c" + i.ToString() + "r" + rowNum.ToString() ).GetComponent<TileNode>().tileType != TileType.WALL)
					neighbourList.Add( GameObject.Find( "c" + i.ToString() + "r" + rowNum.ToString() ) );
		}
		
		for ( int j = rowNum - 1; j <= rowNum + 1; j++ ) {
			if ( GameObject.Find( "c" + colNum.ToString() + "r" + j.ToString() ) != null && ( "c" + colNum.ToString() + "r" + j.ToString() ) != tileName && GameObject.Find( "c" + colNum.ToString() + "r" + j.ToString() ).GetComponent<TileNode>().tileType != TileType.WALL)
					neighbourList.Add( GameObject.Find( "c" + colNum.ToString() + "r" + j.ToString() ) );
		}
		return neighbourList;
	}
}
