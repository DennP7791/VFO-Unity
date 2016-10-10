using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Grid test

public class GridScript : MonoBehaviour
{

    public Rect GridRectangle = new Rect(0, 0, 400, 400);
    public int rows = 5;
    public int columns = 5;

    private bool initialize = true;
    private float bWidth;
    private float bHeight;
    private Cell[] cells = null;

    private class Cell
    {
        Rect rect;
        string text = "";
        GUITexture icon = null;
        int id = -1;

        public Cell(Rect rect, string text)
        {
            this.rect = rect;
            this.text = text;
        }

        public void Draw()
        {
            if (icon)
            {

            }
            else
            {
                GUI.Button(rect, text, GUI.skin.button);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        InitializeCells();
    }


    void InitializeCells()
    {
        if (cells == null)
        {
            bWidth = GridRectangle.width / columns;
            bHeight = GridRectangle.height / rows;

            cells = new Cell[rows * columns];
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    cells[j * columns + i] =
                        new Cell(new Rect(i * bWidth + GridRectangle.x, j * bHeight + GridRectangle.y, bWidth, bHeight), j + ", " + i);
                }
            }
        }
    }

    void OnGUI()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Draw();
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Screen to GUI coordinate
            Vector2 p = new Vector2(Input.mousePosition.x - GridRectangle.x, (Screen.height - Input.mousePosition.y) - GridRectangle.y);
            int row = (int)(p.y / bHeight);
            int col = (int)(p.x / bWidth);
            int idx = row * columns + col;
            Debug.Log(p.ToString() + " --> (" + row + ", " + col + ") --> " + idx);
            if (idx < cells.Length)
            {
                //do something
            }
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
