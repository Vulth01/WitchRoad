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
    private bool isGrounded = true;
    private float jumpHeight = 5;
    private float moveSpeed = 20;

    private void Awake()
    {
        playerObj = this.gameObject;
        playerRb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        move = input.actions["move"];
        jump = input.actions["jump"];
    }

    private void FixedUpdate()
    {
        DoMovement();
        DoJump();
    }

    private void DoMovement()
    {
        Vector2 moveVector = move.ReadValue<Vector2>();
        Vector3 moveVector3 = new Vector3(moveVector.x, 0, moveVector.y);
        Vector3 pos = transform.position;
        pos.x += moveVector3.x * moveSpeed * Time.deltaTime;
        pos.z += moveVector3.z * moveSpeed * Time.deltaTime;
        playerRb.position = pos;
    }

    private void DoJump()
    {
        if (jump.IsPressed() && isGrounded){
            playerRb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Floor")  
            isGrounded = true;

    }


}
