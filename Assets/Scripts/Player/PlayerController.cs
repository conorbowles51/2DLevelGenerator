using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 4f;
    [SerializeField]
    private float accelaration = 20f;
    [SerializeField]
    private float decelaration = 20f;

    private InputHandler input;
    private Rigidbody2D rbody;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        input = InputHandler.instance;
    }

    public void HandleMovement()
    {
        Vector2 targetVelocity = input.move * moveSpeed;

        Vector2 velocityDifference = targetVelocity - rbody.velocity;
        float accelarationRate = (Mathf.Abs(targetVelocity.x + targetVelocity.y) > 0.1f) ? accelaration : decelaration;

        float xVelocity = velocityDifference.x * accelarationRate;
        float yVelocity = velocityDifference.y * accelarationRate;

        rbody.AddForce(new Vector2(xVelocity, yVelocity));
    }

    public Vector2 GetVelocity()
    {
        return rbody.velocity;
    }
}
