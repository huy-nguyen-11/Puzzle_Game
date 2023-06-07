using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static int Point;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private Text high_PointText;

    private void Start()
    {
        Debug.Log(Point);
        //int hiPoint = Point;
        //high_PointText.text = $"{hiPoint}";

        //if (hiPoint > )
        //{
        //    hiPoint = 11;
        //    high_PointText.text = $"{hiPoint}";
        //}

        //int _point = Point;
        //int hiPoint = PlayerPrefs.GetInt("high-point", 0);
        //if (_point > hiPoint)
        //{
        //    PlayerPrefs.SetInt("high-point", _point);
        //    high_PointText.text = $"{hiPoint}";
        //}
    }

    public void BackHome()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Retry()
    {
        SceneManager.LoadScene("Game");
    }

    public void ShowPauseUI()
    {
        pauseUI.SetActive(true);
    }

    public void CloseUI()
    {
        pauseUI.SetActive(false);
    }
}
