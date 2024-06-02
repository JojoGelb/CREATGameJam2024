using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Rigidbody rb;

    private Vector3 moveDir;

    public float jumpForce;

    public float jumpCD = 0;
    private float jumpTimer = 10000;

    public float rotationSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InputManager.Instance.RegisterToJumpEvent(OnJumpPressed);
    }

    private void OnDisable()
    {
        InputManager.Instance?.UnRegisterToJumpEvent(OnJumpPressed);
    }

    private void OnJumpPressed(InputAction.CallbackContext context)
    {
        if (jumpTimer < jumpCD) return;

        jumpTimer = 0;
        rb.AddForce(transform.up * jumpForce);
    }

    private void Update()
    {
        Vector2 moveInput = InputManager.Instance.GetMovementVectorNormalized();
        moveDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        jumpTimer += Time.deltaTime;
    }

    void FixedUpdate()
    {
        if(moveDir != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float initialAngle = transform.GetChild(0).localRotation.eulerAngles.y;

            float newRotation = Mathf.MoveTowardsAngle(initialAngle, angle, rotationSpeed*Time.fixedDeltaTime);

            transform.GetChild(0).localRotation = Quaternion.Euler(0, newRotation, 0);
        }
        rb.MovePosition(rb.position + transform.TransformDirection(moveDir) * moveSpeed * Time.fixedDeltaTime);
    }
}
