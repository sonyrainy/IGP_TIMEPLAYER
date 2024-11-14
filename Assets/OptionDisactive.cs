using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionDisactive : MonoBehaviour
{
    [SerializeField] GameObject settingWindow;

    public void ExitButton()
    {
        settingWindow.SetActive(false);
    }
}
