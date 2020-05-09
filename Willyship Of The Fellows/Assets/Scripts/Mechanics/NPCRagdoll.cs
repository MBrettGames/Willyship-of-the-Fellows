using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRagdoll : MonoBehaviour
{

    private bool b_isRagdolling;
    private Rigidbody rb;
    Animator anim;

    public List<Collider> RagdollParts = new List<Collider>();



    public void Start()
    {
        rb = GetComponent<Rigidbody>();

        anim = GetComponentInChildren<Animator>();
    }

    private void Awake()
    {
        SetRagdollParts();
        SetPlayerCollider();
       // TurnOffRagdoll();
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
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        anim.enabled = false;
        b_isRagdolling = true;
        Debug.Log("I'm ragdolling");

        foreach (Collider col in RagdollParts)
        {
            col.isTrigger = false;
            col.attachedRigidbody.velocity = Vector3.zero;
        }
    }

    public void TurnOffRagdoll()
    {
        rb.useGravity = true;
        this.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        anim.enabled = true;
        b_isRagdolling = false;
        Debug.Log("I'm not ragdolling");

        foreach (Collider col in RagdollParts)
        {
            col.isTrigger = true;
        }
    }

}
