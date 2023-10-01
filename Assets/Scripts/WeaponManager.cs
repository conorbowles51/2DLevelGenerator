using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private Weapon currentWeapon;

    private InputHandler input;

    private Vector2 aimDirection;

    private void Start()
    {
        input = InputHandler.instance;
    }

    private void Update()
    {
        HandleAiming();

        if(input.attack)
        {
            currentWeapon.Shoot(aimDirection);
        }
    }

    private void HandleAiming()
    {
        if(input.aim != Vector2.zero)
        {
            aimDirection = input.aim;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, input.aim);
        }
    }

    public int GetAimDirection()
    {
        return aimDirection.x < 0 ? -1 : 1;
    }
}
