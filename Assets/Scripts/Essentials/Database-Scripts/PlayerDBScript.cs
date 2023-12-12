using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using RNGCrisp = System.Security.Cryptography.RNGCryptoServiceProvider;
using Con64 = System.Convert;

public class PlayerDBScript : MonoBehaviour
{
    public static PlayerDBScript playerDBScriptInstance { get; private set; }
    public PlayerDataStruct playerStruct;

    private readonly string dbName = "/PlayerData.s3db";
    private readonly string filepath = Application.dataPath + "/StreamingAssets/Database";
    string conn, sqlQuery;
    IDbConnection dbConnect;
    IDbCommand dbCommand;
    IDataReader dbReader;

    private void Awake()
    {
        string filePath = filepath + dbName;
        conn = "URI=file:" + filePath;

        playerDBScriptInstance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        dbConnect = new SqliteConnection(conn);

        CreateDB();
    }

    private void CreateDB()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "CREATE TABLE IF NOT EXISTS playerdata (" +
                "[id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "[player_id] VARCHAR(100) NOT NULL UNIQUE, " +
                "[player_name] VARCHAR(128) NOT NULL UNIQUE, " +
                "[player_gender] VARCHAR(10) NOT NULL, " +
                "[player_faculty] VARCHAR(48) NOT NULL, " +
                "[player_study_program] VARCHAR(256) NOT NULL, " +
                "[player_semester] INTEGER DEFAULT '1' NOT NULL, " +
                "[active_quest_code] VARCHAR(100) NOT NULL);";

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
            dbConnect.Close();
        }
    }

    public void AddPlayer(string pName, string pGender, string pFaculty,
        string pStudyProgram, int pSemester)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            string randomPlayerID = RandomBaseString64();
            sqlQuery = "INSERT INTO playerdata (player_id, player_name, " +
                "player_gender, player_faculty, player_study_program," +
                "player_semester, active_quest_code) VALUES " +
                "('" + randomPlayerID + "', '" + pName + "', '" + pGender +"', '"
                + pFaculty + "', '" + pStudyProgram + "', '" + pSemester + "', '"
                + "IntroPKKMBQuestA1');";

            PlayerDataEntry newEntry = new();
            newEntry.playerID = randomPlayerID;
            newEntry.playerName = pName;
            newEntry.playerGender = pGender;
            newEntry.playerFaculty = pFaculty;
            newEntry.playerStudyProgram = pStudyProgram;
            newEntry.playerSemester = pSemester;
            newEntry.activeQuestCode = "IntroPKKMBQuestA1";
            playerStruct.list.Add(newEntry);

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
            dbConnect.Close();
        }
    }

    public void UpdateSemester()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            PlayerDataEntry rawData = ReturnPDE();
            int playerSemNew = rawData.playerSemester + 1;
            
            sqlQuery = "UPDATE playerdata SET player_semester = '" + playerSemNew + 
                "' WHERE playerID = '" + rawData.playerID + "';";

            playerDBScriptInstance.playerStruct.list.ToArray()[0].playerSemester = playerSemNew;

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
            dbConnect.Close();
        }
    }

    // Method to create random string with length of 88
    private string RandomBaseString64()
    {
        // Create a stronger hash code using RNGCryptoServiceProvider
        byte[] random = new byte[64];
        RNGCrisp rng = new RNGCrisp();
        // Populate with random bytes
        rng.GetBytes(random);

        // Convert random bytes to string
        string randomBase64 = Con64.ToBase64String(random);
        // Return string
        return randomBase64;
    }

    public PlayerDataEntry ReturnPDE()
    {
        PlayerDataEntry RawData;
        RawData = playerDBScriptInstance.playerStruct.list.ToArray()[0];
        return RawData;
    }
}

[System.Serializable]
public class PlayerDataStruct
{
    public List<PlayerDataEntry> list = new();
}

public class PlayerDataEntry
{
    public string playerID, playerName, playerFaculty, 
        playerGender, playerStudyProgram, activeQuestCode;
    public int playerSemester;
}