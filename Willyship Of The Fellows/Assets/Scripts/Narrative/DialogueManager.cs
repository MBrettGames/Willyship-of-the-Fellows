using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    private Queue<string> CharNames;


    [SerializeField] private GameObject DialogueBox;
    [SerializeField] QuestProgress Q_Prog;

    public Text nameText;
    public Text dialogueText;

    public Animator animator;
    private GameObject InteractCol;
    private ThirdPersonCharController movementScript;
    private PlayerInteract interactScript;

    public static DialogueManager dialogueStatic;

    void Start()
    {
        sentences = new Queue<string>();
        CharNames = new Queue<string>();
        dialogueStatic = this;
        DialogueBox.SetActive(false);
        animator.SetBool("IsOpen", true);
    }

    public void StartDialogue (Dialogue dialogue, GameObject questGiver, ThirdPersonCharController otherMovementScript, PlayerInteract localInteractScript)
    {
        DialogueBox.SetActive(true);
        movementScript = otherMovementScript;
        interactScript = localInteractScript;
        InteractCol = questGiver;
        animator.SetBool("IsOpen", true);

        sentences.Clear();
        CharNames.Clear();


        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        foreach (string names in dialogue.names)
        {
            CharNames.Enqueue(names);
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
        string name = CharNames.Dequeue();
        string sentence = sentences.Dequeue();


        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence, name));
    }

    IEnumerator TypeSentence (string sentence, string CharName)
    {
        nameText.text = "";
        nameText.text = CharName;

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
        if(InteractCol.CompareTag ("NPCQuestObj"))
        {
        InteractCol.SetActive(false);
            Q_Prog.QuestFinished();
        }
        else
        {
            InteractCol.SetActive(true);
        }
        movementScript.enabled = true;
        interactScript.isInteracting = false;

    }
}
