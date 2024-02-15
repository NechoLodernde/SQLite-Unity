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
