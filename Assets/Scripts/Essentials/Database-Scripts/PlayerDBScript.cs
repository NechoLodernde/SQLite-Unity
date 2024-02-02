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
    public static PlayerDBScript Instance { get; private set; }

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

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        dbConnect = new SqliteConnection(conn);

        CreateDB();
    }

    public void CreateDB()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "CREATE TABLE IF NOT EXISTS playerdata (" +
                "[id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "[player_id] VARCHAR(100) NOT NULL UNIQUE, " +
                "[player_name] VARCHAR(128) NOT NULL UNIQUE, " +
                "[player_gender] VARCHAR(12) NOT NULL, " +
                "[player_faculty] VARCHAR(128) NOT NULL, " +
                "[player_study_program] VARCHAR(256) NOT NULL, " +
                "[player_semester] INTEGER DEFAULT '1' NOT NULL, " +
                "[active_quest_code] VARCHAR(100) NOT NULL);";

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
            dbConnect.Close();
        }
    }

    public void DeleteDB()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "DELETE FROM playerdata;";

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
            dbConnect.Close();
        }
    }

    public int CountData()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "SELECT COUNT(player_id) FROM playerdata;";

            dbCommand.CommandText = sqlQuery;
            string rawData = dbCommand.ExecuteScalar().ToString();
            int.TryParse(rawData, out int y);
            int countData = y;
            return countData;
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

            PlayerDataManager.Instance.InsertNewData(randomPlayerID,
                pName, pGender, pFaculty, pStudyProgram, pSemester, 
                "IntroPKKMBQuestA1");

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
            dbConnect.Close();
        }
    }

    public void LoadPlayers()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            sqlQuery = "SELECT * FROM playerdata;";
            dbCommand.CommandText = sqlQuery;
            dbReader = dbCommand.ExecuteReader();
            // int.TryParse(ageField.text.ToString(), out int y);
            // int pAge = y;
            while (dbReader.Read())
            {
                string pID, pName, pGender, pFaculty, pStudyProgram,
                    pActiveQuest;
                int pSemester;
                pID = "" + dbReader["player_id"];
                pName = "" + dbReader["player_name"];
                pGender = "" + dbReader["player_gender"];
                pFaculty = "" + dbReader["player_faculty"];
                pStudyProgram = "" + dbReader["player_study_program"];
                pActiveQuest = "" + dbReader["active_quest_code"];

                int.TryParse(("" + dbReader["player_semester"]), out int y);
                pSemester = y;
                Debug.Log("The Data:");
                Debug.Log("Player ID: " + pID);
                Debug.Log("Name: " + pName);
                Debug.Log("Gender: " + pGender);
                Debug.Log("Faculty: " + pFaculty);
                Debug.Log("Study Program: " + pStudyProgram);
                Debug.Log("Semester: " + pSemester);
                Debug.Log("Active Quest: " + pActiveQuest);
                Debug.Log("End Data");
            }

            dbReader.Close();
        }
        dbConnect.Close();
    }

    public void UpdateSemester()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            PlayerDEntry rawData = ReturnPDE();
            int newPSem = rawData.playerSemester + 1;
            
            sqlQuery = "UPDATE playerdata SET player_semester = '" + newPSem + 
                "' WHERE player_id = '" + rawData.playerID + "';";

            PlayerDataManager.Instance.UpdateSemester(rawData.playerID,
                newPSem);

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
            dbConnect.Close();
        }
    }

    public void UpdateQuestID(string prevQuestID)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            PlayerDEntry rawData = ReturnPDE();
            // Add reference string that will contain the new quest ID
            //string newQuestID = "ExampleNewID";

            sqlQuery = "UPDATE playerdata SET active_quest_code = '" + prevQuestID +
                "' WHERE player_id = '" + rawData.playerID + "';";

            PlayerDataManager.Instance.UpdateQuestID(rawData.playerID, 
                prevQuestID);

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
            dbConnect.Close();
        }
    }

    public void SavePlayerData()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            PlayerDEntry rawData = ReturnPDE();

            sqlQuery = "UPDATE playerdata SET player_name = '" + rawData.playerName + "'," +
                " player_gender = '" + rawData.playerGender + "', " + 
                "player_faculty = '" + rawData.playerFaculty + "', "+ 
                "player_study_program = '" + rawData.playerStudyProgram + "', " +
                "player_semester = '" + rawData.playerSemester + "', " +
                "active_quest_code = '" + rawData.activeQuestCode + "' " +
                "WHERE player_id = '" + rawData.playerID + "';";

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
        RNGCrisp rng = new();
        // Populate with random bytes
        rng.GetBytes(random);

        // Convert random bytes to string
        string randomBase64 = Con64.ToBase64String(random);
        // Return string
        return randomBase64;
    }

    public PlayerDEntry ReturnPDE()
    {
        PlayerDEntry RawData;
        RawData = PlayerDataManager.Instance.Struct.list.ToArray()[0];
        return RawData;
    }
}