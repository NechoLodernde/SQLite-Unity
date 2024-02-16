using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KRSManager : MonoBehaviour
{
    public static KRSManager Instance { get; private set; }
    public KRSStruct Struct;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string kCode, int kNumber,
        string kYear, int kSemester, string kStatus)
    {
        KRSEntry newEntry = new();
        newEntry.krsCode = kCode;
        newEntry.krsNumber = kNumber;
        newEntry.krsYear = kYear;
        newEntry.krsSemester = kSemester;
        newEntry.krsStatus = kStatus;
        Struct.list.Add(newEntry);
    }

    public void DeleteData(string kCode)
    {
        foreach (KRSEntry entry in Struct.list)
        {
            if (entry.krsCode.Equals(kCode))
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

    public void UpdateKCode(string prevKCode, string newKCode)
    {
        foreach (KRSEntry entry in Struct.list)
        {
            if (entry.krsCode.Equals(prevKCode))
            {
                entry.krsCode = newKCode;
                break;
            }
        }
    }

    public void UpdateKNumber(string prevKCode, int newKNumber)
    {
        foreach (KRSEntry entry in Struct.list)
        {
            if (entry.krsCode.Equals(prevKCode))
            {
                entry.krsNumber = newKNumber;
                break;
            }
        }
    }

    public void UpdateKYear(string prevKCode, string newKYear)
    {
        foreach (KRSEntry entry in Struct.list)
        {
            if (entry.krsCode.Equals(prevKCode))
            {
                entry.krsYear = newKYear;
                break;
            }
        }
    }

    public void UpdateKSemester(string prevKCode, int newKSemester)
    {
        foreach (KRSEntry entry in Struct.list)
        {
            if (entry.krsCode.Equals(prevKCode))
            {
                entry.krsSemester = newKSemester;
                break;
            }
        }
    }

    public void UpdateKStatus(string prevKCode, string newKStatus)
    {
        foreach (KRSEntry entry in Struct.list)
        {
            if (entry.krsCode.Equals(prevKCode))
            {
                entry.krsStatus = newKStatus;
                break;
            }
        }
    }
}

[System.Serializable]
public class KRSStruct
{
    public List<KRSEntry> list = new();
}

[System.Serializable]
public class KRSEntry
{
    public string krsCode;
    public int krsNumber;
    public string krsYear;
    public int krsSemester;
    public string krsStatus;
}