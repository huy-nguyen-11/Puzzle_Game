using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]

public class ShapeData : ScriptableObject
{
    [System.Serializable]

    public class Row
    {
        public bool[] column;
        private int _size;

       // public Row(){}

        public Row(int size)
        {
            CreateRow(size);
        }

        public void CreateRow(int size)
        {
            _size = size;
            column = new bool[_size];
            ClearRow();
        }

        public void ClearRow()
        {
            for (int i = 0; i < _size; i++)
            {
                column[i] = false;
            }
        }


    }

    public int columns = 0;
    public int rows = 0;
    public Row[] board;

    public void Clear()
    {
        for (var i = 0; i < rows; i++)
        {
            board[i].ClearRow();
        }
    }

    public void CreateNewBoard()
    {
        board = new Row[rows];

        for (var i = 0; i < rows; i++)
        {
            board[i] = new Row(columns);
        }
    }

    public Vector2[] GetBlocks()
    {
        List<Vector2> result = new List<Vector2>();
        for (int i = 0; i < rows; i++)
        {
            var row = board[i];
            for (int j = 0; j < columns; j++)
            {
                var col = row.column[j];
                if (col)
                {
                    result.Add(new Vector2(j, i));
                }
            }
        }
        return result.ToArray();
    }
}

