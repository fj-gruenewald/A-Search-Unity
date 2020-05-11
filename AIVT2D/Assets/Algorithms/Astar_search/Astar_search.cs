﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar_search
{
    //Basiskosten (Quadratwurzel von 200 = 14 für Diagonal)
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    //Grid Erstellen
    private Grid<PathNode> grid;

    //Listen
    private List<PathNode> openList;
    private List<PathNode> closedList;

    //
    public Astar_search(int width, int height)
    {
        grid = new Grid<PathNode>(width, height, 10f, Vector3.zero, (Grid<PathNode> grid, int x, int y) => new PathNode(grid, x, y));
    }

    private List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        //Start- Endpunkt
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);

        //Punkte setzten
        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        //Durch alle Punkte gehen (g kosten unendlich, f kosten)
        for(int x = 0; x < grid.GetWidth(); x++)
        {
            for(int y = 0; y < grid.GetHeight(); y++)
            {
                //Kosten setzten
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();

                // Keine Daten von vorherigen Path übernehmen
                pathNode.cameFromNode = null;
            }
        }

        //Kosten für StartNode berechnen
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        //Suchzyklus
        while(openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if(currentNode == endNode)
            {
                //Endpunkt erreicht
                return CalculatePath(endNode);
            }

            //Punkt ist nicht Endpunkt
            openList.Remove(currentNode);
            closedList.Add(currentNode);
        }

    }

    //Nachbarn abrufen
    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();
        //Linke Seite
        if(currentNode.x - 1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
        }

        //Rechte Seite
        if (currentNode.x + 1 < grid.GetWidth())
        {

        }

        //Unten
        if (currentNode.y - 1 >= 0)
        {
            neighbourList.Add(GetNode)
        }

        //Oben
        if (currentNode.y + 1 < grid.GetHeight())
        {
            neighbourList.Add(GetNode)
        }

        return neighbourList;
    }

    //
    private List<PathNode> CalculatePath(PathNode endNode)
    {
        return null;
    }

    //hKosten Berechnen
    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    //CurrentNode berechnen (openList mit geringsten Kosten => Startpunkt ...)
    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        //Punkt mit den niedrigsten Kosten wählen
        PathNode lowestFCostNode = pathNodeList[0];
        for(int i = 1; i < pathNodeList.Count; i++)
        {
            if(pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }
}
