using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KRSInfoManager : MonoBehaviour
{
    public static KRSInfoManager Instance { get; private set; }
    public KRSIStruct Struct;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string kICode, int kINum,
        string kISCode, string kISName, int kITCredit, string kCode)
    {
        KRSIEntry newEntry = new();
        newEntry.krsInfoCode = kICode;
        newEntry.krsInfoNumber = kINum;
        newEntry.krsInfoSubjectCode = kISCode;
        newEntry.krsInfoSubjectName = kISName;
        newEntry.krsInfoTotalCredit = kITCredit;
        newEntry.krsCode = kCode;
        Struct.list.Add(newEntry);
    }

    public void DeleteData(string prevKICode)
    {
        foreach (KRSIEntry entry in Struct.list)
        {
            if (entry.krsInfoCode.Equals(prevKICode))
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

    public void UpdateKICode(string prevKICode, string newKICode)
    {
        foreach (KRSIEntry entry in Struct.list)
        {
            if (prevKICode.Equals(entry.krsInfoCode))
            {
                entry.krsInfoCode = newKICode;
                break;
            }
        }
    }

    public void UpdateKINumber(string prevKICode, int newKINumber)
    {
        foreach (KRSIEntry entry in Struct.list)
        {
            if (prevKICode.Equals(entry.krsInfoCode))
            {
                entry.krsInfoNumber = newKINumber;
                break;
            }
        }
    }

    public void UpdateKISCode(string prevKICode, string newKISCode)
    {
        foreach (KRSIEntry entry in Struct.list)
        {
            if (prevKICode.Equals(entry.krsInfoCode))
            {
                entry.krsInfoSubjectCode = newKISCode;
                break;
            }
        }
    }

    public void UpdateKISName(string prevKICode, string newKISName)
    {
        foreach (KRSIEntry entry in Struct.list)
        {
            if (prevKICode.Equals(entry.krsInfoCode))
            {
                entry.krsInfoSubjectName = newKISName;
                break;
            }
        }
    }

    public void UpdateKITCredit(string prevKICode, int newKITCred)
    {
        foreach (KRSIEntry entry in Struct.list)
        {
            if (prevKICode.Equals(entry.krsInfoCode))
            {
                entry.krsInfoTotalCredit = newKITCred;
                break;
            }
        }
    }

    public void UpdateKCode(string prevKICode, string newKCode)
    {
        foreach (KRSIEntry entry in Struct.list)
        {
            if (prevKICode.Equals(entry.krsInfoCode))
            {
                entry.krsCode = newKCode;
                break;
            }
        }
    }
}

[System.Serializable]
public class KRSIStruct
{
    public List<KRSIEntry> list = new();
}

[System.Serializable]
public class KRSIEntry
{
    public string krsInfoCode;
    public int krsInfoNumber;
    public string krsInfoSubjectCode, krsInfoSubjectName; 
    public int krsInfoTotalCredit; 
    public string krsCode;
}
