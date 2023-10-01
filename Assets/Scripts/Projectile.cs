using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rbody;

    public void Init(Vector2 aimDirection)
    {
        rbody = gameObject.AddComponent<Rigidbody2D>();

        rbody.AddForce(aimDirection.normalized * 10, ForceMode2D.Impulse);

        StartCoroutine("DestroyArrow", 2f);
    }

    private void Update()
    {
        float angle = Mathf.Atan2(rbody.velocity.y, rbody.velocity.x) * Mathf.Rad2Deg + 180;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private IEnumerator DestroyArrow()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
