using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizScoreManager : MonoBehaviour
{
    public static QuizScoreManager Instance { get; private set; }
    public QuizScoreStruct Struct;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string qSCode, int qSPoint,
        string sCode)
    {
        QuizScoreEntry newEntry = new();
        newEntry.quizScoreCode = qSCode;
        newEntry.quizScorePoint = qSPoint;
        newEntry.subjectCode = sCode;
        Struct.list.Add(newEntry);
    }

    public void DeleteData(string prevQSCode)
    {
        foreach (QuizScoreEntry entry in Struct.list)
        {
            if (entry.quizScoreCode.Equals(prevQSCode))
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

    public void UpdateQSCode(string prevQSCode, string newQSCode)
    {
        foreach (QuizScoreEntry entry in Struct.list)
        {
            if (prevQSCode.Equals(entry.quizScoreCode))
            {
                entry.quizScoreCode = newQSCode;
                break;
            }
        }
    }

    public void UpdateQSPoint(string prevQSCode, int newQSPoint)
    {
        foreach (QuizScoreEntry entry in Struct.list)
        {
            if (prevQSCode.Equals(entry.quizScoreCode))
            {
                entry.quizScorePoint = newQSPoint;
                break;
            }
        }
    }

    public void UpdateSCode(string prevQSCode, string newSCode)
    {
        foreach (QuizScoreEntry entry in Struct.list)
        {
            if (prevQSCode.Equals(entry.quizScoreCode))
            {
                entry.subjectCode = newSCode;
                break;
            }
        }
    }
}

[System.Serializable]
public class QuizScoreStruct
{
    public List<QuizScoreEntry> list = new();
}

[System.Serializable]
public class QuizScoreEntry
{
    public string quizScoreCode;
    public int quizScorePoint;
    public string subjectCode;
}
