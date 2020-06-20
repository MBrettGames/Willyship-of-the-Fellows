using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestProgress : MonoBehaviour
{
    [SerializeField] List<ActualQuestElement> QuestEl = new List<ActualQuestElement>();
    private int CurrentQuestInt;

    private void Start()
    {
        CurrentQuestInt = 0;
        QuestFinished();
    }

    public void QuestFinished()
    {
        foreach (GameObject QuestComponent in QuestEl[CurrentQuestInt].TurnOnAfterFinished)
        {
            QuestComponent.SetActive(true);
        }
        foreach (GameObject QuestComponent in QuestEl[CurrentQuestInt].TurnOffAfterFinished)
        {
            QuestComponent.SetActive(false);
        }

        CurrentQuestInt++;
    }

    [System.Serializable]
    public class ActualQuestElement
    {
        public GameObject[] TurnOffAfterFinished = new GameObject[0];
        public GameObject[] TurnOnAfterFinished = new GameObject[0];
    }
}
