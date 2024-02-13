using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamScoreManager : MonoBehaviour
{
    public static ExamScoreManager Instance { get; private set; }
    public ExamScoreStruct Struct;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string eSCode, string eSTerm,
        int eSPoint, string sCode)
    {
        ExamScoreEntry newEntry = new();
        newEntry.examScoreCode = eSCode;
        newEntry.examScoreTerm = eSTerm;
        newEntry.examScorePoint = eSPoint;
        newEntry.subjectCode = sCode;
        Struct.list.Add(newEntry);
    }

    public void DeleteData(string prevESCode, string prevESTerm)
    {
        foreach (ExamScoreEntry eScore in Struct.list)
        {
            if (eScore.examScoreCode.Equals(prevESCode) &&
                eScore.examScoreTerm.Equals(prevESTerm))
            {
                Struct.list.Remove(eScore);
                break;
            }
        }
    }

    public void ClearList()
    {
        Struct.list.Clear();
    }

    public void UpdateESCode(string prevESCode, string prevESTerm, 
        string newESCode)
    {
        foreach (ExamScoreEntry eScore in Struct.list)
        {
            if (prevESCode.Equals(eScore.examScoreCode) &&
                prevESTerm.Equals(eScore.examScoreTerm))
            {
                eScore.examScoreCode = newESCode;
                break;
            }
        }
    }

    public void UpdateESTerm(string prevESCode,string prevESTerm,
        string newESTerm)
    {
        foreach (ExamScoreEntry eScore in Struct.list)
        {
            if (prevESCode.Equals(eScore.examScoreCode) &&
                prevESTerm.Equals(eScore.examScoreTerm))
            {
                eScore.examScoreTerm = newESTerm;
                break;
            }
        }
    }

    public void UpdateESPoint(string prevESCode, string prevESTerm,
        int newESPoint)
    {
        foreach (ExamScoreEntry eScore in Struct.list)
        {
            if (prevESCode.Equals(eScore.examScoreCode) &&
                prevESTerm.Equals(eScore.examScoreTerm))
            {
                eScore.examScorePoint = newESPoint;
                break;
            }
        }
    }

    public void UpdateSCode(string prevESCode, string prevESTerm,
        string newSCode)
    {
        foreach (ExamScoreEntry eScore in Struct.list)
        {
            if (prevESCode.Equals(eScore.examScoreCode) &&
                prevESTerm.Equals(eScore.examScoreTerm))
            {
                eScore.subjectCode = newSCode;
                break;
            }
        }
    }
}

[System.Serializable]
public class ExamScoreStruct
{
    public List<ExamScoreEntry> list = new();
}

[System.Serializable]
public class ExamScoreEntry
{
    public string examScoreCode, examScoreTerm;
    public int examScorePoint;
    public string subjectCode;
}
