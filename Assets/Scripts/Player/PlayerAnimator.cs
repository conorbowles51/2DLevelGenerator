using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("General")]
    [SerializeField]
    private bool doesSpriteFaceLeft;

    [Header("References")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Animator animator;

    public void HandleSpriteFlip(int aimDirection)
    {
        if (aimDirection > 0)
        {
            spriteRenderer.flipX = doesSpriteFaceLeft ? false : true;
        }
        else
        {
            spriteRenderer.flipX = doesSpriteFaceLeft ? true : false;
        }
    }

    public void SetAnimationParameters(bool isMoving)
    {
        animator.SetBool("isMoving", isMoving);
    }
}
