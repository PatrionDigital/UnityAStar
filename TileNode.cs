using UnityEngine;
using System;
using System.Collections;

public class TileNode : MonoBehaviour {
	// Inspector Variables
	public enum TileType {FLOOR = 0, WALL = 1};
	public TileType tileType = TileType.FLOOR;
	
	public string tileName;
	public int colNum = 0;
	public int rowNum = 0;
	
	// TO BE REMOVED: now using Unity gizmos
	public Material[] myMaterial = new Material[2];
	
	public bool amStart = false;
	public bool amGoal = false;

	public ArrayList myNeighbours = new ArrayList();
	// Currently public so it can be used for testing
	public bool toggleTileType = false;
	
	public delegate void ChangePathHandler(bool pathChanged);
	public static event ChangePathHandler onPathChanged;

	// Use this for initialization
	void Start () {
		tileName = "c" + colNum.ToString() + "r" + rowNum.ToString();
		this.transform.name = tileName;
		
		if (tileType == TileType.FLOOR) {
			myNeighbours = FindMyNeighbours();
			AStar.AddTileNode(colNum, rowNum, gameObject);
			AStar.AddTileNeighbours(tileName, myNeighbours);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (toggleTileType) {
			switch (tileType) {
			    case TileType.FLOOR:
				myNeighbours = FindMyNeighbours();
				AStar.AddTileNode(colNum, rowNum, gameObject);
				AStar.AddTileNeighbours(tileName, myNeighbours);
                    		if (onPathChanged != null)
					onPathChanged(true);
				toggleTileType = false;
                    		break;
			    case TileType.WALL:
				myNeighbours.Clear();
				AStar.RemoveTileNeighbours(tileName);
				AStar.RemoveTileNode(tileName);
                    		if (onPathChanged != null)
					onPathChanged(true);
				toggleTileType = false;
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

	private ArrayList FindMyNeighbours() {
		ArrayList neighbourList = new ArrayList();
		
		// This is the HEX-Grid code
		// i.e. Conceptually it's the same, but allows for diagonal
		// neighbours. Future revision will have a selector at the
		// top level for Square or Hex
		/*
		for ( int i = colNum - 1; i <= colNum + 1; i++) {
			for ( int j = rowNum - 1; j <= rowNum + 1; j++) {
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
