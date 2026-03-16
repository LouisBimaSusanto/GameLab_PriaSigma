using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CarJump : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpScale = 1.3f;
    public float jumpDuration = 1f;

    private Vector3 originalScale;
    private Collider2D carCollider;
    private bool isJumping = false;

    void Awake()
    {
        originalScale = transform.localScale;
        carCollider = GetComponent<Collider2D>();
    }

    public void Jump()
    {
        if (isJumping) return;

        isJumping = true;

        carCollider.enabled = false;

        Sequence jumpSequence = DOTween.Sequence();

        jumpSequence.Append(
            transform.DOScale(originalScale * jumpScale, jumpDuration / 2)
        );

        jumpSequence.Append(
            transform.DOScale(originalScale, jumpDuration / 2)
        );

        jumpSequence.OnComplete(() =>
        {
            carCollider.enabled = true;
            isJumping = false;
        });
    }
}
