using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyKeyExitScene : MonoBehaviour
{
        private bool canExit = false; 

        void Start()
    {
        StartCoroutine(EnableExitAfterDelay(5f));
    }
    void Update()
    {
        if (canExit && Input.anyKeyDown)
        {
            ExitGame();
        }
    }

    private IEnumerator EnableExitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canExit = true;
    }

    private void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
