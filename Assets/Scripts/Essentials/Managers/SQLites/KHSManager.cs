using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHSManager : MonoBehaviour
{
    public static KHSManager Instance { get; private set; }
    public KHSStruct Struct;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string kCode, int kNumber,
        int kSemester, string sCode, string sName, int sTCredit,
        string kGrade, string kNote)
    {
        KHSEntry newEntry = new();
        newEntry.khsCode = kCode;
        newEntry.khsNumber = kNumber;
        newEntry.khsSemester = kSemester;
        newEntry.subjectCode = sCode;
        newEntry.subjectName = sName;
        newEntry.subjectTotalCredit = sTCredit;
        newEntry.khsGrade = kGrade;
        newEntry.khsNote = kNote;
        Struct.list.Add(newEntry);
    }

    public void DeleteData(string kCode, int kSemester)
    {
        foreach (KHSEntry entry in Struct.list)
        {
            if (entry.khsCode.Equals(kCode) &&
                entry.khsSemester == kSemester)
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

    public void UpdateKCode(string prevKCode, int prevKSemester,
        string newKCode)
    {
        foreach (KHSEntry entry in Struct.list)
        {
            if (prevKCode.Equals(entry.khsCode) &&
                prevKSemester.Equals(entry.khsSemester))
            {
                entry.khsCode = newKCode;
                break;
            }
        }
    }

    public void UpdateKNumber(string prevKCode, int prevKSemester,
        int newKNumber)
    {
        foreach (KHSEntry entry in Struct.list)
        {
            if (prevKCode.Equals(entry.khsCode) &&
                prevKSemester.Equals(entry.khsSemester))
            {
                entry.khsNumber = newKNumber;
                break;
            }
        }
    }

    public void UpdateKSemester(string prevKCode, int prevKSemester,
        int newKSemester)
    {
        foreach (KHSEntry entry in Struct.list)
        {
            if (prevKCode.Equals(entry.khsCode) &&
                prevKSemester.Equals(entry.khsSemester))
            {
                entry.khsSemester = newKSemester;
                break;
            }
        }
    }

    public void UpdateSCode(string prevKCode, int prevKSemester,
        string newSCode)
    {
        foreach (KHSEntry entry in Struct.list)
        {
            if (prevKCode.Equals(entry.khsCode) &&
                prevKSemester.Equals(entry.khsSemester))
            {
                entry.subjectCode = newSCode;
                break;
            }
        }
    }

    public void UpdateSName(string prevKCode, int prevKSemester,
        string newSName)
    {
        foreach (KHSEntry entry in Struct.list)
        {
            if (prevKCode.Equals(entry.khsCode) &&
                prevKSemester.Equals(entry.khsSemester))
            {
                entry.subjectName = newSName;
                break;
            }
        }
    }

    public void UpdateSTCredit(string prevKCode, int prevKSemester,
        int newSTCredit)
    {
        foreach (KHSEntry entry in Struct.list)
        {
            if (prevKCode.Equals(entry.khsCode) &&
                prevKSemester.Equals(entry.khsSemester))
            {
                entry.subjectTotalCredit = newSTCredit;
                break;
            }
        }
    }

    public void UpdateKGrade(string prevKCode, int prevKSemester,
        string newKGrade)
    {
        foreach (KHSEntry entry in Struct.list)
        {
            if (prevKCode.Equals(entry.subjectCode) &&
                prevKSemester.Equals(entry.khsSemester))
            {
                entry.khsGrade = newKGrade;
                break;
            }
        }
    }

    public void UpdateKNote(string prevKCode, int prevKSemester,
        string newKNote)
    {
        foreach (KHSEntry entry in Struct.list)
        {
            if (prevKCode.Equals(entry.subjectCode) &&
                prevKSemester.Equals(entry.khsSemester))
            {
                entry.khsNote = newKNote;
                break;
            }
        }
    }
}

[System.Serializable]
public class KHSStruct
{
    public List<KHSEntry> list = new();
}

[System.Serializable]
public class KHSEntry
{
    public string khsCode;
    public int khsNumber, khsSemester;
    public string subjectCode, subjectName;
    public int subjectTotalCredit;
    public string khsGrade, khsNote;
}
