using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingMenu;

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void ShowSetting()
    {
        settingMenu.SetActive(true);
    }

    public void CloseSetting()
    {
        settingMenu.SetActive(false);
    }
}
