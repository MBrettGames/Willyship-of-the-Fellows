﻿using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCharController : MonoBehaviour
{
    #region Serialised

    [SerializeField] private float f_maxSpeed;
    [SerializeField] private float f_JumpForce;
    [SerializeField] private float f_mouseSensitivity;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float runModifier;
    [SerializeField] private float camDistance;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform camTarget;

    #endregion

    #region Privates

    private bool b_jump;
    private bool b_isGrounded;
    private bool b_run;

    private Vector3 v_moveDir;
    private Vector3 v_mouseMove;
    private RaycastHit hit;

    private Rigidbody rb;
    private Player player;

    Animator anim;
    private float currentSpeed;

    #endregion

    private void Start()
    {

        rb = GetComponent<Rigidbody>();

        anim = GetComponentInChildren<Animator>();

        player = ReInput.players.GetPlayer(0);

    }

    private void Update()
    {
        anim.SetFloat("speed", currentSpeed);
        anim.SetBool("run", b_run);

        GroundCheck();
        GetInputs();
        AdjustMoveDirection();

    }

    private void FixedUpdate()
    {

        Move();
        Jump();

    }

    private void LateUpdate()
    {

        cam.transform.LookAt(camTarget);

        cam.transform.Translate(v_mouseMove * Time.deltaTime * f_mouseSensitivity);

        cam.transform.position = camTarget.position + (cam.transform.position - camTarget.position).normalized * camDistance;

    }

    private void GetInputs()
    {


        b_jump = player.GetButtonDown("Jump");
        b_run = player.GetButton("Run");

        v_moveDir.x = player.GetAxis("LeftStickX");
        v_moveDir.z = player.GetAxis("LeftStickY");

        v_mouseMove.y = player.GetAxis("RightStickY");
        v_mouseMove.x = player.GetAxis("RightStickX");

    }

    private void AdjustMoveDirection()
    {

        v_moveDir *= f_maxSpeed * (b_run ? runModifier : 1);
        v_moveDir *= Time.deltaTime;

        v_moveDir = cam.transform.TransformDirection(v_moveDir);

        v_moveDir = (!b_isGrounded ? Vector3.ProjectOnPlane(v_moveDir, Vector3.up) : Vector3.ProjectOnPlane(v_moveDir, hit.normal));


    }

    private void GroundCheck()
    {
        hit.normal = Vector3.forward;
        b_isGrounded = Physics.Raycast(transform.position + transform.up * 0.1f, Vector3.down, out hit, groundCheckDistance);
    }

    private void Move()
    {
        rb.AddForce(v_moveDir, ForceMode.Impulse);

        float y = rb.velocity.y;

        rb.velocity *= 0.9f;

        rb.velocity = Vector3.Scale(rb.velocity, (Vector3.one - Vector3.up));
        currentSpeed = rb.velocity.magnitude;
        rb.velocity += Vector3.up * y;

        Debug.Log(y);

    }

    private void Jump()
    {
        if (b_jump)
        {
            rb.AddForce(Vector3.up * f_JumpForce * (b_isGrounded ? 1 : 0), ForceMode.Impulse);
        }
    }

}
