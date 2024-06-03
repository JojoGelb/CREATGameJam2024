using _Script;
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

    public float pollutionSlowDown = 2f;
    private bool slowed = false;

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

        int r = (int)(GetColorAtPosition().r * 10);
        Debug.Log("R: " + r);
        if (r == 3)
        {
            if (!slowed)
            {
                moveSpeed -= pollutionSlowDown;
                slowed = true;
            }
        }
        else
        {
            if (slowed)
            {
                moveSpeed += pollutionSlowDown;
                slowed = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (moveDir != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float initialAngle = transform.GetChild(0).localRotation.eulerAngles.y;

            float newRotation = Mathf.MoveTowardsAngle(initialAngle, angle, rotationSpeed*Time.fixedDeltaTime);

            transform.GetChild(0).localRotation = Quaternion.Euler(0, newRotation, 0);
        }
        rb.MovePosition(rb.position + transform.TransformDirection(moveDir) * moveSpeed * Time.fixedDeltaTime);
    }

    public LayerMask layer;
    public Color GetColorAtPosition()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position + transform.up * 10,-transform.up, out hit,Mathf.Infinity ,1<<7 ))
        {
            Debug.Log("Not found");
            return Color.white;
        }
            

        if(!hit.transform.TryGetComponent(out PollutionManager po))
        {
            Debug.Log("Wrong collided: " + hit.transform.name);
            return Color.white;
        } else
        {
            Debug.Log("Hit right dude");
            return po.GetColorAtPosition(hit);
        }
    }

}
