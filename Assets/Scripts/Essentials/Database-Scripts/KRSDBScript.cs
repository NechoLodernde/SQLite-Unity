using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class KRSDBScript : MonoBehaviour
{
    public static KRSDBScript Instance { get; private set; }

    private readonly string dbName = "/KRS.s3db";
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

            sqlQuery = "CREATE TABLE IF NOT EXISTS krs ([id] " +
                "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "[krs_code] VARCHAR(50) NOT NULL, [krs_number] " +
                "INTEGER NOT NULL UNIQUE, [krs_year] VARCHAR(128) " +
                "NOT NULL UNIQUE, [krs_semester] INTEGER NOT NULL, " +
                "[krs_status] VARCHAR(50) NOT NULL);";

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
        }

        dbConnect.Close();
    }

    public void DeleteDB()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "DELETE FROM krs;";

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

            sqlQuery = "SELECT COUNT(krs_code) from krs;";

            dbCommand.CommandText = sqlQuery;
            string rawData = dbCommand.ExecuteScalar().ToString();
            int.TryParse(rawData, out int x);
            int countData = x;
            dbConnect.Close();
            return countData;
        }
    }

    public void AddKHS(string kCode, string kYear, int kSemester, 
        string kStatus)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            int kNumber = CountData();

            sqlQuery = "INSERT INTO krs (krs_code, krs_number, " +
                "krs_year, krs_semester, krs_status) VALUES ('" +
                kCode + "', '" + kNumber + "', '" + kYear + "', '" +
                kSemester + "', '" + kStatus + "');";

            KRSManager.Instance.InsertNewData(kCode, kNumber,
                kYear, kSemester, kStatus);

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
        }

        dbConnect.Close();
    }

    public void DeleteKRS(string prevKCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            Debug.Log("Input Code: " + prevKCode);

            sqlQuery = "DELETE FROM krs WHERE krs_code='" + prevKCode
                + "';";

            KRSManager.Instance.DeleteData(prevKCode);
            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
        }

        dbConnect.Close();
    }

    public void SaveKRS()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            KRSEntry[] rawData = ReturnKRSEArray();

            foreach (KRSEntry entry in rawData)
            {
                sqlQuery = "UPDATE krs SET krs_code='" + entry.
                    krsCode + "', krs_number='" + entry.krsNumber +
                    "', krs_year='" + entry.krsYear + "', krs_semester='" +
                    entry.krsSemester + "', krs_status='" + entry.
                    krsStatus + "' WHERE krs_code='" + entry.
                    krsCode + "';";

                dbCommand.CommandText = sqlQuery;
                dbCommand.ExecuteScalar();
            }
        }

        dbConnect.Close();
    }

    public void LoadKRS()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "SELECT * FROM krs;";

            dbCommand.CommandText = sqlQuery;
            dbReader = dbCommand.ExecuteReader();

            while (dbReader.Read())
            {
                KRSEntry newEntry = new();
                string kCode, kYear, kStatus;
                int kNumber, kSemester;

                kCode = "" + dbReader["krs_code"];
                kYear = "" + dbReader["krs_year"];
                kStatus = "" + dbReader["krs_status"];

                int.TryParse(("" + dbReader["krs_number"]),
                    out int x);
                int.TryParse(("" + dbReader["krs_semester"]),
                    out int y);
                kNumber = x;
                kSemester = y;

                Debug.Log("The Data:");
                Debug.Log("KRS Code: " + kCode);
                Debug.Log("KRS Number: " + kNumber);
                Debug.Log("KRS Year: " + kYear);
                Debug.Log("KRS Semester: " + kSemester);
                Debug.Log("KRS Status: " + kStatus);
                Debug.Log("End of Data");

                newEntry.krsCode = kCode;
                newEntry.krsNumber = kNumber;
                newEntry.krsYear = kYear;
                newEntry.krsSemester = kSemester;
                newEntry.krsStatus = kStatus;

                KRSManager.Instance.Struct.list.Add(newEntry);
            }

            dbReader.Close();
        }

        dbConnect.Close();
    }

    public void UpdateKCode(string prevKCode, string newKCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            KRSEntry[] rawData = ReturnKRSEArray();

            foreach (KRSEntry entry in rawData)
            {
                if (entry.krsCode.Equals(prevKCode))
                {
                    sqlQuery = "UPDATE krs SET krs_code'" + newKCode
                        + "' WHERE krs_code='" + prevKCode + "';";

                    KRSManager.Instance.UpdateKCode(prevKCode,
                        newKCode);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateKNumber(string prevKCode, int newKNumber)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            KRSEntry[] rawData = ReturnKRSEArray();

            foreach (KRSEntry entry in rawData)
            {
                if (entry.krsCode.Equals(prevKCode))
                {
                    sqlQuery = "UPDATE krs SET krs_number'" + newKNumber
                        + "' WHERE krs_code='" + prevKCode + "';";

                    KRSManager.Instance.UpdateKNumber(prevKCode,
                        newKNumber);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateKYear(string prevKCode, string newKYear)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            KRSEntry[] rawData = ReturnKRSEArray();

            foreach (KRSEntry entry in rawData)
            {
                if (entry.krsCode.Equals(prevKCode))
                {
                    sqlQuery = "UPDATE krs SET krs_year'" + newKYear
                        + "' WHERE krs_code='" + prevKCode + "';";

                    KRSManager.Instance.UpdateKYear(prevKCode,
                        newKYear);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateKSemester(string prevKCode, int newKSemester)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            KRSEntry[] rawData = ReturnKRSEArray();

            foreach (KRSEntry entry in rawData)
            {
                if (entry.krsCode.Equals(prevKCode))
                {
                    sqlQuery = "UPDATE krs SET krs_semester'" + newKSemester
                        + "' WHERE krs_code='" + prevKCode + "';";

                    KRSManager.Instance.UpdateKSemester(prevKCode,
                        newKSemester);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateKStatus(string prevKCode, string newKStatus)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            KRSEntry[] rawData = ReturnKRSEArray();

            foreach (KRSEntry entry in rawData)
            {
                if (entry.krsCode.Equals(prevKCode))
                {
                    sqlQuery = "UPDATE krs SET krs_status'" + newKStatus
                        + "' WHERE krs_code='" + prevKCode + "';";

                    KRSManager.Instance.UpdateKStatus(prevKCode,
                        newKStatus);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public KRSEntry[] ReturnKRSEArray()
    {
        KRSEntry[] RawData;
        RawData = KRSManager.Instance.Struct.list.ToArray();
        return RawData;
    }
}
