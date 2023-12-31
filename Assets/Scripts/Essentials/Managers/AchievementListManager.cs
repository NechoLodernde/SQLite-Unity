using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class AchievementListManager : MonoBehaviour
{
    public static AchievementListManager AchievementListInstance { get; private set; }
    public AchievementDataStruct achievementStruct;

    private void Awake()
    {
        AchievementListInstance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string aID, int aNumber, string aName,
        string aDesc, string uCode)
    {
        AchievementDataEntry newEntry = new();
        newEntry.achievementID = aID;
        newEntry.achievementNumber = aNumber;
        newEntry.achievementName = aName;
        newEntry.achievementDesc = aDesc;
        newEntry.unlockCode = uCode;
        achievementStruct.list.Add(newEntry);
    }
}

[System.Serializable]
public class AchievementDataStruct
{
    public List<AchievementDataEntry> list = new();
}

[System.Serializable]
public class AchievementDataEntry
{
    public string achievementID;
    public int achievementNumber;
    public string achievementName, achievementDesc, unlockCode;
}
