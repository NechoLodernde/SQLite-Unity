using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class ImageClipsDBScript : MonoBehaviour
{
    public static ImageClipsDBScript Instance { get; private set; }

    private readonly string dbName = "/ImageClips.s3db";
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

            sqlQuery = "CREATE TABLE IF NOT EXISTS imageclips(" +
                "[id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "[image_clips_code] VARCHAR(100) NOT NULL UNIQUE, " +
                "[image_clips_name] VARCHAR(256) NOT NULL, " +
                "[image_clips_path] VARCHAR(256) NOT NULL);";

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

            sqlQuery = "DELETE FROM imageclips;";

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
            dbConnect.Close();
        }
    }

    public void AddClip(string iCCode, string iCName, string iCPath)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "INSERT INTO imageclips (image_clips_code, " +
                "image_clips_name, image_clips_path) VALUES ('" +
                iCCode + "', '" + iCName + "', '" + iCPath + "');";

            ImageClipsManager.Instance.InsertNewData(iCCode, iCName,
                iCPath);

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
        }

        dbConnect.Close();
    }

    public void DeleteClip(string prevICCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            Debug.Log("Input code: " + prevICCode);

            sqlQuery = "DELETE FROM imageclips WHERE " +
                "image_clips_code='" + prevICCode + "';";

            ImageClipsManager.Instance.DeleteData(prevICCode);

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
        }

        dbConnect.Close();
    }

    public void SaveImageClips()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            ImageClipsEntry[] rawData = ReturnICEArray();

            foreach (ImageClipsEntry entry in rawData)
            {
                sqlQuery = "UPDATE imageclips SET image_clips_code='" +
                    entry.imageClipsCode + "', image_clips_name='" +
                    entry.imageClipsName + "', image_clips_path='" +
                    entry.imageClipsPath + "' WHERE image_clips_code='" +
                    entry.imageClipsCode + "';";

                dbCommand.CommandText = sqlQuery;
                dbCommand.ExecuteScalar();
            }

            dbConnect.Close();
        }
    }

    public void LoadImageClips()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            sqlQuery = "SELECT * FROM imageclips;";
            dbCommand.CommandText = sqlQuery;
            dbReader = dbCommand.ExecuteReader();

            while (dbReader.Read())
            {
                ImageClipsEntry newEntry = new();
                string iCCode, iCName, iCPath;
                iCCode = "" + dbReader["image_clips_code"];
                iCName = "" + dbReader["image_clips_name"];
                iCPath = "" + dbReader["image_clips_path"];

                Debug.Log("The Data:");
                Debug.Log("Image Clips Code: " + iCCode);
                Debug.Log("Image Clips Name: " + iCName);
                Debug.Log("Image Clips Path: " + iCPath);
                Debug.Log("End of Data");

                newEntry.imageClipsCode = iCCode;
                newEntry.imageClipsName = iCName;
                newEntry.imageClipsPath = iCPath;

                ImageClipsManager.Instance.Struct.list.Add(newEntry);
            }

            dbReader.Close();
        }

        dbConnect.Close();
    }

    public void UpdateICCode(string prevICCode, string newICCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            ImageClipsEntry[] rawData = ReturnICEArray();

            foreach (ImageClipsEntry entry in rawData)
            {
                if (entry.imageClipsCode.Equals(prevICCode))
                {
                    sqlQuery = "UPDATE imageclips SET image_clips_code='" +
                        newICCode + "' WHERE image_clips_code='" + prevICCode
                        + "';";

                    ImageClipsManager.Instance.UpdateICCode(prevICCode,
                        newICCode);

                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateICName(string prevICCode, string newICName)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            ImageClipsEntry[] rawData = ReturnICEArray();

            foreach (ImageClipsEntry entry in rawData)
            {
                if (entry.imageClipsCode.Equals(prevICCode))
                {
                    sqlQuery = "UPDATE imageclips SET image_clips_name='" +
                        newICName + "' WHERE image_clips_code='" +
                        prevICCode + "';";

                    ImageClipsManager.Instance.UpdateICName(prevICCode,
                        newICName);

                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateICPath(string prevICCode, string newICPath)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            ImageClipsEntry[] rawData = ReturnICEArray();

            foreach (ImageClipsEntry entry in rawData)
            {
                if (entry.imageClipsCode.Equals(prevICCode))
                {
                    sqlQuery = "UPDATE imageclips SET image_clips_path='" +
                        newICPath + "' WHERE image_clips_code='" +
                        prevICCode + "';";

                    ImageClipsManager.Instance.UpdateICPath(prevICCode,
                        newICPath);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public ImageClipsEntry[] ReturnICEArray()
    {
        ImageClipsEntry[] RawData;
        RawData = ImageClipsManager.Instance.Struct.list.ToArray();
        return RawData;
    }
}
