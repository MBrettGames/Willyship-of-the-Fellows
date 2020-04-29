using Cinemachine;
using Rewired;
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
    [SerializeField] private CinemachineFreeLook camLook;
    [SerializeField] private Transform camTarget;

    #endregion

    #region Privates



    private bool b_jump;
    private bool b_isGrounded;
    private bool b_run;
    private bool b_attack;
    private bool b_utility;

    private bool isRagdolling;
    private bool b_self;


    private Vector3 v_moveDir;
    private Vector3 v_mouseMove;
    private RaycastHit hit;

    private Rigidbody rb;
    private Player player;

    Animator anim;
    private float currentSpeed;

    #endregion

    #region Publics

    public List<Collider> RagdollParts = new List<Collider>();

    #endregion

    private void Start()
    {

        rb = GetComponent<Rigidbody>();

        anim = GetComponentInChildren<Animator>();

        player = ReInput.players.GetPlayer(0);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Awake()
    {
        //TurnOffRagdoll();
        SetRagdollParts();
        SetPlayerCollider();
    }

    private void Update()
    {
        anim.SetFloat("speed", currentSpeed);
        anim.SetBool("run", b_run);
        anim.SetBool("grounded", b_isGrounded);

        GroundCheck();
        GetInputs();
        AdjustMoveDirection();

    }

    private void FixedUpdate()
    {

        Attack();
        Move();
        Jump();
        UtilityAction();

    }

    private void LateUpdate()
    {

        camLook.m_XAxis.m_InputAxisValue = v_mouseMove.x;
        camLook.m_YAxis.m_InputAxisValue = v_mouseMove.y;

        //cam.transform.LookAt(camTarget);

        //cam.transform.Translate(v_mouseMove * Time.deltaTime * f_mouseSensitivity);

        //cam.transform.position = camTarget.position + (cam.transform.position - camTarget.position).normalized * camDistance;

    }

    private void GetInputs()
    {


        b_jump = player.GetButtonDown("Jump");
        b_run = player.GetButton("Run");
        b_attack = player.GetButton("Attack");
        b_utility = player.GetButton("Utility");

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

        v_moveDir = Vector3.ProjectOnPlane(v_moveDir, Vector3.up);


    }

    private void GroundCheck()
    {
        b_self = false;
        hit.normal = Vector3.forward;
        b_isGrounded = Physics.Raycast(transform.position + transform.up * 0.1f, Vector3.down, out hit, groundCheckDistance);

        foreach (Collider col in RagdollParts)
        {
            if (col.gameObject == hit.collider.gameObject)
            {
                b_self = true;
                break;
            }
        }
    }

    private void Move()
    {
        rb.AddForce(v_moveDir, ForceMode.Impulse);

        float y = rb.velocity.y;

        rb.velocity *= 0.8f;

        rb.velocity = Vector3.Scale(rb.velocity, (Vector3.one - Vector3.up));

        //speed for animation reasons
        currentSpeed = rb.velocity.magnitude;
        rb.velocity += Vector3.up * y;

        if (v_moveDir.magnitude > 0.3f)
            transform.rotation = Quaternion.LookRotation(v_moveDir);

    }

    public void StopMovement()
    {
        anim.SetFloat("speed", 0f);
        rb.velocity = Vector3.zero;
    }

    private void Jump()
    {
        if (b_jump && b_isGrounded)
        {
            StartCoroutine(JumpDelay());
            anim.Play("Leap");
            rb.AddForce(Vector3.up * f_JumpForce * (b_isGrounded ? 1 : 0), ForceMode.Impulse);
        }
    }

    private void Attack()
    {
        if (b_attack && b_isGrounded)
        {
            anim.Play("Drop_Kick");
            StopMovement();
            StartCoroutine(Dropkickslow());
        }
    }

    private void UtilityAction()
    {
        //TurnOnRagdoll();
    }

    IEnumerator Dropkickslow()
    {
        f_maxSpeed = f_maxSpeed / 1.1f;
        yield return new WaitForSeconds(1);
        f_maxSpeed = f_maxSpeed * 1.1f;
    }

    IEnumerator JumpDelay()
    {
        yield return new WaitForSeconds(1);
    }

    private void SetRagdollParts()
    {
        Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();

        foreach (Collider col in colliders)
        {
            if (col.gameObject != this.gameObject)
            {
                col.enabled = false;
                RagdollParts.Add(col);
            }
        }
    }

    private void SetPlayerCollider()
    {
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
    }

    /* public void TurnOnRagdoll()
     {
         rb.useGravity = false;
         rb.velocity = Vector3.zero;
         this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
         anim.enabled = false;

         foreach(Collider col in RagdollParts)
         {
             col.enabled = false;
             col.attachedRigidbody.velocity = Vector3.zero;
         }
     }

     public void TurnOffRagdoll()
     {
         rb.useGravity = true;
         this.gameObject.GetComponent<CapsuleCollider>().enabled = true;
         anim.enabled = true;

         foreach (Collider col in RagdollParts)
         {
             col.enabled = true;
         }
     } */


}
