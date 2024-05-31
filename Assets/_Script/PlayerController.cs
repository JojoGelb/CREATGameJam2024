using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Rigidbody rb;

    private Vector3 moveDir;

    public float jumpForce;

    public float jumpCD = 0;
    private float jumpTimer = 10000;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        jumpTimer += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Space) && jumpTimer > jumpCD)
        {
            jumpTimer = 0;
            rb.AddForce(transform.up * jumpForce);
        }
    }

    void FixedUpdate()
    {
        if(moveDir != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            transform.GetChild(0).localRotation = Quaternion.Euler(0, angle, 0);
        }
        rb.MovePosition(rb.position + transform.TransformDirection(moveDir) * moveSpeed * Time.fixedDeltaTime);
    }
}
