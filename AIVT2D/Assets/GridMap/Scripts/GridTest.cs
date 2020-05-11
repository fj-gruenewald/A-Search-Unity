using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridTest : MonoBehaviour
{
    private Grid<bool> grid;
    private void Start()
    {
        grid = new Grid<bool>(100, 25, 10f, Vector3.zero);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
        }
    }
}
