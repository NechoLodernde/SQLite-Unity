using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    public QuestStruct Struct;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string qCode, int qNumber,
        string qType, string qTitle, string qInfo,
        int qReward, string cCode)
    {
        QuestEntry newEntry = new();
        newEntry.questCode = qCode;
        newEntry.questNumber = qNumber;
        newEntry.questType = qType;
        newEntry.questTitle = qTitle;
        newEntry.questInfo = qInfo;
        newEntry.questReward = qReward;
        newEntry.completionCode = cCode;
        Struct.list.Add(newEntry);
    }

    public void DeleteData(string prevQCode)
    {
        foreach (QuestEntry entry in Struct.list)
        {
            if (entry.questCode.Equals(prevQCode))
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

    public void UpdateQCode(string prevQCode, string newQCode)
    {
        foreach (QuestEntry entry in Struct.list)
        {
            if (prevQCode.Equals(entry.questCode))
            {
                entry.questCode = newQCode;
                break;
            }
        }
    }

    public void UpdateQNumber(string prevQCode, int newQNumber)
    {
        foreach (QuestEntry entry in Struct.list)
        {
            if (prevQCode.Equals(entry.questCode))
            {
                entry.questNumber = newQNumber;
                break;
            }
        }
    }

    public void UpdateQType(string prevQCode, string newQType)
    {
        foreach (QuestEntry entry in Struct.list)
        {
            if (prevQCode.Equals(entry.questCode))
            {
                entry.questType = newQType;
                break;
            }
        }
    }

    public void UpdateQTitle(string prevQCode, string newQTitle)
    {
        foreach (QuestEntry entry in Struct.list)
        {
            if (prevQCode.Equals(entry.questCode))
            {
                entry.questTitle = newQTitle;
                break;
            }
        }
    }

    public void UpdateQInfo(string prevQCode, string newQInfo)
    {
        foreach (QuestEntry entry in Struct.list)
        {
            if (prevQCode.Equals(entry.questCode))
            {
                entry.questInfo = newQInfo;
                break;
            }
        }
    }

    public void UpdateQReward(string prevQCode, int newQReward)
    {
        foreach (QuestEntry entry in Struct.list)
        {
            if (prevQCode.Equals(entry.questCode))
            {
                entry.questReward = newQReward;
                break;
            }
        }
    }

    public void UpdateCCode(string prevQCode, string newCCode)
    {
        foreach (QuestEntry entry in Struct.list)
        {
            if (prevQCode.Equals(entry.questCode))
            {
                entry.completionCode = newCCode;
                break;
            }
        }
    }
}

[System.Serializable]
public class QuestStruct
{
    public List<QuestEntry> list = new();
}

[System.Serializable]
public class QuestEntry
{
    public string questCode;
    public int questNumber;
    public string questType, questTitle, questInfo;
    public int questReward;
    public string completionCode;
}
