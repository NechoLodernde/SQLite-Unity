using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class QuizScoreDBScript : MonoBehaviour
{
    public static QuizScoreDBScript Instance { get; private set; }

    private readonly string dbName = "/QuizScore.s3db";
    private readonly string filepath = Application.dataPath +
        "/StreamingAssets/Database";
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

            sqlQuery = "CREATE TABLE IF NOT EXISTS quizscore (" +
                "[id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "[quiz_score_code] VARCHAR(100) NOT NULL, " +
                "[quiz_score_point] INTEGER, " +
                "[subject_code] VARCHAR(100) NOT NULL);";

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

            sqlQuery = "DELETE FROM quizscore;";

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
            dbConnect.Close();
        }
    }

    public void AddScore(string qSCode, int qSPoint, string sCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "INSERT INTO quizscore (quiz_score_code, " +
                "quiz_score_point, subject_code) VALUES ('" +
                qSCode + "', '" + qSPoint + "', '" + sCode + "');";

            QuizScoreManager.Instance.InsertNewData(qSCode,
                qSPoint, sCode);

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
        }

        dbConnect.Close();
    }

    public void DeleteInventory(string prevQSCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            Debug.Log("Input code: " + prevQSCode);

            sqlQuery = "DELETE FROM quizscore WHERE " +
                "quiz_score_code='" + prevQSCode + "';";

            QuizScoreManager.Instance.DeleteData(prevQSCode);
            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
        }

        dbConnect.Close();
    }

    public void SaveScores()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            QuizScoreEntry[] rawData = ReturnQSEArray();

            foreach (QuizScoreEntry entry in rawData)
            {
                sqlQuery = "UPDATE quizscore SET quiz_score_code='" +
                    entry.quizScoreCode + "', quiz_score_point='" +
                    entry.quizScorePoint + "', subject_code='" +
                    entry.subjectCode + "' WHERE quiz_score_code='" +
                    entry.quizScoreCode + "';";

                dbCommand.CommandText = sqlQuery;
                dbCommand.ExecuteScalar();
            }

            dbConnect.Close();
        }
    }

    public void LoadScores()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "SELECT * FROM quizscore;";
            dbCommand.CommandText = sqlQuery;
            dbReader = dbCommand.ExecuteReader();

            while (dbReader.Read())
            {
                QuizScoreEntry newEntry = new();
                string qSCode, sCode;
                int qSPoint;
                qSCode = "" + dbReader["quiz_score_code"];
                sCode = "" + dbReader["subject_code"];

                int.TryParse(("" + dbReader["quiz_score_point"]),
                    out int x);
                qSPoint = x;

                Debug.Log("The Data:");
                Debug.Log("Quiz Score Code: " + qSCode);
                Debug.Log("Quiz Score Point: " + qSPoint);
                Debug.Log("Subject Code: " + sCode);

                newEntry.quizScoreCode = qSCode;
                newEntry.quizScorePoint = qSPoint;
                newEntry.subjectCode = sCode;

                QuizScoreManager.Instance.Struct.list.Add(newEntry);
            }

            dbReader.Close();
        }

        dbConnect.Close();
    }

    public void UpdateQSCode(string prevQSCode, string newQSCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            QuizScoreEntry[] rawData = ReturnQSEArray();

            foreach (QuizScoreEntry entry in rawData)
            {
                if (entry.quizScoreCode.Equals(prevQSCode))
                {
                    sqlQuery = "UPDATE quizscore SET quiz_score_code='" +
                        newQSCode + "' WHERE quiz_score_code='" + prevQSCode + "';";

                    QuizScoreManager.Instance.UpdateQSCode(prevQSCode, newQSCode);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateQSPoint(string prevQSCode, int newQSPoint)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            QuizScoreEntry[] rawData = ReturnQSEArray();

            foreach (QuizScoreEntry entry in rawData)
            {
                if (entry.quizScoreCode.Equals(prevQSCode))
                {
                    sqlQuery = "UPDATE quizscore SET quiz_score_point='" +
                        newQSPoint + "' WHERE quiz_score_code='" + prevQSCode +
                        "';";

                    QuizScoreManager.Instance.UpdateQSPoint(prevQSCode, newQSPoint);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateSCode(string prevQSCode, string newSCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            QuizScoreEntry[] rawData = ReturnQSEArray();

            foreach (QuizScoreEntry entry in rawData)
            {
                if (entry.quizScoreCode.Equals(prevQSCode))
                {
                    sqlQuery = "UPDATE quizscore SET subject_code='" +
                        newSCode + "' WHERE quiz_score_code='" + prevQSCode
                        + "';";

                    QuizScoreManager.Instance.UpdateSCode(prevQSCode, newSCode);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public QuizScoreEntry[] ReturnQSEArray()
    {
        QuizScoreEntry[] RawData;
        RawData = QuizScoreManager.Instance.Struct.list.ToArray();
        return RawData;
    }
}
