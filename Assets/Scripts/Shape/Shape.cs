using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public GameObject squareShapeImage;
    public Vector3 shapeSelectedScale;
    public Vector2 offset = new Vector2(0.0f, 700.0f);

    [HideInInspector]
    public ShapeData currentShapedata;

    public int TotalSquareNumber { get; set;}

    private List<GameObject> _currentShape = new List<GameObject>();
    private Vector3 _shapeStartScale;
    private RectTransform _transform;
   // private bool shapeDragable = true;
    private Canvas _canvas;

   
    private Vector3 _startPosition;
    private bool _shapeActicve = true;

    private void OnEnable()
    {
        GameEvent.MoveShapeToStartPostion += MoveShapeToStartPostion;
        GameEvent.SetShapeInactive += SetShapeInactive;
    }

    private void OnDisable()
    {
        GameEvent.MoveShapeToStartPostion -= MoveShapeToStartPostion;
        GameEvent.SetShapeInactive -= SetShapeInactive;
    }


    public void Awake()
    {
        _shapeStartScale = this.GetComponent<RectTransform>().localScale;
        _transform = this.GetComponent<RectTransform>();
        _canvas = this.GetComponentInParent<Canvas>();
       // shapeDragable = true;
        _startPosition = _transform.localPosition;
        _shapeActicve = true;
    }

    public bool IsOnStartPosition()
    {
        return _transform.localPosition == _startPosition;
    }

    public bool IsAnyShapeSquareActive()
    {
        foreach (var square in _currentShape)
        {
            if (square.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    public void DeactivateShape()
    {
        if (_shapeActicve)
        {
            foreach (var square in _currentShape)
            {
                square?.GetComponent<ShapeSquare>().DeactivateShape();//check d != null then all method deactivate    
            }
        }
        _shapeActicve = false;
    }

    private void SetShapeInactive()
    {
        if(IsOnStartPosition() == false && IsAnyShapeSquareActive())
        {
            foreach(var square in _currentShape)
            {
                square.gameObject.SetActive(false);
            }
        }
    }

    public void ActivateShape()
    {
        if (!_shapeActicve)
        {
            foreach (var square in _currentShape)
            {
                square?.GetComponent<ShapeSquare>().ActivateShape();
            }
        }
        _shapeActicve = true;
    }

    public void RequestNewShape(ShapeData shapeData)
    {
        _transform.localPosition = _startPosition;
        CreatShape(shapeData);
    }

    public void CreatShape(ShapeData shapeData)
    {
        currentShapedata = shapeData;

        TotalSquareNumber = GetNumberOfSquares(shapeData);
        while (_currentShape.Count <= TotalSquareNumber)
        {
            _currentShape.Add(Instantiate(squareShapeImage, transform) as GameObject);
        }
        foreach (var square in _currentShape)
        {
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false);
        }
        var squareRect = squareShapeImage.GetComponent<RectTransform>();
        var moveDistance = new Vector2(squareRect.rect.width * squareRect.localScale.x, squareRect.rect.height * squareRect.localScale.y);

        int currentIndexInlist = 0;

        for (var row = 0; row < shapeData.rows; row++)
        {
            for (var column = 0; column < shapeData.columns; column++)
            {
                if (shapeData.board[row].column[column])
                {
                    _currentShape[currentIndexInlist].SetActive(true);
                    _currentShape[currentIndexInlist].GetComponent<RectTransform>().localPosition = new Vector2(GetXPositionForShapeSquare(shapeData, column, moveDistance), GetYPositionForShapeSquare(shapeData, row, moveDistance));

                    currentIndexInlist++;
                }
            }
        }
    }

    private float GetXPositionForShapeSquare(ShapeData shapeData, int column, Vector2 moveDistance)
    {
        float shiftOnX = 0f;
        if (shapeData.columns > 1)
        {
            float startXPos;
            if (shapeData.columns % 2 != 0)
                startXPos = (shapeData.columns / 2) * moveDistance.x * -1;
            else
                startXPos = ((shapeData.columns / 2) - 1) * moveDistance.x * -1 - moveDistance.x / 2;
            shiftOnX = startXPos + column * moveDistance.x;

        }
        return shiftOnX;
    }

    private float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
    {
        float shiftOnY = 0f;
        if (shapeData.rows > 1)
        {
            float startYPos;
            if (shapeData.rows % 2 != 0)
                startYPos = (shapeData.rows / 2) * moveDistance.y;
            else
                startYPos = ((shapeData.rows / 2) - 1) * moveDistance.y + moveDistance.y / 2;
            shiftOnY = startYPos - row * moveDistance.y;
        }
        return shiftOnY;
    }


    private int GetNumberOfSquares(ShapeData shapeData)
    {
        int number = 0;
        foreach (var rowData in shapeData.board)
        {
            foreach (var active in rowData.column)
            {
                if (active)
                    number++;
            }
        }

        return number;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = shapeSelectedScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _transform.anchorMin = new Vector2(0, 0);
        _transform.anchorMax = new Vector2(0, 0);
        _transform.pivot = new Vector2(0, 0);
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position, Camera.main, out pos);
        _transform.localPosition = pos + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = _shapeStartScale;
        GameEvent.CheckIfShapeCanBePlaced();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    private void MoveShapeToStartPostion()
    {
        _transform.transform.localPosition = _startPosition;
    }

}
