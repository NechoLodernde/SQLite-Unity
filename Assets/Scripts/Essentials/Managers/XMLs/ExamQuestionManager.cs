using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamQuestionManager : MonoBehaviour
{
    public static ExamQuestionManager Instance { get; private set; }
    public ExamQStruct Struct;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string eQCode, string eQTerm,
        string eQPhase, int eQNumber, string eQDescription,
        string eQChoice1, string eQChoice2, string eQChoice3,
        string eQChoice4, int eQAnswer, string sCode)
    {
        ExamQEntry newEntry = new()
        {
            examQCode = eQCode,
            examQTerm = eQTerm,
            examQPhase = eQPhase,
            examQNumber = eQNumber,
            examQDescription = eQDescription,
            examQChoice1 = eQChoice1,
            examQChoice2 = eQChoice2,
            examQChoice3 = eQChoice3,
            examQChoice4 = eQChoice4,
            examQAnswer = eQAnswer,
            subjectCode = sCode
        };
        Struct.list.Add(newEntry);
    }

    public void DeleteData(string prevEQCode)
    {
        foreach (ExamQEntry entry in Struct.list)
        {
            if (entry.examQCode.Equals(prevEQCode))
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

    public void UpdateEQCode(string prevEQCode, string newEQCode)
    {
        foreach (ExamQEntry entry in Struct.list)
        {
            if (prevEQCode.Equals(entry.examQCode))
            {
                entry.examQCode = newEQCode;
                break;
            }
        }
    }

    public void UpdateEQTerm(string prevEQCode, string newEQTerm)
    {
        foreach (ExamQEntry entry in Struct.list)
        {
            if (prevEQCode.Equals(entry.examQCode))
            {
                entry.examQTerm = newEQTerm;
                break;
            }
        }
    }

    public void UpdateEQPhase (string prevEQCode, string newEQPhase)
    {
        foreach (ExamQEntry entry in Struct.list)
        {
            if (prevEQCode.Equals(entry.examQCode))
            {
                entry.examQPhase = newEQPhase;
                break;
            }
        }
    }

    public void UpdateEQNumber (string prevEQCode, int newEQNumber)
    {
        foreach (ExamQEntry entry in Struct.list)
        {
            if (prevEQCode.Equals(entry.examQCode))
            {
                entry.examQNumber = newEQNumber;
                break;
            }
        }
    }

    public void UpdateEQDesc(string prevEQCode, string newEQDesc)
    {
        foreach (ExamQEntry entry in Struct.list)
        {
            if (prevEQCode.Equals(entry.examQCode))
            {
                entry.examQDescription = newEQDesc;
                break;
            }
        }
    }

    public void UpdateEQChoice(string prevEQCode, int choice,
        string newEQChoice)
    {
        foreach (ExamQEntry entry in Struct.list)
        {
            if (prevEQCode.Equals(entry.examQCode))
            {
                switch (choice)
                {
                    case 1:
                        entry.examQChoice1 = newEQChoice;
                        break;
                    case 2:
                        entry.examQChoice2 = newEQChoice;
                        break;
                    case 3:
                        entry.examQChoice3 = newEQChoice;
                        break;
                    case 4:
                        entry.examQChoice4 = newEQChoice;
                        break;
                }
                break;
            }
        }
    }

    public void UpdateEQAnswer(string prevEQCode, int newEQAnswer)
    {
        foreach (ExamQEntry entry in Struct.list)
        {
            if (prevEQCode.Equals(entry.examQCode))
            {
                entry.examQAnswer = newEQAnswer;
                break;
            }
        }
    }

    public void UpdateSCode(string prevEQCode, string newSCode)
    {
        foreach (ExamQEntry entry in Struct.list)
        {
            if (prevEQCode.Equals(entry.examQCode))
            {
                entry.subjectCode = newSCode;
                break;
            }
        }
    }
}

[System.Serializable]
public class ExamQStruct
{
    public List<ExamQEntry> list = new();
}

[System.Serializable]
public class ExamQEntry
{
    public string examQCode;
    public string examQTerm;
    public string examQPhase;
    public int examQNumber;
    public string examQDescription;
    public string examQChoice1;
    public string examQChoice2;
    public string examQChoice3;
    public string examQChoice4;
    public int examQAnswer;
    public string subjectCode;
}
