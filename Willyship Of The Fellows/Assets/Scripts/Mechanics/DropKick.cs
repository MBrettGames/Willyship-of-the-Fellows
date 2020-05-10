using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropKick : MonoBehaviour
{
    private NPCRagdoll npcRagdoll;

    private void OnTriggerEnter(Collider other, NPCRagdoll ragdollscript)
    {
        npcRagdoll = other.GetComponent<NPCRagdoll>();
        if (other.CompareTag("HurtableNPC"))
        {
            npcRagdoll.TurnOnRagdoll();
        }
    }
}
