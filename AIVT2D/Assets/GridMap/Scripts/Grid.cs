using CodeMonkey.Utils;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

//Verwendet ein Generic (TGridObject)
public class Grid<TGridObject>
{
    //Deklaration der größenvariablen
    private int width;

    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;

    //Events für das ändern von Werten im Grid
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    //Konstruktor für Breite und Höhe
    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        //Array für Höhe und Breite des Grids
        gridArray = new TGridObject[width, height];

        //Setzten von Default Values in Leere Grid Elemente
        for(int x = 0; x < gridArray.GetLength(0); x++)
        {
            for(int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        //Debug 
        bool showDebug = false;
        if (showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            //Durch Array gehen (1. Dimension)
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                //Durch Array gehen (2. Dimension)
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    //Konsolenausgabe zum testen der Koordinaten (01, 10)
                    //Debug.Log(x + "," + y);

                    //!!!Utils noch auflösen und hier einbinden
                    //Datenwerte zu Grid in der Welt anzeigen ()
                    debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new UnityEngine.Vector3(cellSize, cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter);

                    //Zeichnen von Außenlinien für das Grid (Unten / Links)
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            //Zeichnen von Außenlinien für das Grid (Oben / Rechts)
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            //
            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };

            //Konsolenausgabe zum testen ob Werte in den Grid Zellen dargestellt werden (x2 y1 wert56 / x1 y1 wert66)
            //SetValue(2, 1, 56);
            //SetValue(1, 1, 66);

            //Konsolenausgabe zum testen ob Gridwerte angezeigt werden (0,0,0,0)
            //Debug.Log(width + " " + height);
        }
    }

    //Node Breite
    public int GetWidth()
    {
        return width;
    }

    //Node Höhe
    public int GetHeight()
    {
        return height;
    }

    //Node Zellengröße
    public float GetCellSize()
    {
        return cellSize;
    }

    //Konvertieren von X und Y in eine Welt Position
    public UnityEngine.Vector3 GetWorldPosition(int x, int y)
    {
        return new UnityEngine.Vector3(x, y) * cellSize + originPosition;
    }

    //XY für einen bestimmten Vector3 der Welt Position
    public void GetXY(UnityEngine.Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    //Funktion zum setzten eines Wertes in einen Grid Block
    public void SetGridObject(int x, int y, TGridObject value)
    {
        //Test ob der Wert zugelassen ist
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    //Event zum ändern von Grid Objects (Grid Values)
    public void TriggerGridObjectChanged(int x, int y)
    {
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }

    //Funktion zum ändern der Grid Value durch Mausklick
    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    //Grid Value Koordinaten abrufen
    public TGridObject GetGridObject(int x, int y)
    {
        //Test ob der Wert zugelassen ist
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    //Grid Value Welt Position abrufen
    public TGridObject GetGridObject(UnityEngine.Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }
}