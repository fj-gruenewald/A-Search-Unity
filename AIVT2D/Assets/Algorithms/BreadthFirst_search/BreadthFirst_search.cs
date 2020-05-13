using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirst_search
{
    //Grid Erstellen
    private Grid<AstarPathNode> grid;

    //Start und Endpunkt

    //Konstruktor für Breitensuche
    public BreadthFirst_search(int width, int height)
    {
        grid = new Grid<AstarPathNode>(width, height, 10f, Vector3.zero, (Grid<AstarPathNode> g, int x, int y) => new AstarPathNode(g, x, y));
    }

    //Breitensuche durchführen
}
