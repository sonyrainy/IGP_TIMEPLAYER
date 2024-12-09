using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTreeAttackObject : TimeZoneObject
{
    [SerializeField] GameObject[] colliders;
    [SerializeField] GameObject copyObject;
    [SerializeField] ForestBoss forestBoss;
    [SerializeField] float fadeDuration = 2.0f;
    [SerializeField] bool isAttack = false;
    public bool isInTimeZone = false;
    public bool isTreeDestroied = false;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        forestBoss = FindObjectOfType<ForestBoss>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInTimeZone == true)
        {
            animator.speed = 1f;
        }
        else
        {
            animator.speed = 0.01f;
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Grow") && stateInfo.normalizedTime >= 1.0f && !isAttack)
        {
            isAttack = !isAttack;
            forestBoss.OnDamaged();
            StartCoroutine(fadeDestroy());
        }
    }

    public void ChangeCollider(int index)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].SetActive(false);
        }

        colliders[index].SetActive(true);
    }

    IEnumerator fadeDestroy() 
    {
        if (spriteRenderer == null)
        {
            yield break;
        }

        Color originalColor = spriteRenderer.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime/fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
        isTreeDestroied = true;
        spriteRenderer.color = originalColor;
    }
}
