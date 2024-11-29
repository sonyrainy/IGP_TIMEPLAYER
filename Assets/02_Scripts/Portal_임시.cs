using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리 기능 사용을 위한 네임스페이스

public class Portal : MonoBehaviour
{
    public string nextSceneName; // 이동할 다음 씬의 이름
    [SerializeField] private float delay = 5f;  // 포탈 활성화 지연 시간

    private Collider2D portalCollider;
    private SpriteRenderer portalRenderer;

    void Start()
    {
        // 오브젝트의 Collider와 SpriteRenderer를 비활성화하여 포탈 기능을 중지
        portalCollider = GetComponent<Collider2D>();
        portalRenderer = GetComponent<SpriteRenderer>();

        if (portalCollider != null)
            portalCollider.enabled = false;

        if (portalRenderer != null)
            portalRenderer.enabled = false;

        // 일정 시간 뒤 활성화
        StartCoroutine(ActivatePortalAfterDelay());
    }

    // 포탈에 플레이어가 닿았을 때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 플레이어가 포탈에 닿았는지 확인
        {
            // 다음 씬으로 이동
            LoadNextScene();
        }
    }

    // 다음 씬을 로드하는 함수
    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName)) // 이동할 씬 이름이 비어있지 않은 경우
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("다음 씬의 이름이 설정되지 않았습니다.");
        }
    }

    private IEnumerator ActivatePortalAfterDelay()
    {
        yield return new WaitForSeconds(delay);  // 지연 시간만큼 대기

        // 포탈 기능을 활성화
        if (portalCollider != null)
            portalCollider.enabled = true;

        if (portalRenderer != null)
            portalRenderer.enabled = true;
    }
}
