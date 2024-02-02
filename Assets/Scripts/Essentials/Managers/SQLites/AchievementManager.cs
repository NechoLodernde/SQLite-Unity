using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }
    public AchievementStruct Struct;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string aID, int aNumber,
        string aName, string aUText)
    {
        AchievementEntry newEntry = new();
        newEntry.achievementID = aID;
        newEntry.achievementNumber = aNumber;
        newEntry.achievementName = aName;
        newEntry.achievementUnlockedText = aUText;
        Struct.list.Add(newEntry);
    }

    public void DeleteData(string prevAID)
    {
        foreach (AchievementEntry achieve in Struct.list)
        {
            if (achieve.achievementID.Equals(prevAID))
            {
                Struct.list.Remove(achieve);
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
        foreach (AchievementEntry entry in Struct.list)
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
        foreach (AchievementEntry entry in Struct.list)
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
        foreach (AchievementEntry entry in Struct.list)
        {
            if (prevAID.Equals(entry.achievementID))
            {
                entry.achievementName = newAName;
                break;
            }
        }
    }

    public void UpdateAUText(string prevAID, string newAUText)
    {
        foreach (AchievementEntry entry in Struct.list)
        {
            if (prevAID.Equals(entry.achievementID))
            {
                entry.achievementUnlockedText = newAUText;
                break;
            }
        }
    }
}

[System.Serializable]
public class AchievementStruct
{
    public List<AchievementEntry> list = new();
}

[System.Serializable]
public class AchievementEntry
{
    public string achievementID;
    public int achievementNumber;
    public string achievementName, achievementUnlockedText;
}