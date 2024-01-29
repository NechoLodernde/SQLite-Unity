using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager AchievementInstance { get; private set; }
    public AchievementStruct achievementStruct;

    private void Awake()
    {
        AchievementInstance = this;

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
        achievementStruct.list.Add(newEntry);
    }

    public void DeleteData(string prevAID)
    {
        foreach (AchievementEntry achieve in achievementStruct.list)
        {
            if (achieve.achievementID.Equals(prevAID))
            {
                achievementStruct.list.Remove(achieve);
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