using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHSTotalGradeManager : MonoBehaviour
{
    public static KHSTotalGradeManager Instance { get; private set; }
    public KHSTGStruct Struct;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string kCode, int kSem,
        double kTotGrad)
    {
        KHSTGEntry newEntry = new();
        newEntry.khsCode = kCode;
        newEntry.khsSemester = kSem;
        newEntry.khsTotalGrade = kTotGrad;
        Struct.list.Add(newEntry);
    }

    public void DeleteData(string prevKCode)
    {
        foreach (KHSTGEntry entry in Struct.list)
        {
            if (entry.khsCode.Equals(prevKCode))
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

    public void UpdateKTGCode(string prevKCode, string newKCode)
    {
        foreach (KHSTGEntry entry in Struct.list)
        {
            if (prevKCode.Equals(entry.khsCode))
            {
                entry.khsCode = newKCode;
                break;
            }
        }
    }

    public void UpdateKTGSemester(string prevKCode, int newKSem)
    {
        foreach (KHSTGEntry entry in Struct.list)
        {
            if (prevKCode.Equals(entry.khsCode))
            {
                entry.khsSemester = newKSem;
                break;
            }
        }
    }

    public void UpdateKTGrade(string prevKCode, double newKTGrade)
    {
        foreach (KHSTGEntry entry in Struct.list)
        {
            if (prevKCode.Equals(entry.khsCode))
            {
                entry.khsTotalGrade = newKTGrade;
                break;
            }
        }
    }
}

[System.Serializable]
public class KHSTGStruct
{
    public List<KHSTGEntry> list = new();
}

[System.Serializable]
public class KHSTGEntry
{
    public string khsCode;
    public int khsSemester;
    public double khsTotalGrade;
}
