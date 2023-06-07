using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGame : MonoBehaviour
{
    public ShapeStorage shapeStorage;
    public int columns = 0;
    public int rows = 0;
    public float squaresgap = 0.1f;
    public GameObject gridsquare;
    public Vector2 startPosition = new Vector2(0.0f, 0.0f);
    public float squareScale = 0.5f;
    public float everySquareOffset = 0.0f;

    private Vector2 _offset = new Vector2(0.0f, 0.0f);
    private List<GameObject> _gridSquares = new List<GameObject>();

    private LineIndicator _lineIndicator;

    private void OnEnable()
    {
        GameEvent.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
    }

    private void OnDisable()
    {
        GameEvent.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
    }

    private void Start()
    {
        _lineIndicator = GetComponent<LineIndicator>();
        CreatGrid();
    }

    private void CreatGrid()
    {
        SpawnGridsquares();
        SetGridSquaresPositions();
    }

    private void SpawnGridsquares()
    {
        //0,1,2,3,4
        //5,6,7,8,9
        int square_index = 0;

        for (var row = 0; row < rows; ++row)
        {
            for (var colum = 0; colum < columns; ++colum)
            {
                _gridSquares.Add(Instantiate(gridsquare) as GameObject);// tao ra cac gridsquare sau do add vao danh sach _gridSquares ( as Gameobject la ep kieu ve doi tuong gameobject)

                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SquareIndex = square_index;
                _gridSquares[_gridSquares.Count -1].transform.SetParent(this.transform);// gọi đến đối tượng tượng cuối cùng trong danh sách _gridSquare , sau đó xét nó làm con của danh sách _gridsquares
                _gridSquares[_gridSquares.Count -1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                _gridSquares[_gridSquares.Count -1].GetComponent<GridSquare>().SetImage(square_index % 2 == 0);
                square_index++;
            }
        }
    }

    private void SetGridSquaresPositions()
    {
        int column_number = 0;
        int row_number = 0;
        Vector2 square_gap_number = new Vector2(0.0f, 0.0f);
        bool row_moved = false;

        var square_rect = _gridSquares[0].GetComponent<RectTransform>();

        _offset.x = square_rect.rect.width * square_rect.transform.localScale.x + everySquareOffset;
        _offset.y = square_rect.rect.height * square_rect.transform.localScale.y + everySquareOffset;


        foreach (GameObject square in _gridSquares)
        {
            if (column_number + 1 > columns)
            {
                square_gap_number.x = 0;
                column_number = 0;
                row_number++;
                row_moved = false;
            }



            var pos_x_offset = _offset.x * column_number + (square_gap_number.x * squaresgap);
            var pos_y_offset = _offset.y * row_number + (square_gap_number.y * squaresgap);

            if (column_number > 0 && column_number % 3 == 0)
            {
                square_gap_number.x++;
                pos_x_offset += squaresgap;
            }
            if (row_number > 0 && row_number % 3 == 0 && row_moved == false)
            {
                row_moved = true;
                square_gap_number.y++;
                pos_y_offset += squaresgap;
            }

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset);
            square.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset, 0.0f);

            column_number++;

        }

    }

    private void CheckIfShapeCanBePlaced()
    {
        var squareIdexes = new List<int>();

        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();
            if(gridSquare.Selected && !gridSquare.squareOccupied)
            {
                squareIdexes.Add(gridSquare.SquareIndex);
                gridSquare.Selected = false;
            }
            
        }

        var currentSelectedShape = shapeStorage.GetCurentSelectedShape();
        if (currentSelectedShape == null) return;

        if(currentSelectedShape.TotalSquareNumber == squareIdexes.Count)
        {
            foreach( var squaIndex in squareIdexes)
            {
                _gridSquares[squaIndex].GetComponent<GridSquare>().PlaceShapeOnBoard();
            }

            var shapeLeft = 0;

            foreach( var shape in shapeStorage.shapeList)
            {
                if(shape.IsOnStartPosition() && shape.IsAnyShapeSquareActive())
                {
                    shapeLeft++;
                }
            }

            if(shapeLeft == 0)
            {
                GameEvent.RequestNewShapes();
            }
            else
            {
                GameEvent.SetShapeInactive();
            }

            CheckIfAnyLinecompleted();
        }
        else
        {
            GameEvent.MoveShapeToStartPostion();
        }

        
    }

    void CheckIfAnyLinecompleted()
    {
        List<int[]> lines = new List<int[]>();

        //columns
        foreach ( var column in _lineIndicator.comlumnIndexs)
        {
            lines.Add(_lineIndicator.GetVerticalLine(column));
        }

        //rows
        for (int row = 0; row < 9; row++)
        {
            List<int> data = new List<int>(9);
            for (var index = 0; index < 9; index++)
            {
                data.Add(_lineIndicator.line_data[row, index]);
            }
            lines.Add(data.ToArray());
           
        }

        var completedLines = CheckSquareCompleted(lines);

        if(completedLines > 2)
        {
            // todo animmation
        }

        //todo add score
        if (completedLines >= 1)
        {
            GameManager.Instance.GainPoint();
        }

        CheckEndGame();
       
    }

    private int CheckSquareCompleted(List<int[]> data)
    {
        List<int[]> completedLines = new List<int[]>();

        var linesCompleted = 0;

        foreach (var line in data)
        {
            var lineCompleted = true;
            foreach (var squareIdex in line)
            {
                var comp = _gridSquares[squareIdex].GetComponent<GridSquare>();
                if(comp.squareOccupied == false)
                {
                    lineCompleted = false;
                }
            }

            if (lineCompleted)
            {
                completedLines.Add(line);
            }
        }

        foreach (var line in completedLines)
        {
            var completed = false;

            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.Deactivate();
                completed = true;
            }

            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.ClearOccupied();
            }

            if (completed)
            {
                linesCompleted++;
            }
        }

        return linesCompleted;

    }

    private void CheckEndGame()
    {
        var validShapes = 0;

        for (var index = 0; index < shapeStorage.shapeList.Count; index++)
        {
            var isShapeActive = shapeStorage.shapeList[index].IsAnyShapeSquareActive();
            if(CheckIfShapeCanBePlacedOnGrid(shapeStorage.shapeList[index]) && isShapeActive)
            {
                shapeStorage.shapeList[index]?.ActivateShape();
                validShapes++;
            }
        }

        if(validShapes == 0)
        {
            Debug.Log("gameOver!");
            GameManager.Instance.GameOver();
        }
    }

    private bool CheckIfShapeCanBePlacedOnGrid(Shape currentShape)
    {
        var _currentShapeData = currentShape.currentShapedata;
        var shapeColumns = _currentShapeData.columns;
        var shapeRows = _currentShapeData.rows;

        List<int> originalShapeFilledUpSquares = new List<int>();
        var squareIndex = 0;

        for (var rowIndex = 0; rowIndex < shapeRows; rowIndex++)
        {
            for (var columnIdex = 0; columnIdex < shapeColumns; columnIdex++)
            {
                if (_currentShapeData.board[rowIndex].column[columnIdex])
                {
                    originalShapeFilledUpSquares.Add(squareIndex);
                }

                squareIndex++;
            }
        }

        if(currentShape.TotalSquareNumber != originalShapeFilledUpSquares.Count)
        {
            Debug.LogError("number of filled square are not the same as the original shape have");
        }

        var squareList = GetAllSquareCombination(shapeColumns, shapeRows);

        bool canBePlaced = false;

        foreach ( var number in squareList)
        {
            bool shapeCanBePlacedOnBoard = true;
            foreach (var squareIndexToCheck  in originalShapeFilledUpSquares)
            {
                var comp = _gridSquares[number[squareIndexToCheck]].GetComponent<GridSquare>();
                if (comp.squareOccupied)
                {
                    shapeCanBePlacedOnBoard = false;
                }
            }

            if (shapeCanBePlacedOnBoard)
            {
                canBePlaced = true;
            }
        }

        return canBePlaced;
    }


    private List<int[]> GetAllSquareCombination(int colums , int rows)
    {
        var squareList = new List<int[]>();
        var lastColumnIndex = 0;
        var lastRowIndex = 0;

        int safeIndex = 0;

        while( lastRowIndex + (rows -1) < 9)
        {
            var rowData = new List<int>();

            for (var row = lastRowIndex; row < lastRowIndex + rows; row++)
            {
                for (var column = lastColumnIndex; column < lastColumnIndex + columns; column++)
                {
                    rowData.Add(_lineIndicator.line_data[row, column]);
                }
            }

            squareList.Add(rowData.ToArray());

            lastColumnIndex++;

            if(lastColumnIndex + (columns -1) >= 9)
            {
                lastRowIndex++;
                lastColumnIndex = 0;
            }

            safeIndex++;
            if(safeIndex > 100)
            {
                break;
            }
        }

        return squareList;

    }
}

