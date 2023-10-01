using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerAnimator))]
public class PlayerManager : Entity
{
    [Header("References")]
    [SerializeField]
    private WeaponManager weaponManager;

    private PlayerController playerController;
    private PlayerAnimator playerAnimator;

    private void Start() 
    {
        InitalizeEntity();

        playerController = GetComponent<PlayerController>();   
        playerAnimator = GetComponent<PlayerAnimator>(); 
    }

    private void Update()
    {
        playerAnimator.SetAnimationParameters(playerController.GetVelocity().magnitude > 0.1f);
        playerAnimator.HandleSpriteFlip(weaponManager.GetAimDirection());
    }

    private void FixedUpdate()
    {
        playerController.HandleMovement();
    }
}
