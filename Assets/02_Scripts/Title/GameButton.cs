using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButton : MonoBehaviour
{
    [SerializeField] GameObject settingWindow;

    public void StartGame()
    {
        SceneManager.LoadScene("IGP_Forest_Normal_forShowcase");
    }

    public void SettingActiveButton()
    {
        settingWindow.SetActive(true);
    }
}
