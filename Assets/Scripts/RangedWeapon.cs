using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    [Header("Settings")]
    [SerializeField]
    private GameObject projectileGameObject;

    public override void Shoot(Vector2 aimDirection)
    {
        base.Shoot(aimDirection);
        Projectile projectile = Instantiate(projectileGameObject, transform.position, transform.rotation).GetComponent<Projectile>();

        projectile.Init(aimDirection);
    }
}
