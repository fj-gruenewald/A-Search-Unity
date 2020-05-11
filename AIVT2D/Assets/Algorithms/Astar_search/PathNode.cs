using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    //Variablen
    private Grid<PathNode> grid;
    private int x;
    private int y;

    //Variablen für Kosten
    public int gCost;
    public int hCost;
    public int fCost;

    //Von welcher Node ist man gekommen
    public PathNode cameFromNode;

    //Generic für Astar_search
    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    //fKosten berechnen
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    //Koordinaten in den Nodes Anzeigen
    public override string ToString()
    {
        return x + "," + y;
    }
}
