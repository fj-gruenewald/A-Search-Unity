using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar_search {

    //Basiskosten (Quadratwurzel von 200 = 14 für Diagonal)
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static Astar_search Instance { get; private set; }

    //Grid Erstellen
    private Grid<AstarPathNode> grid;

    //Listen
    private List<AstarPathNode> openList;
    private List<AstarPathNode> closedList;

    //Konstruktor für A* Suche
    public Astar_search(int width, int height) {
        Instance = this;
        grid = new Grid<AstarPathNode>(width, height, 10f, Vector3.zero, (Grid<AstarPathNode> g, int x, int y) => new AstarPathNode(g, x, y));
    }

    public Grid<AstarPathNode> GetGrid() {
        return grid;
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition) {
        grid.GetXY(startWorldPosition, out int startX, out int startY);
        grid.GetXY(endWorldPosition, out int endX, out int endY);

        List<AstarPathNode> path = FindPath(startX, startY, endX, endY);
        if (path == null) {
            return null;
        } else {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (AstarPathNode pathNode in path) {
                vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
            }
            return vectorPath;
        }
    }

    public List<AstarPathNode> FindPath(int startX, int startY, int endX, int endY) {

        //Start- Endpunkt
        AstarPathNode startNode = grid.GetGridObject(startX, startY);
        AstarPathNode endNode = grid.GetGridObject(endX, endY);

        if (startNode == null || endNode == null) {
            // Invalid Path
            return null;
        }

        //Punkte setzten
        openList = new List<AstarPathNode> { startNode };
        closedList = new List<AstarPathNode>();

        //Durch alle Punkte gehen (g kosten unendlich, f kosten)
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {

                //Kosten setzten
                AstarPathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = 99999999;
                pathNode.CalculateFCost();

                // Keine Daten von vorherigen Path übernehmen
                pathNode.cameFromNode = null;
            }
        }

        //Kosten für StartNode berechnen
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();
        
        SearchDebugStepVisual.Instance.ClearSnapshots();
        SearchDebugStepVisual.Instance.TakeSnapshot(grid, startNode, openList, closedList);

        //Suchzyklus
        while (openList.Count > 0) {
            AstarPathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode) {
                // Reached final node
                SearchDebugStepVisual.Instance.TakeSnapshot(grid, currentNode, openList, closedList);
                SearchDebugStepVisual.Instance.TakeSnapshotFinalPath(grid, CalculatePath(endNode));

                //Endpunkt erreicht
                return CalculatePath(endNode);
            }

            //Punkt ist nicht Endpunkt
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            //Nachbaren verarbeiten
            foreach (AstarPathNode neighbourNode in GetNeighbourList(currentNode)) {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable) {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost) {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode)) {
                        openList.Add(neighbourNode);
                    }
                }
                SearchDebugStepVisual.Instance.TakeSnapshot(grid, currentNode, openList, closedList);
            }
        }

        //OpenList ist leer
        return null;
    }

    //Nachbarn abrufen
    private List<AstarPathNode> GetNeighbourList(AstarPathNode currentNode) {
        List<AstarPathNode> neighbourList = new List<AstarPathNode>();

        if (currentNode.x - 1 >= 0) {
            //Links
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            //Links Unten
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            //Links Oben
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }
        if (currentNode.x + 1 < grid.GetWidth()) {
            //Rechts
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            //Rechts Unten
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            //Rechts Oben
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }
        //Unten
        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        //Oben
        if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbourList;
    }

    //Aktuellen Node bekommen
    public AstarPathNode GetNode(int x, int y) {
        return grid.GetGridObject(x, y);
    }

    //Calculate Path
    private List<AstarPathNode> CalculatePath(AstarPathNode endNode) {
        List<AstarPathNode> path = new List<AstarPathNode>();
        path.Add(endNode);
        AstarPathNode currentNode = endNode;
        while (currentNode.cameFromNode != null) {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    //hKosten Berechnen
    private int CalculateDistanceCost(AstarPathNode a, AstarPathNode b) {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    //CurrentNode berechnen (openList mit geringsten Kosten => Startpunkt ...)
    private AstarPathNode GetLowestFCostNode(List<AstarPathNode> pathNodeList) {

        //Punkt mit den niedrigsten Kosten wählen
        AstarPathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++) {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost) {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

    //Breitensuche durchführen


}
