using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class KHSDBScript : MonoBehaviour
{
    public static KHSDBScript Instance { get; private set; }

    private readonly string dbName = "/KHS.s3db";
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

            sqlQuery = "CREATE TABLE IF NOT EXISTS khs ([id] " +
                "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "[khs_code] VARCHAR(50) NOT NULL, [khs_number] " +
                "INTEGER NOT NULL UNIQUE, [khs_semester] INTEGER " +
                "NOT NULL, [subject_code] VARCHAR(100) NOT NULL, " +
                "[subject_name] VARCHAR(128) NOT NULL, " +
                "[subject_total_credit] INTEGER NOT NULL, " +
                "[khs_grade] VARCHAR(50) NOT NULL, [khs_note] " +
                "VARCHAR(256) NOT NULL);";

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

            sqlQuery = "DELETE FROM khs;";

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

            sqlQuery = "SELECT COUNT(khs_code) from khs;";

            dbCommand.CommandText = sqlQuery;
            string rawData = dbCommand.ExecuteScalar().ToString();
            int.TryParse(rawData, out int x);
            int countData = x;
            dbConnect.Close();
            return countData;
        }
    }

    public void AddKHS(string kCode, int kSemester,
        string sCode, string sName, int sTCredit, string kGrade,
        string kNote)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            int kNumber = CountData();

            sqlQuery = "INSERT INTO khs (khs_code, khs_number, " +
                "khs_semester, subject_code, subject_name, " +
                "subject_total_credit, khs_grade, khs_note) VALUES " +
                "('" + kCode + "', '" + kNumber + "', '" + kSemester +
                "', '" + sCode + "', '" + sName + "', '" + sTCredit +
                "', '" + kGrade + "', '" + kNote + "');";

            KHSManager.Instance.InsertNewData(kCode, kNumber, kSemester,
                sCode, sName, sTCredit, kGrade, kNote);

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
        }

        dbConnect.Close();
    }

    public void DeleteKHS(string prevKCode, int prevKSemester)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            Debug.Log("Input code: " + prevKCode);

            sqlQuery = "DELETE FROM khs WHERE khs_code='" + prevKCode +
                "' AND khs_semester='" + prevKSemester + "';";

            KHSManager.Instance.DeleteData(prevKCode, prevKSemester);
            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
        }

        dbConnect.Close();
    }

    public void SaveKHS()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            KHSEntry[] rawData = ReturnKHSEArray();

            foreach (KHSEntry entry in rawData)
            {
                sqlQuery = "UPDATE khs SET khs_code='" + entry.
                    khsCode + "', khs_number='" + entry.khsNumber + 
                    "', khs_semester='" + entry.khsSemester + "', " +
                    "subject_code='" + entry.subjectCode + "', " +
                    "subject_name='" + entry.subjectName + "', " +
                    "subject_total_credit='" + entry.subjectTotalCredit
                    + "', khs_grade='" + entry.khsGrade + "', " +
                    "khs_note='" + entry.khsNote + "' WHERE " +
                    "khs_code='" + entry.khsCode + "' AND " +
                    "khs_semester='" + entry.khsSemester + "';";

                dbCommand.CommandText = sqlQuery;
                dbCommand.ExecuteScalar();
            }

            dbConnect.Close();
        }
    }

    public void LoadKHS()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "SELECT * FROM khs;";

            dbCommand.CommandText = sqlQuery;
            dbReader = dbCommand.ExecuteReader();

            while (dbReader.Read())
            {
                KHSEntry newEntry = new();
                string kCode, sCode, sName, kGrade, kNote;
                int kNumber, kSemester, sTCredit;

                kCode = "" + dbReader["khs_code"];
                sCode = "" + dbReader["subject_code"];
                sName = "" + dbReader["subject_name"];
                kGrade = "" + dbReader["khs_grade"];
                kNote = "" + dbReader["khs_note"];

                int.TryParse(("" + dbReader["khs_number"]),
                    out int x);
                int.TryParse(("" + dbReader["khs_semester"]),
                    out int y);
                int.TryParse(("" + dbReader["subject_total_credit"]),
                    out int z);
                kNumber = x;
                kSemester = y;
                sTCredit = z;

                Debug.Log("The Data:");
                Debug.Log("KHS Code: " + kCode);
                Debug.Log("KHS Number: " + kNumber);
                Debug.Log("KHS Semester: " + kSemester);
                Debug.Log("Subject Code: " + sCode);
                Debug.Log("Subject Name: " + sName);
                Debug.Log("Subject Total Credit: " + sTCredit);
                Debug.Log("KHS Grade: " + kGrade);
                Debug.Log("KHS Note: " + kNote);
                Debug.Log("End of Data");

                newEntry.khsCode = kCode;
                newEntry.khsNumber = kNumber;
                newEntry.khsSemester = kSemester;
                newEntry.subjectCode = sCode;
                newEntry.subjectName = sName;
                newEntry.subjectTotalCredit = sTCredit;
                newEntry.khsGrade = kGrade;
                newEntry.khsNote = kNote;

                KHSManager.Instance.Struct.list.Add(newEntry);
            }

            dbReader.Close();
        }

        dbConnect.Close();
    }

    public void UpdateKCode(string prevKCode, int prevKSemester,
        string newKCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            KHSEntry[] rawData = ReturnKHSEArray();

            foreach (KHSEntry entry in rawData)
            {
                if (entry.khsCode.Equals(prevKCode) &&
                    entry.khsSemester.Equals(prevKSemester))
                {
                    sqlQuery = "UPDATE khs SET khs_code='" + newKCode +
                        "' WHERE khs_code='" + prevKCode + "' AND " +
                        "khs_semester='" + prevKSemester + "';";

                    KHSManager.Instance.UpdateKCode(prevKCode,
                        prevKSemester, newKCode);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateKNumber(string prevKCode, int prevKSemester,
        int newKNumber)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            KHSEntry[] rawData = ReturnKHSEArray();

            foreach (KHSEntry entry in rawData)
            {
                if (entry.khsCode.Equals(prevKCode) &&
                    entry.khsSemester.Equals(prevKSemester))
                {
                    sqlQuery = "UPDATE khs SET khs_number='" + newKNumber +
                        "' WHERE khs_code='" + prevKCode + "' AND " +
                        "khs_semester='" + prevKSemester + "';";

                    KHSManager.Instance.UpdateKNumber(prevKCode,
                        prevKSemester, newKNumber);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateKSemester(string prevKCode, int prevKSemester,
        int newKSemester)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            KHSEntry[] rawData = ReturnKHSEArray();

            foreach (KHSEntry entry in rawData)
            {
                if (entry.khsCode.Equals(prevKCode) &&
                    entry.khsSemester.Equals(prevKSemester))
                {
                    sqlQuery = "UPDATE khs SET khs_semester ='" + newKSemester +
                        "' WHERE khs_code='" + prevKCode + "' AND " +
                        "khs_semester='" + prevKSemester + "';";

                    KHSManager.Instance.UpdateKSemester(prevKCode,
                        prevKSemester, newKSemester);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateSCode(string prevKCode, int prevKSemester,
        string newSCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            KHSEntry[] rawData = ReturnKHSEArray();

            foreach (KHSEntry entry in rawData)
            {
                if (entry.khsCode.Equals(prevKCode) &&
                    entry.khsSemester.Equals(prevKSemester))
                {
                    sqlQuery = "UPDATE khs SET subject_code ='" + newSCode +
                        "' WHERE khs_code='" + prevKCode + "' AND " +
                        "khs_semester='" + prevKSemester + "';";

                    KHSManager.Instance.UpdateSCode(prevKCode,
                        prevKSemester, newSCode);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateSName(string prevKCode, int prevKSemester,
        string newSName)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            KHSEntry[] rawData = ReturnKHSEArray();

            foreach (KHSEntry entry in rawData)
            {
                if (entry.khsCode.Equals(prevKCode) &&
                    entry.khsSemester.Equals(prevKSemester))
                {
                    sqlQuery = "UPDATE khs SET subject_name='" + newSName +
                        "' WHERE khs_code='" + prevKCode + "' AND " +
                        "khs_semester='" + prevKSemester + "';";

                    KHSManager.Instance.UpdateSName(prevKCode,
                        prevKSemester, newSName);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateSTCredit(string prevKCode, int prevKSemester,
        int newSTCredit)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            KHSEntry[] rawData = ReturnKHSEArray();

            foreach (KHSEntry entry in rawData)
            {
                if (entry.khsCode.Equals(prevKCode) &&
                    entry.khsSemester.Equals(prevKSemester))
                {
                    sqlQuery = "UPDATE khs SET subject_total_credit='" 
                        + newSTCredit + "' WHERE khs_code='" 
                        + prevKCode  + "' AND " + "khs_semester='" 
                        + prevKSemester + "';";

                    KHSManager.Instance.UpdateSTCredit(prevKCode,
                        prevKSemester, newSTCredit);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateKGrade(string prevKCode, int prevKSemester,
        string newKGrade)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            KHSEntry[] rawData = ReturnKHSEArray();

            foreach (KHSEntry entry in rawData)
            {
                if (entry.khsCode.Equals(prevKCode) &&
                    entry.khsSemester.Equals(prevKSemester))
                {
                    sqlQuery = "UPDATE khs SET khs_grade='" + newKGrade +
                        "' WHERE khs_code='" + prevKCode + "' AND " +
                        "khs_semester='" + prevKSemester + "';";

                    KHSManager.Instance.UpdateKGrade(prevKCode,
                        prevKSemester, newKGrade);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateKNote(string prevKCode, int prevKSemester,
        string newKNote)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            KHSEntry[] rawData = ReturnKHSEArray();

            foreach (KHSEntry entry in rawData)
            {
                if (entry.khsCode.Equals(prevKCode) &&
                    entry.khsSemester.Equals(prevKSemester))
                {
                    sqlQuery = "UPDATE khs SET khs_note='" + newKNote+
                        "' WHERE khs_code='" + prevKCode + "' AND " +
                        "khs_semester='" + prevKSemester + "';";

                    KHSManager.Instance.UpdateKNote(prevKCode,
                        prevKSemester, newKNote);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public KHSEntry[] ReturnKHSEArray()
    {
        KHSEntry[] RawData;
        RawData = KHSManager.Instance.Struct.list.ToArray();
        return RawData;
    }
}
