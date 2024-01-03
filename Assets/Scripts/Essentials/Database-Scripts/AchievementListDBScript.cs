using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class AchievementListDBScript : MonoBehaviour
{
    public static AchievementListDBScript AchievementListDBInstance { get; private set; }

    private readonly string dbName = "/AchievementList.s3db";
    private readonly string dbFilepath = Application.dataPath +
        "/StreamingAssets/Database";
    string conn, sqlQuery;
    IDbConnection dbConnect;
    IDbCommand dbCommand;
    IDataReader dbReader;

    private void Awake()
    {
        string filepath = dbFilepath + dbName;
        conn = "URI=file:" + filepath;

        AchievementListDBInstance = this;

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

            sqlQuery = "CREATE TABLE IF NOT EXISTS achievementlist (" +
                "[id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "[achievement_id] VARCHAR(64) NOT NULL UNIQUE, " +
                "[achievement_name] VARCHAR(128) NOT NULL UNIQUE, " +
                "[unlock_code] VARCHAR(64) NOT NULL UNIQUE);";

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
            dbConnect.Close();
        }
    }

    public void DeleteDB()
    {
        dbConnect.Open();
        dbCommand = dbConnect.CreateCommand();

        sqlQuery = "DELETE FROM achievementlist;";

        dbCommand.CommandText = sqlQuery;
        dbCommand.ExecuteScalar();
        dbConnect.Close();
    }

    public int CountData()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "SELECT COUNT(achievement_id) from " +
                "achievementlist;";

            dbCommand.CommandText = sqlQuery;
            string rawData = dbCommand.ExecuteScalar().ToString();
            int.TryParse(rawData, out int y);
            int countData = y;
            dbConnect.Close();
            return countData;
        }
    }

    public void AddAchievement(string aId, string aName, string uCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            sqlQuery = "INSERT INTO achievementlist (achievement_id, " +
                "achievement_name, unlock_code) VALUES ('" + aId + "', '" +
                aName + "', '" + uCode + "');";

            AchievementListManager.AchievementListInstance.InsertNewData(
                aId, aName, uCode);

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
            dbConnect.Close();
        }
    }

    public void LoadAchievementsData()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            sqlQuery = "SELECT * FROM achievementlist;";
            dbCommand.CommandText = sqlQuery;
            dbReader = dbCommand.ExecuteReader();

            while (dbReader.Read())
            {
                string aID, aName, uCode;
                aID = "" + dbReader["achievement_id"];
                aName = "" + dbReader["achievement_name"];
                uCode = "" + dbReader["unlock_code"];

                Debug.Log("New Data");
                Debug.Log("Achievement ID: " + aID);
                Debug.Log("Achievement Name: " + aName);
                Debug.Log("Unlock Code: " + uCode);
                Debug.Log("End Data");
            }
            dbReader.Close();
        }
        dbConnect.Close();
    }

    public void SaveAchievementData()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            AchievementDataEntry[] rawData = ReturnADEArray();

            foreach (AchievementDataEntry achieve in rawData)
            {
                sqlQuery = "UPDATE achievementlist SET achievement_name = '" +
                    achieve.achievementName + "', unlock_code = '" +
                    achieve.unlockCode + "' WHERE achievement_id = '" +
                    achieve.achievementID + "';";

                dbCommand.CommandText = sqlQuery;
                dbCommand.ExecuteScalar();
            }

            dbConnect.Close();
        }
    }

    public AchievementDataEntry[] ReturnADEArray()
    {
        AchievementDataEntry[] RawData;
        RawData = AchievementListManager.AchievementListInstance.achievementStruct.list.ToArray();
        return RawData;
    }
}
