using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;


public class PlayerInteract : MonoBehaviour
{
    [SerializeField] QuestProgress Q_Prog;
    private GameObject raycastedObj;
    private Player player;
    private QuestElement questElement;
    public bool isInteracting;
    private bool isSpeaking;

    public Dialogue dialogue;


    [SerializeField] private int rayLength = 10;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private Animator anim;

    [SerializeField] private GameObject uiCrosshairTalk;
    [SerializeField] private GameObject uiCrosshairUse;

    [SerializeField] private ThirdPersonCharController movementScript;

    private void Start()
    {

        player = ReInput.players.GetPlayer(0);
    }

    private void Update()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, out hit, rayLength, layerMaskInteract.value))
        {
            if (hit.collider.CompareTag("NPC") || hit.collider.CompareTag("NPCQuestObj"))
            {
                raycastedObj = hit.collider.gameObject;
                CrosshairTalkActive();
                isSpeaking = true;

                if (player.GetButtonDown("Interact"))
                {
                    if (isInteracting)
                    {
                        DialogueManager.dialogueStatic.DisplayNextSentence();
                    }

                    else
                    {
                        CrosshairInactive();
                        TriggerDialogue(raycastedObj);
                    }
                }
            }
        }
        else
        {
            CrosshairInactive();
            isSpeaking = false;
        }

        if (Physics.Raycast(transform.position, fwd, out hit, rayLength, layerMaskInteract.value))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                raycastedObj = hit.collider.gameObject;
                CrosshairUseActive();

                if (player.GetButtonDown("Interact"))
                {
                    anim.Play("Interact");

                }

            }
            else if (hit.collider.CompareTag("QuestInteractable"))
            {
                raycastedObj = hit.collider.gameObject;
                CrosshairUseActive();

                if (player.GetButtonDown("Interact"))
                {
                    anim.Play("Interact");
                    Q_Prog.QuestFinished();
                }

            }
            else
            {
                CrosshairUseInactive();
            }

        }
        else
        {
            CrosshairUseInactive();
        }
    }

    void CrosshairTalkActive()
    {
        if (!isSpeaking)
        {
            uiCrosshairTalk.SetActive(true);
        }
    }

    void CrosshairUseActive()
    {
        uiCrosshairUse.SetActive(true);
    }

    void CrosshairInactive()
    {
        uiCrosshairTalk.SetActive(false);
    }


    void CrosshairUseInactive()
    {
        uiCrosshairUse.SetActive(false);
    }


    public void TriggerDialogue(GameObject NPC)
    {
        questElement = NPC.GetComponent<QuestElement>();
        DialogueManager.dialogueStatic.StartDialogue(questElement.dialogue, NPC, movementScript, this);
        isInteracting = true;
        movementScript.enabled = false;
        movementScript.StopMovement();
        anim.Play("Greet");

    }
}


