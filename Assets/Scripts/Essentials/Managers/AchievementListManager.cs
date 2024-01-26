using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementListManager : MonoBehaviour
{
    public static AchievementListManager Instance { get; private set; }
    public AchievementLStruct Struct;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string aID, int aNumber, string aName,
        string aDesc, string aUCode, int uStatus)
    {
        AchievementLEntry newEntry = new()
        {
            achievementID = aID,
            achievementNumber = aNumber,
            achievementName = aName,
            achievementDesc = aDesc,
            achievementUCode = aUCode,
            unlockStatus = uStatus
        };
        Struct.list.Add(newEntry);
    }

    public void DeleteData(string prevAID)
    {
        foreach (AchievementLEntry entry in Struct.list)
        {
            if (prevAID.Equals(entry.achievementID))
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

    public void UpdateAID(string prevAID, string newAID)
    {
        foreach (AchievementLEntry entry in Struct.list)
        {
            if (prevAID.Equals(entry.achievementID))
            {
                entry.achievementID = newAID;
                break;
            }
        }
    }

    public void UpdateANumber(string prevAID, int newANumber)
    {
        foreach (AchievementLEntry entry in Struct.list)
        {
            if (prevAID.Equals(entry.achievementID))
            {
                entry.achievementNumber = newANumber;
                break;
            }
        }
    }

    public void UpdateAName(string prevAID, string newAName)
    {
        foreach (AchievementLEntry entry in Struct.list)
        {
            if (prevAID.Equals(entry.achievementID))
            {
                entry.achievementName = newAName;
                break;
            }
        }
    }

    public void UpdateADesc(string prevAID, string newADesc)
    {
        foreach (AchievementLEntry entry in Struct.list)
        {
            if (prevAID.Equals(entry.achievementID))
            {
                entry.achievementDesc = newADesc;
                break;
            }
        }
    }

    public void UpdateAUCode(string prevAID, string newAUCode)
    {
        foreach (AchievementLEntry entry in Struct.list)
        {
            if (prevAID.Equals(entry.achievementID))
            {
                entry.achievementUCode = newAUCode;
                break;
            }
        }
    }

    public void UpdateUStatus(string prevAID, int newUStatus)
    {
        foreach (AchievementLEntry entry in Struct.list)
        {
            if (prevAID.Equals(entry.achievementID))
            {
                entry.unlockStatus = newUStatus;
                break;
            }
        }
    }
}

[System.Serializable]
public class AchievementLStruct
{
    public List<AchievementLEntry> list = new();
}

[System.Serializable]
public class AchievementLEntry
{
    public string achievementID;
    public int achievementNumber;
    public string achievementName, achievementDesc, achievementUCode;
    public int unlockStatus;
}
