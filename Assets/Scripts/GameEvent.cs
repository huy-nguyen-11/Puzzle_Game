using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public static Action CheckIfShapeCanBePlaced;

    public static Action MoveShapeToStartPostion;

    public static Action RequestNewShapes;

    public static Action SetShapeInactive;
}
