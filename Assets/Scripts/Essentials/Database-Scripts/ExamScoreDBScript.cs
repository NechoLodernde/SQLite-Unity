using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class ExamScoreDBScript : MonoBehaviour
{
    public static ExamScoreDBScript Instance { get; private set; }

    private readonly string dbName = "/ExamScore.s3db";
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

            sqlQuery = "CREATE TABLE IF NOT EXISTS examscore (" +
                "[id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "[exam_score_code] VARCHAR(100) NOT NULL, " +
                "[exam_score_term] VARCHAR(25) NOT NULL, " +
                "[exam_score_point] INTEGER NOT NULL, " +
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

            sqlQuery = "DELETE FROM examscore;";

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
            dbConnect.Close();
        }
    }

    public void AddScore(string eSCode, string eSTerm,
        int eSPoint, string sCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "INSERT INTO examscore (exam_score_code, " +
                "exam_score_term, exam_score_point, subject_code) " +
                "VALUES ('" + eSCode + "', '" + eSTerm + "', '" +
                eSPoint + "', '" + sCode + "');";

            ExamScoreManager.Instance.InsertNewData(eSCode,
                eSTerm, eSPoint, sCode);

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
        }

            dbConnect.Close();
    }

    public void DeleteScore(string eSCode, string eSTerm)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            Debug.Log("Input code: " + eSCode);
            Debug.Log("Input term: " + eSTerm);

            sqlQuery = "DELETE FROM examscore WHERE " +
                "exam_score_code='" + eSCode + "' AND " +
                "exam_score_term='" + eSTerm + "';";

            ExamScoreManager.Instance.DeleteData(eSCode, eSTerm);
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
            ExamScoreEntry[] rawData = ReturnESEArray();

            foreach (ExamScoreEntry entry in rawData)
            {
                sqlQuery = "UPDATE examscore SET exam_score_code='" +
                    entry.examScoreCode + "', exam_score_term='" +
                    entry.examScoreTerm + "', exam_score_point='" +
                    entry.examScorePoint + "', subject_code='" +
                    entry.subjectCode + "' WHERE exam_score_code='" +
                    entry.examScoreCode + "' AND exam_score_term='" +
                    entry.examScoreTerm + "';";

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
            sqlQuery = "SELECT * FROM examscore;";
            dbCommand.CommandText = sqlQuery;
            dbReader = dbCommand.ExecuteReader();

            while (dbReader.Read())
            {
                ExamScoreEntry newEntry = new();
                string eSCode, eSTerm, sCode;
                int eSPoint;
                eSCode = "" + dbReader["exam_score_code"];
                eSTerm = "" + dbReader["exam_score_term"];
                sCode = "" + dbReader["subject_code"];

                int.TryParse(("" + dbReader["exam_score_point"]),
                    out int x);
                eSPoint = x;

                Debug.Log("The Data:");
                Debug.Log("Exam Score Code: " + eSCode);
                Debug.Log("Exam Score Term: " + eSTerm);
                Debug.Log("Exam Score Point: " + eSPoint);
                Debug.Log("Subject Code: " + sCode);

                newEntry.examScoreCode = eSCode;
                newEntry.examScoreTerm = eSTerm;
                newEntry.examScorePoint = eSPoint;
                newEntry.subjectCode = sCode;

                ExamScoreManager.Instance.Struct.list.Add(newEntry);
            }

            dbReader.Close();
        }

        dbConnect.Close();
    }

    public void UpdateESCode(string prevESCode, string prevESTerm,
        string newESCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            ExamScoreEntry[] rawData = ReturnESEArray();

            foreach (ExamScoreEntry entry in rawData)
            {
                if (entry.examScoreCode.Equals(prevESCode) &&
                    entry.examScoreTerm.Equals(prevESTerm))
                {
                    sqlQuery = "UPDATE examscore SET exam_score_code='" +
                        newESCode + "' WHERE exam_score_code='" + prevESCode +
                        "' AND exam_score_term='" + prevESTerm + "';";

                    ExamScoreManager.Instance.UpdateESCode(prevESCode,
                        prevESTerm, newESCode);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateESTerm(string prevESCode, string prevESTerm,
        string newESTerm)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            ExamScoreEntry[] rawData = ReturnESEArray();

            foreach (ExamScoreEntry entry in rawData)
            {
                if (entry.examScoreCode.Equals(prevESCode) &&
                    entry.examScoreTerm.Equals(prevESTerm))
                {
                    sqlQuery = "UPDATE examscore SET exam_score_term='" +
                        newESTerm + "' WHERE exam_score_code='" + prevESCode +
                        "' AND exam_Score_term='" + prevESTerm + "';";

                    ExamScoreManager.Instance.UpdateESTerm(prevESCode,
                        prevESTerm, newESTerm);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateESPoint(string prevESCode, string prevESTerm,
        int newESPoint)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            ExamScoreEntry[] rawData = ReturnESEArray();

            foreach (ExamScoreEntry entry in rawData)
            {
                if (entry.examScoreCode.Equals(prevESCode) &&
                    entry.examScoreTerm.Equals(prevESTerm))
                {
                    sqlQuery = "UPDATE examscore SET exam_score_point='" +
                        newESPoint + "' WHERE exam_score_code='" + prevESCode +
                        "' AND exam_score_term='" + prevESTerm + "';";

                    ExamScoreManager.Instance.UpdateESPoint(prevESCode,
                        prevESTerm, newESPoint);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateSCode(string prevESCode, string prevESTerm,
        string newSCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            ExamScoreEntry[] rawData = ReturnESEArray();

            foreach (ExamScoreEntry entry in rawData)
            {
                if (entry.examScoreCode.Equals(prevESCode) &&
                    entry.examScoreTerm.Equals(prevESTerm))
                {
                    sqlQuery = "UPDATE examscore SET subject_code='" +
                        newSCode + "' WHERE exam_score_code='" + prevESCode +
                        "' AND exam_score_term='" + prevESTerm + "';";

                    ExamScoreManager.Instance.UpdateSCode(prevESCode,
                        prevESTerm, newSCode);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public ExamScoreEntry[] ReturnESEArray()
    {
        ExamScoreEntry[] RawData;
        RawData = ExamScoreManager.Instance.Struct.list.ToArray();
        return RawData;
    }
}
