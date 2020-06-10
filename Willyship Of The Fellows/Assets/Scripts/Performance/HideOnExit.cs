using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnExit : MonoBehaviour
{
    [SerializeField] GameObject GO_Parent;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GO_Parent.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GO_Parent.SetActive(false);
        }
    }
}
