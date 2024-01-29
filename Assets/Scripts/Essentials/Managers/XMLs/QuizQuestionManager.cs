using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizQuestionManager : MonoBehaviour
{
    public static QuizQuestionManager Instance { get; private set; }
    public QuizQStruct Struct;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string qQCode, string qQPhase,
        int qQNumber, string qQDescription, string qQChoice1,
        string qQChoice2, string qQChoice3, string qQChoice4,
        int qQAnswer, string sCode)
    {
        QuizQEntry newEntry = new();
        newEntry.quizQCode = qQCode;
        newEntry.quizQPhase = qQPhase;
        newEntry.quizQNumber = qQNumber;
        newEntry.quizQDescription = qQDescription;
        newEntry.quizQChoice1 = qQChoice1;
        newEntry.quizQChoice2 = qQChoice2;
        newEntry.quizQChoice3 = qQChoice3;
        newEntry.quizQChoice4 = qQChoice4;
        newEntry.quizQAnswer = qQAnswer;
        newEntry.subjectCode = sCode;
        Struct.list.Add(newEntry);
    }

    public void DeleteData(string prevQQCode)
    {
        foreach (QuizQEntry entry in Struct.list)
        {
            if (entry.quizQCode.Equals(prevQQCode))
            {
                Struct.list.Remove(entry);
                break;
            }
        }
    }

    public void ClearList()
    {
        Struct.list.Clear();
    }

    public void UpdateQQCode(string prevQQCode, string newQQCode)
    {
        foreach (QuizQEntry entry in Struct.list)
        {
            if (prevQQCode.Equals(entry.quizQCode))
            {
                entry.quizQCode = newQQCode;
                break;
            }
        }
    }

    public void UpdateQQPhase(string prevQQCode, string newQQPhase)
    {
        foreach (QuizQEntry entry in Struct.list)
        {
            if (prevQQCode.Equals(entry.quizQCode))
            {
                entry.quizQPhase = newQQPhase;
                break;
            }
        }
    }

    public void UpdateQQNumber(string prevQQCode, int newQQNumber)
    {
        foreach (QuizQEntry entry in Struct.list)
        {
            if (prevQQCode.Equals(entry.quizQCode))
            {
                entry.quizQNumber = newQQNumber;
                break;
            }
        }
    }

    public void UpdateQQDesc(string prevQQCode, string newQQDesc)
    {
        foreach (QuizQEntry entry in Struct.list)
        {
            if (prevQQCode.Equals(entry.quizQCode))
            {
                entry.quizQDescription = newQQDesc;
                break;
            }
        }
    }

    public void UpdateQQChoice(string prevQQCode, int choice,
        string newQQChoice)
    {
        foreach (QuizQEntry entry in Struct.list)
        {
            if (prevQQCode.Equals(entry.quizQCode))
            {
                switch (choice)
                {
                    case 1:
                        entry.quizQChoice1 = newQQChoice;
                        break;
                    case 2:
                        entry.quizQChoice2 = newQQChoice;
                        break;
                    case 3:
                        entry.quizQChoice3 = newQQChoice;
                        break;
                    case 4:
                        entry.quizQChoice4 = newQQChoice;
                        break;
                }
                break;
            }
        }
    }

    public void UpdateQQAnswer(string prevQQCode, int newQQAnswer)
    {
        foreach (QuizQEntry entry in Struct.list)
        {
            if (prevQQCode.Equals(entry.quizQCode))
            {
                entry.quizQAnswer = newQQAnswer;
                break;
            }
        }
    }

    public void UpdateSCode(string prevQQCode, string newSCode)
    {
        foreach (QuizQEntry entry in Struct.list)
        {
            if (prevQQCode.Equals(entry.quizQCode))
            {
                entry.subjectCode = newSCode;
                break;
            }
        }
    }
}

[System.Serializable]
public class QuizQStruct
{
    public List<QuizQEntry> list = new();
}

[System.Serializable]
public class QuizQEntry
{
    public string quizQCode;
    public string quizQPhase;
    public int quizQNumber;
    public string quizQDescription;
    public string quizQChoice1;
    public string quizQChoice2;
    public string quizQChoice3;
    public string quizQChoice4;
    public int quizQAnswer;
    public string subjectCode;
}
