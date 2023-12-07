using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Security.Cryptography;

public class PlayerDBScript : MonoBehaviour
{
    private readonly string dbName = "/PlayerData.s3db";
    private readonly string filepath = Application.dataPath + "/StreamingAssets/Database";
    string conn, sqlQuery;
    IDbConnection dbConn;
    IDbCommand dbCmd;

    private void Start()
    {
        string filePath = filepath + dbName;
        conn = "URI=file:" + filePath;
        dbConn = new SqliteConnection(conn);

        CreateDB();
    }

    private void CreateDB()
    {
        using (dbConn = new SqliteConnection(conn))
        {
            dbConn.Open();
            dbCmd = dbConn.CreateCommand();

            sqlQuery = "CREATE TABLE IF NOT EXISTS playerdata (" +
                "[id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "[playerID] VARCHAR(100) NOT NULL UNIQUE, " +
                "[playerName] VARCHAR(128) NOT NULL UNIQUE, " +
                "[playerGender] VARCHAR(10) NOT NULL, " +
                "[playerFaculty] VARCHAR(48) NOT NULL, " +
                "[playerStudyProgram] VARCHAR(256) NOT NULL, " +
                "[playerSemester] INTEGER DEFAULT '1' NOT NULL, " +
                "[activeQuestCode] VARCHAR(100) NOT NULL UNIQUE);";

            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteScalar();
            dbConn.Close();
        }
    }
}
