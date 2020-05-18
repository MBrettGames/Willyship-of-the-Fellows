using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRagdoll : MonoBehaviour
{

    public bool b_isRagdolling;
    private Rigidbody rb;
    Animator anim;
    [SerializeField] GameObject DialogueCols;

    public List<Collider> RagdollParts = new List<Collider>();



    public void Start()
    {
        rb = GetComponent<Rigidbody>();

        anim = GetComponentInChildren<Animator>();
        b_isRagdolling = false;
        RagdollCtrl();
    }

    private void Awake()
    {
        SetRagdollParts();
        SetPlayerCollider();

    }

    private void Update()
    { 
    }

    public void RagdollCtrl()
    {
        if (b_isRagdolling == true)
        {
            TurnOnRagdoll();
        }

        else
        {
            TurnOffRagdoll();
        }
    }

    private void SetRagdollParts()
    {
        Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("RagdollPart") && col.gameObject != this.gameObject)
            {
                col.isTrigger = true;
                RagdollParts.Add(col);
            }
        }
    }

    private void SetPlayerCollider()
    {
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        Debug.Log("I'm a player collider");

    }

    public void TurnOnRagdoll()
    {
        DialogueCols.SetActive(false);
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        anim.enabled = false;
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Debug.Log("I'm ragdolling");

        foreach (Collider col in RagdollParts)
        {
            col.isTrigger = false;
            col.attachedRigidbody.velocity = Vector3.zero;
        }
    }

    public void TurnOffRagdoll()
    {
        DialogueCols.SetActive(true);
        rb.useGravity = true;
        anim.enabled = true;
        this.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        Debug.Log("I'm not ragdolling");

        foreach (Collider col in RagdollParts)
        {
            col.isTrigger = true;
        }
    }

}
