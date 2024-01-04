using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class AchievementDBScript : MonoBehaviour
{
    public static AchievementDBScript AchievementDBInstance { get; private set; }

    private readonly string dbName = "/Achievement.s3db";
    private readonly string refName = "/AchievementList.s3db";
    private readonly string filepath = Application.dataPath +
        "/StreamingAssets/Database";
    private readonly string refFilepath = Application.dataPath +
        "/StreamingAssets/Database";
    string conn, refConn, refSqlQuery, sqlQuery;
    IDbConnection dbConnect;
    IDbConnection dbRefConnect;
    IDbCommand dbCommand;
    IDbCommand dbRefCommand;
    IDataReader dbReader;

    private void Awake()
    {
        string filePath = filepath + dbName;
        conn = "URI=file:" + filePath;
        string refFilePath = refFilepath + refName;
        refConn = "URI=file:" + refFilePath;

        AchievementDBInstance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        dbConnect = new SqliteConnection(conn);
        dbRefConnect = new SqliteConnection(refConn);
    }

    public void CreateDB()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "CREATE TABLE IF NOT EXISTS achievement (" +
                "[id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "[achievement_id] VARCHAR(64) NOT NULL UNIQUE, " +
                "[achievement_number] INTEGER NOT NULL UNIQUE, " +
                "[achievement_name] VARCHAR(128) NOT NULL UNIQUE, " +
                "[achievement_unlocked_text] VARCHAR(512) NOT NULL);";

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

            sqlQuery = "DELETE FROM achievement;";

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

            sqlQuery = "SELECT COUNT(achievement_id) from achievement;";

            dbCommand.CommandText = sqlQuery;
            string rawData = dbCommand.ExecuteScalar().ToString();
            int.TryParse(rawData, out int y);
            int countData = y;
            dbConnect.Close();
            return countData;
        }
    }

    public void AddAchievement(string aCode, string uCode, string text)
    {
        using (dbRefConnect = new SqliteConnection(refConn))
        {
            dbRefConnect.Open();
            dbRefCommand = dbRefConnect.CreateCommand();
            refSqlQuery = "SELECT * FROM achievementlist;";
            dbRefCommand.CommandText = refSqlQuery;
            dbReader = dbRefCommand.ExecuteReader();
            int totalAchievement = CountData();
            while (dbReader.Read())
            {
                string aID, aName, aUCode;
                aID = "" + dbReader["achievement_id"];
                aUCode = "" + dbReader["unlock_code"];
                if (aID.Equals(aCode) && aUCode.Equals(uCode))
                {
                    aName = "" + dbReader["achievement_name"];
                    totalAchievement++;
                    using (dbConnect = new SqliteConnection(conn))
                    {
                        dbConnect.Open();
                        dbCommand = dbConnect.CreateCommand();
                        sqlQuery = "INSERT INTO achievement (" +
                            "achievement_id, achievement_number, " +
                            "achievement_name, achievement_unlocked_text) " +
                            "VALUES ('" + aID + "', '" + totalAchievement +
                            "', '" + aName + "', '" + text + "');";

                        AchievementManager.AchievementInstance.InsertNewData(
                            aID, totalAchievement, aName, "Congrats!");

                        dbCommand.CommandText = sqlQuery;
                        dbCommand.ExecuteScalar();
                    }
                    dbConnect.Close();
                }
            }
            dbReader.Close();
        }
        dbRefConnect.Close();
    }

    public void DeleteAchievement(string prevAID)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            Debug.Log("Input id: " + prevAID);
            sqlQuery = "DELETE FROM achievement WHERE " +
                "achievement_id='" + prevAID + "';";
            AchievementManager.AchievementInstance.DeleteData(prevAID);
            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
        }
        dbConnect.Close();
    }

    public AchievementEntry[] ReturnAEArray()
    {
        AchievementEntry[] RawData;
        RawData = AchievementManager.AchievementInstance.achievementStruct.list.ToArray();
        return RawData;
    }
}
