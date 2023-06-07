using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{

    public Image normalImnage;
    public Image hooverImage;
    public Image activeImage;
    public List<Sprite> normalImages;

    public bool Selected { get; set;}
    public int SquareIndex { get; set;}
    public bool squareOccupied { get; set;}

    private void Start()
    {
        Selected = false;
        squareOccupied = false;
    }

    public bool CanWeUseThisSquare()
    {
        return hooverImage.gameObject.activeSelf;
    }

    public void PlaceShapeOnBoard()
    {
        ActivateSquare();
    }


    public void ActivateSquare()
    {
        hooverImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);
        Selected = true;
        squareOccupied = true;
    }

    public void Deactivate()
    {
        activeImage.gameObject.SetActive(false);
    }

    public void ClearOccupied()
    {
        Selected = false;
        squareOccupied = false;
    }
    public void SetImage(bool setFirstImage)
    {
        normalImnage.GetComponent<Image>().sprite = setFirstImage ? normalImages[1] : normalImages[0]; // true thì trả về hình ảnh trong mảng 1 , false thì trả về mảng 0.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (squareOccupied == false)
        {
            Selected = true;
            hooverImage.gameObject.SetActive(true);
        }
        else if(collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Selected = true;
        if (squareOccupied == false)
        {
            hooverImage.gameObject.SetActive(true);
        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (squareOccupied == false)
        {
            Selected = false;
            hooverImage.gameObject.SetActive(false);
        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().UnSetOccupied();
        }
    }
}
