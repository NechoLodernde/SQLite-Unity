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
                "[achievement_number] INTEGER NOT NULL UNIQUE, " +
                "[achievement_name] VARCHAR(128) NOT NULL UNIQUE, " +
                "[achievement_desc] VARCHAR(1024) NOT NULL UNIQUE, " +
                "[achievement_unlock_code] VARCHAR(64) NOT NULL UNIQUE, " +
                "[unlock_status] INTEGER NOT NULL);";

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

    public void AddAchievement(string aId, string aName, 
        string aDesc, string aUCode, int uStatus)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            int totalA = CountData();
            totalA++;
            sqlQuery = "INSERT INTO achievementlist (achievement_id, " +
                "achievement_number, achievement_name, achievement_desc, " +
                "achievement_unlock_code, unlock_status) VALUES ('" + aId + "', '" + totalA + "', '" +
                aName + "', '" + aDesc + "', '" + aUCode + "', '" + uStatus + "');";

            AchievementListManager.Instance.InsertNewData(
                aId, totalA, aName, aDesc, aUCode, uStatus);

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
                string aID, aName, aDesc, aUCode;
                int aNumber, uStatus;
                aID = "" + dbReader["achievement_id"];
                int.TryParse(("" + dbReader["achievement_number"]),
                    out int x);
                aNumber = x;
                aName = "" + dbReader["achievement_name"];
                aDesc = "" + dbReader["achievement_desc"];
                aUCode = "" + dbReader["achievement_unlock_code"];
                int.TryParse(("" + dbReader["unlock_status"]),
                    out int y);
                uStatus = y;
                Debug.Log("New Data");
                Debug.Log("Achievement ID: " + aID);
                Debug.Log("Achievement Number: " + aNumber);
                Debug.Log("Achievement Name: " + aName);
                Debug.Log("Achievement Desc: " + aDesc);
                Debug.Log("Achivement Unlock Code: " + aUCode);
                Debug.Log("Unlock Status: " + uStatus);
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
            AchievementLEntry[] rawData = ReturnADEArray();

            foreach (AchievementLEntry achieve in rawData)
            {
                sqlQuery = "UPDATE achievementlist SET achievement_number = '" +
                    achieve.achievementNumber + "', achievement_name = '" +
                    achieve.achievementName + "', achievement_desc = '" +
                    achieve.achievementDesc + "', achievement_unlock_code = '" +
                    achieve.achievementUCode + "', unlock_status = '" +
                    achieve.unlockStatus + "'" + " WHERE achievement_id = '" +
                    achieve.achievementID + "';";

                dbCommand.CommandText = sqlQuery;
                dbCommand.ExecuteScalar();
            }

            dbConnect.Close();
        }
    }

    public AchievementLEntry[] ReturnADEArray()
    {
        AchievementLEntry[] RawData;
        RawData = AchievementListManager.Instance.Struct.list.ToArray();
        return RawData;
    }
}
