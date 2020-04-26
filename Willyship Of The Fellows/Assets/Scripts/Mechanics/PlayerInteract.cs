using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;


public class PlayerInteract : MonoBehaviour
{
    private GameObject raycastedObj;
    private Player player;

    [SerializeField] private int rayLength = 10;
    [SerializeField] private LayerMask layerMaskInteract;

    [SerializeField] private GameObject uiCrosshair;


    private void Update()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, out hit, rayLength, layerMaskInteract.value))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                raycastedObj = hit.collider.gameObject;
                CrosshairActive();

                if (Input.GetButtonDown("Interact"))
                {
                    Debug.Log("I have interacted");
                    raycastedObj.SetActive(false);
                }
            }
        }
        else
        {
            CrosshairInactive();
        }
    }

    void CrosshairActive()
    {
        uiCrosshair.SetActive (true);
    }

    void CrosshairInactive()
    {
        uiCrosshair.SetActive(false);
    }

}
