using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;

    [SerializeField] private GameObject DialogueBox;

    public Text nameText;
    public Text dialogueText;

    public Animator animator;
    private GameObject questBox;
    private ThirdPersonCharController movementScript;
    private PlayerInteract interactScript;

    public static DialogueManager dialogueStatic;

    void Start()
    {
        sentences = new Queue<string>();
        dialogueStatic = this;
        DialogueBox.SetActive(false);
        animator.SetBool("IsOpen", true);
    }

    public void StartDialogue (Dialogue dialogue, GameObject questGiver, ThirdPersonCharController otherMovementScript, PlayerInteract localInteractScript)
    {
        DialogueBox.SetActive(true);
        movementScript = otherMovementScript;
        interactScript = localInteractScript;
        questBox = questGiver;
        animator.SetBool("IsOpen", true);
        nameText.text = (dialogue.name);

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence ()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        questBox.SetActive(false);
        movementScript.enabled = true;
        interactScript.isInteracting = false;

    }
}
