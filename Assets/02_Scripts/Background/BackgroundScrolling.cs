using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundScrolling : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject player;

    private Transform backgroundTransform;
    private Rigidbody2D playerRigidbody;

    private float direction;

    // Start is called before the first frame update
    void Start()
    {
        backgroundTransform = GetComponent<Transform>();

        if (player != null)
        {
            playerRigidbody = player.GetComponent<Rigidbody2D>();

            // Rigidbody�� ���� ��� ��� �޽��� ���
            if (playerRigidbody == null)
            {
                Debug.LogWarning("Player GameObject�� Rigidbody�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("Player GameObject�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerRigidbody != null)
        {
            backgroundTransform.Translate(new Vector3(-1 * (playerRigidbody.velocity.x * (speed / 100)), 0, 0));

            if (backgroundTransform.position.x == -9.75f)
            {
                StartCoroutine(ReplicateBackground());
            }
        }
    }

    private IEnumerator ReplicateBackground()
    {
        Instantiate(gameObject, new Vector3(transform.position.x + 27.6f, 0, 0), Quaternion.identity);

        yield return 0;
    }
}

