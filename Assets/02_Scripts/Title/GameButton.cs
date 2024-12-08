using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButton : MonoBehaviour
{
    [SerializeField] GameObject settingWindow;

    public void StartGame()
    {
        //SceneManager.LoadScene("IGP_CameraMove, Patrol, Die, Grow");
        SceneManager.LoadScene("IGP_Forest_Normal_forShowcase");

       // SceneManager.LoadScene("IGP_Forest");
    }

    public void SettingActiveButton()
    {
        settingWindow.SetActive(true);
    }
}
