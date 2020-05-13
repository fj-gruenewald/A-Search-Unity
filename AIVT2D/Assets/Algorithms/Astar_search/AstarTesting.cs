using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey;

public class AstarTesting : MonoBehaviour {
    
    [SerializeField] private SearchDebugStepVisual pathfindingDebugStepVisual;
    [SerializeField] private SearchVisual pathfindingVisual;
    private Astar_search astarsearch;

    private void Start() {
        astarsearch = new Astar_search(25, 25);
        pathfindingDebugStepVisual.Setup(astarsearch.GetGrid());
        pathfindingVisual.SetGrid(astarsearch.GetGrid());
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            astarsearch.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            List<AstarPathNode> path = astarsearch.FindPath(0, 0, x, y);
            if (path != null) {
                for (int i=0; i<path.Count - 1; i++) {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i+1].x, path[i+1].y) * 10f + Vector3.one * 5f, Color.green, 5f);
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            astarsearch.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            astarsearch.GetNode(x, y).SetIsWalkable(!astarsearch.GetNode(x, y).isWalkable);
        }
    }

}
