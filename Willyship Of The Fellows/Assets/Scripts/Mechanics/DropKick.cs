﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropKick : MonoBehaviour
{
    private NPCRagdoll npcRagdoll;

    private void OnTriggerEnter(Collider other)
    {
        npcRagdoll = other.GetComponent<NPCRagdoll>();
        if (other.CompareTag("HurtableNPC"))
        {
            npcRagdoll.b_isRagdolling = true;
            npcRagdoll.RagdollCtrl();
            Debug.Log("I kicked him");
        }
    }
}
