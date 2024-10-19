using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private GameObject playerObj;
    private Rigidbody playerRb;
    private PlayerInput input;
    private InputAction move;
    private InputAction jump;
    private bool isGrounded;
    private float jumpHeight = 15;

    private void Awake()
    {
        playerObj = this.gameObject;
        input = GetComponent<PlayerInput>();
        move = input.actions["move"];
        jump = input.actions["jump"];
    }

    private void Update()
    {
        DoJump();
    }

    private void DoJump()
    {
        Vector2 moveVector = move.ReadValue<Vector2>();
        if (jump.IsPressed() && isGrounded)
        {
            playerRb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }



}
