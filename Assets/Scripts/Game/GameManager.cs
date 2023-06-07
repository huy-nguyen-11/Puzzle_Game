using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private Text  pointText;
    [SerializeField] private Text  gameOver_PointText;
    [SerializeField] private GameObject gameOverPanel;

    private int _point = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        gameOver_PointText.text = $"{_point}";
       
    }

    public void GainPoint()
    {
        _point += 1;
        pointText.text = $"{_point}";
    }

   
}
