using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class AchievementDBScript : MonoBehaviour
{
    public static AchievementDBScript Instance { get; private set; }

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

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        dbConnect = new SqliteConnection(conn);
        dbRefConnect = new SqliteConnection(refConn);

        CreateDB();
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

                        AchievementManager.Instance.InsertNewData(
                            aID, totalAchievement, aName, text);

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
            AchievementManager.Instance.DeleteData(prevAID);
            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
        }

        dbConnect.Close();
    }

    public void SaveAchievements()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            AchievementEntry[] rawData = ReturnAEArray();

            foreach (AchievementEntry entry in rawData)
            {
                sqlQuery = "UPDATE achievement SET achievement_id='" +
                    entry.achievementID + "', achievement_number='" +
                    entry.achievementNumber + "', achievement_name='" +
                    entry.achievementName + "', achievement_unlocked_text='"
                    + entry.achievementUnlockedText + "' WHERE achievement_id='"
                    + entry.achievementID +"';";
                
                dbCommand.CommandText = sqlQuery;
                dbCommand.ExecuteScalar();
            }

            dbConnect.Close();
        }
    }

    public void LoadAchievements()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            sqlQuery = "SELECT * FROM achievement;";
            dbCommand.CommandText = sqlQuery;
            dbReader = dbCommand.ExecuteReader();
            
            while (dbReader.Read())
            {
                AchievementEntry newEntry = new();
                string aID, aName, aUText;
                int aNumber;
                aID = "" + dbReader["achievement_id"];
                aName = "" + dbReader["achievement_name"];
                aUText = "" + dbReader["achievement_unlocked_text"];

                int.TryParse(("" + dbReader["achievement_number"]), out int x);
                aNumber = x;
                Debug.Log("The Data:");
                Debug.Log("Achievement ID: " + aID);
                Debug.Log("Achievement Number: " + aNumber);
                Debug.Log("Achievement Name: " + aName);
                Debug.Log("Achievement Unlocked Text: " + aUText);
                Debug.Log("End of Data");

                newEntry.achievementID = aID;
                newEntry.achievementNumber = aNumber;
                newEntry.achievementName = aName;
                newEntry.achievementUnlockedText = aUText;

                AchievementManager.Instance.Struct.list.Add(newEntry);
            }

            dbReader.Close();
        }

        dbConnect.Close();
    }

    public void UpdateAID(string prevAID, string newAID)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            AchievementEntry[] rawData = ReturnAEArray();

            foreach (AchievementEntry entry in rawData)
            {
                if (entry.achievementID.Equals(prevAID))
                {
                    sqlQuery = "UPDATE achievement SET achievement_id = '" +
                    newAID + "' WHERE achievement_id = '" + prevAID + "';";

                    AchievementManager.Instance.UpdateAID(prevAID, newAID);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateANumber(string prevAID, int newANumber)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            AchievementEntry[] rawData = ReturnAEArray();

            foreach (AchievementEntry entry in rawData)
            {
                if (entry.achievementID.Equals(prevAID))
                {
                    sqlQuery = "UPDATE achievement SET achievement_number = '" +
                        newANumber + "' WHERE achievement_id = '" + prevAID + "';";

                    AchievementManager.Instance.UpdateANumber(prevAID, newANumber);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateAName(string prevAID, string newAName)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            AchievementEntry[] rawData = ReturnAEArray();
            
            foreach (AchievementEntry entry in rawData)
            {
                if (entry.achievementID.Equals(prevAID))
                {
                    sqlQuery = "UPDATE achievement SET achievement_name = '" +
                        newAName + "' WHERE achievement_id = '" + prevAID + "';";

                    AchievementManager.Instance.UpdateAName(prevAID, newAName);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateAUText(string prevAID, string newAUText)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            AchievementEntry[] rawData = ReturnAEArray();

            foreach (AchievementEntry entry in rawData)
            {
                if (entry.achievementID.Equals(prevAID))
                {
                    sqlQuery = "UPDATE achievement SET achievement_unlocked_text = '" +
                        newAUText + "' WHERE achievement_id = '" + prevAID + "';";

                    AchievementManager.Instance.UpdateAUText(prevAID, newAUText);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public AchievementEntry[] ReturnAEArray()
    {
        AchievementEntry[] RawData;
        RawData = AchievementManager.Instance.Struct.list.ToArray();
        return RawData;
    }
}
