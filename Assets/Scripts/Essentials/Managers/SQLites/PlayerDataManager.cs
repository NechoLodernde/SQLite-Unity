using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }
    public PlayerDStruct Struct;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string playerId, string playerName,
        string playerGender, string playerFaculty, string playerStudyProgram,
        int playerSemester, string activeQuestCode)
    {
        PlayerDEntry newEntry = new();
        newEntry.playerID = playerId;
        newEntry.playerName = playerName;
        newEntry.playerGender = playerGender;
        newEntry.playerFaculty = playerFaculty;
        newEntry.playerStudyProgram = playerStudyProgram;
        newEntry.playerSemester = playerSemester;
        newEntry.activeQuestCode = activeQuestCode;
        Struct.list.Add(newEntry);
    }

    public void DeleteData(string prevPID)
    {
        foreach (PlayerDEntry entry in Struct.list)
        {
            if (entry.playerID.Equals(prevPID))
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

    public void UpdatePlayerID(string prevPID, string newPID)
    {
        foreach (PlayerDEntry entry in Struct.list)
        {
            if (prevPID.Equals(entry.playerID))
            {
                entry.playerID = newPID;
                break;
            }
        }
    }

    public void UpdatePlayerName(string prevPID, string newPName)
    {
        foreach (PlayerDEntry entry in Struct.list)
        {
            if (prevPID.Equals(entry.playerID))
            {
                entry.playerName = newPName;
                break;
            }
        }
    }

    public void UpdatePlayerFaculty(string prevPID, string newPFaculty)
    {
        foreach (PlayerDEntry entry in Struct.list)
        {
            if (prevPID.Equals(entry.playerID))
            {
                entry.playerFaculty = newPFaculty;
                break;
            }
        }
    }

    public void UpdatePlayerGender(string prevPID, string newPGender)
    {
        foreach (PlayerDEntry entry in Struct.list)
        {
            if (prevPID.Equals(entry.playerID))
            {
                entry.playerGender = newPGender;
                break;
            }
        }
    }

    public void UpdateSProgram(string prevPID, string newSProgram)
    {
        foreach (PlayerDEntry entry in Struct.list)
        {
            if (prevPID.Equals(entry.playerID))
            {
                entry.playerStudyProgram = newSProgram;
                break;
            }
        }
    }

    public void UpdateQuestID(string prevPID, string newQID)
    {
        //Struct.list.ToArray()[0].activeQuestCode = newQuestID;
        foreach (PlayerDEntry entry in Struct.list)
        {
            if (prevPID.Equals(entry.playerID))
            {
                entry.activeQuestCode = newQID;
                break;
            }
        }
    }

    public void UpdateSemester(string prevPID, int newSem)
    {
        //Struct.list.ToArray()[0].playerSemester = newSemester;
        foreach (PlayerDEntry entry in Struct.list)
        {
            if (prevPID.Equals(entry.playerID))
            {
                entry.playerSemester = newSem;
                break;
            }
        }
    }
}

[System.Serializable]
public class PlayerDStruct
{
    public List<PlayerDEntry> list = new();
}

[System.Serializable]
public class PlayerDEntry
{
    public string playerID, playerName, playerFaculty,
        playerGender, playerStudyProgram, activeQuestCode;
    public int playerSemester;
}