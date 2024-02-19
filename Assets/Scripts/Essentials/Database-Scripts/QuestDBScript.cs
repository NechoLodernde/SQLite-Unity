using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class QuestDBScript : MonoBehaviour
{
    public static QuestDBScript Instance {  get; private set; }

    private readonly string dbName = "/Quest.s3db";
    private readonly string filepath = Application.dataPath +
        "StreamingAssets/Database";
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

            sqlQuery = "CREATE TABLE IF NOT EXISTS quest ([id] " +
                "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "[quest_code] VARCHAR(64) NOT NULL UNIQUE, " +
                "[quest_number] INTEGER NOT NULL, [quest_type] " +
                "VARCHAR(64) NOT NULL, [quest_title] VARCHAR(256) " +
                "NOT NULL, [quest_info] VARCHAR(512) NOT NULL, " +
                "[quest_reward] INTEGER NOT NULL, [completion_code] " +
                "VARCHAR(64) NOT NULL);";

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

            sqlQuery = "DELETE FROM quest;";

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

            sqlQuery = "SELECT COUNT(quest_code) from quest;";

            dbCommand.CommandText = sqlQuery;
            string rawData = dbCommand.ExecuteScalar().ToString();
            int.TryParse(rawData, out int x);
            int countData = x;
            dbConnect.Close();
            return countData;
        }
    }

    public void AddQuest(string qCode, int qNumber, string qType,
        string qTitle, string qInfo, int qReward, string cCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "INSERT INTO quest (quest_code, quest_number, " +
                "quest_type, quest_title, quest_info, quest_reward, " +
                "completion_code) VALUES ('" + qCode + "', '" + qNumber +
                "', '" + qType + "', '" + qTitle + "', '" + qInfo +
                "', '" + qReward + "', '" + cCode + "');";

            QuestManager.Instance.InsertNewData(qCode, qNumber, qType,
                qTitle, qInfo, qReward, cCode);

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
        }

        dbConnect.Close();
    }

    public void DeleteQuest(string prevQCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            Debug.Log("Input Code: " + prevQCode);

            sqlQuery = "DELETE FROM quest WHERE quest_code='" +
                prevQCode + "';";

            QuestManager.Instance.DeleteData(prevQCode);
            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
        }

        dbConnect.Close();
    }

    public void SaveQuests()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            QuestEntry[] rawData = ReturnQEArray();

            foreach (QuestEntry e in rawData)
            {
                sqlQuery = "UPDATE quest SET quest_code='" + e.questCode +
                    "', quest_number='" + e.questNumber + "', quest_type='" +
                    e.questType + "', quest_title='" + e.questTitle + "', " +
                    "quest_info='" + e.questInfo + "', quest_reward='" +
                    e.questReward + "', completion_code='" + e.completionCode +
                    "';";

                dbCommand.CommandText = sqlQuery;
                dbCommand.ExecuteScalar();
            }
        }

        dbConnect.Close();
    }

    public void LoadQuests()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "SELECT * FROM quest;";

            dbCommand.CommandText = sqlQuery;
            dbReader = dbCommand.ExecuteReader();

            while(dbReader.Read())
            {
                QuestEntry newEntry = new();
                string qCode, qType, qTitle, qInfo, cCode;
                int qNumber, qReward;

                qCode = "" + dbReader["quest_code"];
                qType = "" + dbReader["quest_type"];
                qTitle = "" + dbReader["quest_title"];
                qInfo = "" + dbReader["quest_info"];
                cCode = "" + dbReader["completion_code"];

                int.TryParse(("" + dbReader["quest_number"]),
                    out int x);
                int.TryParse(("" + dbReader["quest_reward"]),
                    out int y);
                qNumber = x;
                qReward = y;

                Debug.Log("The Data:");
                Debug.Log("Quest Code: " + qCode);
                Debug.Log("Quest Number: " + qNumber);
                Debug.Log("Quest Type: " + qType);
                Debug.Log("Quest Title: " + qTitle);
                Debug.Log("Quest Info: " + qInfo);
                Debug.Log("Quest Reward: " + qReward);
                Debug.Log("Completion Code: " + cCode);
                Debug.Log("End of Data");

                newEntry.questCode = qCode;
                newEntry.questNumber = qNumber;
                newEntry.questType = qType;
                newEntry.questTitle = qTitle;
                newEntry.questInfo = qInfo;
                newEntry.questReward = qReward;
                newEntry.completionCode = cCode;

                QuestManager.Instance.Struct.list.Add(newEntry);
            }

            dbReader.Close();
        }

        dbConnect.Close();
    }

    public void UpdateQCode(string prevQCode, string newQCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            QuestEntry[] rawData = ReturnQEArray();

            foreach (QuestEntry e in rawData)
            {
                if (e.questCode.Equals(prevQCode))
                {
                    sqlQuery = "UPDATE quest SET quest_code='" + newQCode +
                        "' WHERE quest_code='" + prevQCode + "';";

                    QuestManager.Instance.UpdateQCode(prevQCode, newQCode);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateQNumber(string prevQCode, int newQNumber)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            QuestEntry[] rawData = ReturnQEArray();

            foreach (QuestEntry e in rawData)
            {
                if (e.questCode.Equals(prevQCode))
                {
                    sqlQuery = "UPDATE quest SET quest_number='" + newQNumber +
                        "' WHERE quest_code='" + prevQCode + "';";

                    QuestManager.Instance.UpdateQNumber(prevQCode, newQNumber);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateQType(string prevQCode, string newQType)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            QuestEntry[] rawData = ReturnQEArray();

            foreach (QuestEntry e in rawData)
            {
                if (e.questCode.Equals(prevQCode))
                {
                    sqlQuery = "UPDATE quest SET quest_type='" + newQType +
                        "' WHERE quest_code='" + prevQCode + "';";

                    QuestManager.Instance.UpdateQType(prevQCode, newQType);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateQTitle(string prevQCode, string newQTitle)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            QuestEntry[] rawData = ReturnQEArray();

            foreach (QuestEntry e in rawData)
            {
                if (e.questCode.Equals(prevQCode))
                {
                    sqlQuery = "UPDATE quest SET quest_title='" + newQTitle +
                        "' WHERE quest_code='" + prevQCode + "';";

                    QuestManager.Instance.UpdateQTitle(prevQCode, newQTitle);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar(); 
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateQInfo(string prevQCode, string newQInfo)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            QuestEntry[] rawData = ReturnQEArray();

            foreach (QuestEntry e in rawData)
            {
                if (e.questCode.Equals(prevQCode))
                {
                    sqlQuery = "UPDATE quest SET quest_info='" + newQInfo +
                        "' WHERE quest_code='" + prevQCode + "';";

                    QuestManager.Instance.UpdateQInfo(prevQCode, newQInfo);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateQReward(string prevQCode, int newQReward)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            QuestEntry[] rawData = ReturnQEArray();

            foreach (QuestEntry e in rawData)
            {
                if (e.questCode.Equals(prevQCode))
                {
                    sqlQuery = "UPDATE quest SET quest_reward='" + newQReward +
                        "' WHERE quest_code='" + prevQCode + "';";

                    QuestManager.Instance.UpdateQReward(prevQCode, newQReward);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateCCode(string prevQCode, string newCCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            QuestEntry[] rawData = ReturnQEArray();

            foreach (QuestEntry e in rawData)
            {
                if (e.questCode.Equals(prevQCode))
                {
                    sqlQuery = "UPDATE quest SET completion_code='" + newCCode +
                        "' WHERE quest_code='" + prevQCode + "';";

                    QuestManager.Instance.UpdateCCode(prevQCode, newCCode);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public QuestEntry[] ReturnQEArray()
    {
        QuestEntry[] RawData;
        RawData = QuestManager.Instance.Struct.list.ToArray();
        return RawData;
    }
}
