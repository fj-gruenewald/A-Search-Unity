using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarPathNode {

    //Variablen
    private Grid<AstarPathNode> grid;
    public int x;
    public int y;


    //Variablen für Kosten
    public int gCost;
    public int hCost;
    public int fCost;

    //Ist es eine Wand
    public bool isWalkable;

    //Von welcher Node ist man gekommen
    public AstarPathNode cameFromNode;

    //Generic für Astar_search
    public AstarPathNode(Grid<AstarPathNode> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }

    //fKosten berechnen
    public void CalculateFCost() {
        fCost = gCost + hCost;
    }

    //Nodes über die gelaufen werden kann
    public void SetIsWalkable(bool isWalkable) {
        this.isWalkable = isWalkable;
        grid.TriggerGridObjectChanged(x, y);
    }

    //Koordinaten in den Nodes Anzeigen
    public override string ToString() {
        return x + "," + y;
    }

}
