using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCharController: MonoBehaviour
{
    #region Serialised

    [SerializeField] private float f_maxSpeed;
    [SerializeField] private float f_JumpForce;
    [SerializeField] private float f_mouseSensitivity;
    [SerializeField] private float groundCheckDistance;

    [SerializeField] private Camera cam;

    #endregion

    #region Privates

    private bool b_jump;
    private bool b_isGrounded;

    private Vector3 v_moveDir;
    private Vector3 v_mouseMove;
    private RaycastHit hit;

    private Rigidbody rb;
    private Player player;

    #endregion

    private void Start()
    {

        rb = GetComponent<Rigidbody>();

        player = ReInput.players.GetPlayer(0);

    }

    private void Update()
    {

        GroundCheck();
        GetInputs();
        AdjustMoveDirection();

    }

    private void FixedUpdate()
    {

       Move();

    }

    private void GetInputs()
    {


        b_jump = player.GetButtonDown("Jump");

        v_moveDir.x = player.GetAxis("LeftStickX");
        v_moveDir.z = player.GetAxis("LeftStickY");

        v_mouseMove.x = -player.GetAxis("RightStickY");
        v_mouseMove.y = player.GetAxis("RightStickX");

    }

    private void AdjustMoveDirection()
    {

        v_moveDir *= f_maxSpeed;
        v_moveDir *= Time.deltaTime;

        v_moveDir = cam.transform.TransformDirection(v_moveDir);

        v_moveDir = (hit.normal == Vector3.forward ? Vector3.ProjectOnPlane(v_moveDir, Vector3.up) : Vector3.ProjectOnPlane(v_moveDir, hit.normal));


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
        rb.velocity += Vector3.up * y;

        Debug.Log(y);

    }

}
