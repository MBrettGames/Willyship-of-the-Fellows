using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRagdoll : MonoBehaviour
{

    public bool b_isRagdolling;
    private Rigidbody rb;
    Animator anim;
    [SerializeField] GameObject DialogueCols;
    [SerializeField] Rigidbody ChestBone;
    public List<Collider> RagdollParts = new List<Collider>();
    private Animator Anim;
    private Vector3 ChestBonePos;


    public void Start()
    {
        rb = GetComponent<Rigidbody>();

        foreach (Collider col in RagdollParts)
        {
            col.attachedRigidbody.isKinematic = true;
        }

        ChestBonePos = ChestBone.transform.localPosition ;

        anim = GetComponentInChildren<Animator>();
        b_isRagdolling = false;
        RagdollCtrl(Vector3.zero);

    }

    private void Awake()
    {
        SetRagdollParts();
        SetPlayerCollider();

    }

    public void RagdollCtrl(Vector3 HitDir)
    {
        if (b_isRagdolling == true)
        {
            TurnOnRagdoll(HitDir);
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

    }

    public void TurnOnRagdoll(Vector3 HitDir)
    {
        DialogueCols.SetActive(false);
        rb.useGravity = false;
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        anim.enabled = false;
        rb.velocity = Vector3.zero;

        foreach (Collider col in RagdollParts)
        {
            col.attachedRigidbody.isKinematic = false;
            col.isTrigger = false;
            col.attachedRigidbody.velocity = Vector3.zero;
            col.attachedRigidbody.useGravity = false;
        }
        StartCoroutine(DelayRagdoll());
        ChestBone.AddForce(HitDir, ForceMode.Impulse);
        StartCoroutine(GetUp());

    }

    public void TurnOffRagdoll()
    {
        transform.position = ChestBone.transform.TransformPoint(ChestBone.transform.localPosition);
        ChestBone.transform.localPosition = ChestBonePos;
        DialogueCols.SetActive(true);
        this.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        anim.enabled = true;
        rb.useGravity = true;

        foreach (Collider col in RagdollParts)
        {
            col.attachedRigidbody.isKinematic = true;
            col.isTrigger = true;
        }

    }

    IEnumerator DelayRagdoll()
    {
        yield return new WaitForEndOfFrame();
        foreach (Collider col in RagdollParts)
        {
            col.attachedRigidbody.velocity = Vector3.zero;
            col.attachedRigidbody.useGravity = true;
        }

    }

    IEnumerator GetUp()
    {
        yield return new WaitForSeconds(5f);
        TurnOffRagdoll();
        anim.Play("GetUp");
    }

}
