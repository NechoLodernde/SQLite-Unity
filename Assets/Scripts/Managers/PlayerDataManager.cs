using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager PlayerDataInstance { get; private set; }
    public PlayerDataStruct playerStruct;

    private void Awake()
    {
        PlayerDataInstance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string playerId, string playerName,
        string playerGender, string playerFaculty, string playerStudyProgram,
        int playerSemester, string activeQuestCode)
    {
        PlayerDataEntry newEntry = new();
        newEntry.playerID = playerId;
        newEntry.playerName = playerName;
        newEntry.playerGender = playerGender;
        newEntry.playerFaculty = playerFaculty;
        newEntry.playerStudyProgram = playerStudyProgram;
        newEntry.playerSemester = playerSemester;
        newEntry.activeQuestCode = activeQuestCode;
        playerStruct.list.Add(newEntry);
    }

    public void UpdateSemester(int newSemester)
    {
        playerStruct.list.ToArray()[0].playerSemester = newSemester;
    }

    public void UpdateQuestID(string newQuestID)
    {
        playerStruct.list.ToArray()[0].activeQuestCode = newQuestID;
    }
}

[System.Serializable]
public class PlayerDataStruct
{
    public List<PlayerDataEntry> list = new();
}

[System.Serializable]
public class PlayerDataEntry
{
    public string playerID, playerName, playerFaculty,
        playerGender, playerStudyProgram, activeQuestCode;
    public int playerSemester;
}