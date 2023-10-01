using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance { get; private set; }

    public Vector2 move { get; private set; }
    public Vector2 aim { get; private set; }

    public bool attack { get; private set; }

    private PlayerInputActions inputActions;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;

        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += playerInputActions => move = playerInputActions.ReadValue<Vector2>();
        inputActions.Player.Aim.performed += playerInputActions => aim = playerInputActions.ReadValue<Vector2>();

        inputActions.Player.Attack.canceled += PlayerInputActions => attack = true;

        inputActions.Enable();
    }

    private void LateUpdate()
    {
        attack = false;
    }
}
