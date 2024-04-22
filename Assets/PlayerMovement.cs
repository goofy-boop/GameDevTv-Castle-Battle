using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 5f;

    public float jumpForce = 3f;

    public float gravity = -9.81f;

    public Transform gCheck;
    public float gRadius = 0.4f;
    public LayerMask gLayer;

    public Transform cam;

    public Animator animator;

    [Range(0f, 1f)]public float turnSpeed;

    bool isGrounded;

    Vector3 moveDir;

    Vector3 velocity;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(gCheck.position, gRadius, gLayer);

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        float z = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");

        bool isMoving = x != 0f || z != 0f;

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        Vector3 camRelativeZ = z * camForward;
        Vector3 camRelativeX = x * camRight;
        camRelativeZ.y = 0f;
        camRelativeX.y = 0f;
        camRelativeZ = camRelativeZ.normalized;
        camRelativeX = camRelativeX.normalized;

        moveDir = camRelativeZ + camRelativeX;

        controller.Move(moveDir * speed * Time.deltaTime);

        if (isMoving)
        {
            Quaternion currentRotation = transform.rotation;
            Quaternion desiredRotatation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(currentRotation, desiredRotatation, turnSpeed);
        }

        if (Input.GetButton("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}