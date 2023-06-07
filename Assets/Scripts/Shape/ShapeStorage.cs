using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeStorage : MonoBehaviour
{
    public List<ShapeData> shapeData;
    public List<Shape> shapeList;

    private void OnEnable()
    {
        GameEvent.RequestNewShapes += RequestNewShapes;
    }

    private void OnDisable()
    {
        GameEvent.RequestNewShapes -= RequestNewShapes;
    }


    void Start()
    {
        foreach (var shape in shapeList)
        {
            var shapeIndex = UnityEngine.Random.Range(0, shapeData.Count);
            shape.CreatShape(shapeData[shapeIndex]);
        }
    }


    public Shape GetCurentSelectedShape()
    {
        foreach (var shape in shapeList)
        {
            if (shape.IsOnStartPosition() == false && shape.IsAnyShapeSquareActive())
            {
                return shape;
            }
        }

        Debug.LogError("khong co shape duoc chon!");
        return null;
    }

    private void RequestNewShapes()
    {
        foreach(var shape in shapeList)
        {
            var shapeIndex = UnityEngine.Random.Range(0, shapeData.Count);
            shape.RequestNewShape(shapeData[shapeIndex]);
        }
    }

}
