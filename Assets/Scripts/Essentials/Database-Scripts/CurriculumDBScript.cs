using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class CurriculumDBScript : MonoBehaviour
{
    public static CurriculumDBScript CurriculumDBInstance { get; private set; }

    private readonly string dbName = "/Curriculum.s3db";
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

        CurriculumDBInstance = this;

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

            sqlQuery = "CREATE TABLE IF NOT EXISTS curriculum (" +
                "[id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "[subject_code] VARCHAR(32) NOT NULL UNIQUE, " +
                "[subject_name] VARCHAR(128) NOT NULL, " +
                "[subject_faculty] VARCHAR(128) NOT NULL, " +
                "[subject_study_program] VARCHAR(128) NOT NULL, " +
                "[subject_total_credit] INTEGER NOT NULL, " +
                "[subject_duration] INTEGER NOT NULL, " +
                "[subject_prequirement_code] VARCHAR(32) NOT NULL, " +
                "[subject_semester] INTEGER NOT NULL, " +
                "[subject_year] VARCHAR(12) NOT NULL, " +
                "[subject_room_code] VARCHAR(24) NOT NULL);";

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

            sqlQuery = "DELETE FROM curriculum;";

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

            sqlQuery = "SELECT COUNT(subject_code) from curriculum;";

            dbCommand.CommandText = sqlQuery;
            string rawData = dbCommand.ExecuteScalar().ToString();
            int.TryParse(rawData, out int y);
            int countData = y;
            dbConnect.Close();
            return countData;
        }
    }

    public void AddCurriculum(string sCode, string sName, string sFaculty,
        string sStudyProgram, int sTotalCredit, int sDuration,
        string sPrequirementCode, int sSemester, string sYear,
        string sRoomCode)
    {

    }
}
