using CodeMonkey.Utils;
using UnityEngine;

public class Grid
{
    //Deklaration der größenvariablen
    private int width;

    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;

    //Konstruktor für Breite und Höhe
    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        //Array für Höhe und Breite des Grids
        gridArray = new int[width, height];
        //Array für den Debug Text des Grids
        debugTextArray = new TextMesh[width, height];

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
                debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new UnityEngine.Vector3(cellSize, cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter);

                //Zeichnen von Außenlinien für das Grid (Unten / Links)
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        //Zeichnen von Außenlinien für das Grid (Oben / Rechts)
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

        //Konsolenausgabe zum testen ob Werte in den Grid Zellen dargestellt werden (x2 y1 wert56)
        //SetValue(2, 1, 56);
        //SetValue(1, 1, 66);

        //Konsolenausgabe zum testen ob Gridwerte angezeigt werden (0,0,0,0)
        //Debug.Log(width + " " + height);
    }

    //Konvertieren von X und Y in eine Welt Position
    private UnityEngine.Vector3 GetWorldPosition(int x, int y)
    {
        return new UnityEngine.Vector3(x, y) * cellSize + originPosition;
    }

    //XY für einen bestimmten Vector3 der Welt Position
    private void GetXY(UnityEngine.Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    //Funktion zum setzten eines Wertes in einen Grid Block
    public void SetValue(int x, int y, int value)
    {
        //Test ob der Wert zugelassen ist
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    //Funktion zum ändern der Grid Value durch Mausklick
    public void SetValue(UnityEngine.Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    //Grid Value Koordinaten abrufen
    public int GetValue(int x, int y)
    {
        //Test ob der Wert zugelassen ist
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return 0;
        }
    }

    //Grid Value Welt Position abrufen
    public int GetValue(UnityEngine.Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
}