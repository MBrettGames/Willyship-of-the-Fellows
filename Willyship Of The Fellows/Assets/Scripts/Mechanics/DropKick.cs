using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropKick : MonoBehaviour
{
    private NPCRagdoll npcRagdoll;
    private Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        npcRagdoll = other.GetComponent<NPCRagdoll>();
        anim = other.GetComponentInChildren<Animator>();
        if (other.CompareTag("HurtableNPC"))
        {
            npcRagdoll.b_isRagdolling = true;
            npcRagdoll.RagdollCtrl(transform.forward * 1000);
        }

        if (other.CompareTag("GuardNPC"))
        {
            anim.Play("Stagger");
        }
    }
}
