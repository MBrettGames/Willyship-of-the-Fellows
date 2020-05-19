using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableNPCs : MonoBehaviour
{
    [SerializeField] GameObject NPC_Parent;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NPC_Parent.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NPC_Parent.SetActive(false);
        }
    }
}
