using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 재시작을 위해 필요
using UnityEngine.EventSystems; // EventSystem을 제어하기 위해 필요

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI; // Pause Menu UI
    private bool isPaused = false;

    private void Start()
    {
        // PauseMenu UI는 초기에는 비활성화
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
    }

    private void Update()
    {
        // ESC 키로 Pause Menu 활성화/비활성화
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

public void PauseGame()
{
    if (pauseMenuUI != null)
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; 
        isPaused = true;

        //Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 모든 레이어 비활성화
        Physics2D.autoSyncTransforms = false;
    }
}


    public void ResumeGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f; // 게임 재개
            isPaused = false;

            // EventSystem의 입력 활성화
            if (EventSystem.current != null)
            {
                EventSystem.current.enabled = true;
            }

            // 마우스와 키보드 입력 복구
            //Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // 시간 초기화
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 다시 로드
    }

    public void ExitToHome()
    {
        Time.timeScale = 1f; // 시간 초기화
        SceneManager.LoadScene("01_FINAL_Title"); // Home Scene 이름을 실제 홈 씬 이름으로 변경
    }
}
