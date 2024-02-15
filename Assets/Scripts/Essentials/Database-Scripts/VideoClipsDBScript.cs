using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class VideoClipsDBScript : MonoBehaviour
{
    public static VideoClipsDBScript Instance { get; private set; }

    private readonly string dbName = "/VideoClips.s3db";
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

            sqlQuery = "CREATE TABLE IF NOT EXISTS videoclips (" +
                "[id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "[video_clips_code] VARCHAR(100) NOT NULL UNIQUE, " +
                "[video_clips_name] VARCHAR(256) NOT NULL, " +
                "[video_clips_path] VARCHAR(256) NOT NULL);";

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

            sqlQuery = "DELETE FROM videoclips;";

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
            dbConnect.Close();
        }
    }

    public void AddClip(string vCCode, string vCName, string vCPath)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "INSERT INTO videoclips (video_clips_code, " +
                "video_clips_name, video_clips_path) VALUES ('" +
                vCCode + "', '" + vCName + "', '" + vCPath + "');";

            VideoClipsManager.Instance.InsertNewData(vCCode,
                vCName, vCPath);

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
        }

        dbConnect.Close();
    }

    public void DeleteClip(string prevVCCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            Debug.Log("Input id: " + prevVCCode);

            sqlQuery = "DELETE FROM videoclips WHERE " +
                "video_clips_code='" + prevVCCode + "';";

            VideoClipsManager.Instance.DeleteData(prevVCCode);
            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
        }

        dbConnect.Close();
    }

    public void SaveVideoClips()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            VideoClipsEntry[] rawData = ReturnVCEArray();

            foreach (VideoClipsEntry entry in rawData)
            {
                sqlQuery = "UPDATE videoclips SET video_clips_code='" +
                    entry.videoClipsCode + "', video_clips_name='" +
                    entry.videoClipsName + "', video_clips_path='" +
                    entry.videoClipsPath + "' WHERE video_clips_code='" +
                    entry.videoClipsCode + "';";

                dbCommand.CommandText = sqlQuery;
                dbCommand.ExecuteScalar();
            }

            dbConnect.Close();
        }
    }

    public void LoadVideoClips()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "SELECT * FROM videoclips;";

            dbCommand.CommandText = sqlQuery;
            dbReader = dbCommand.ExecuteReader();

            while (dbReader.Read())
            {
                VideoClipsEntry newEntry = new();
                string vCCode, vCName, vCPath;
                vCCode = "" + dbReader["video_clips_code"];
                vCName = "" + dbReader["video_clips_name"];
                vCPath = "" + dbReader["video_clips_path"];

                Debug.Log("The Data:");
                Debug.Log("Video Clips Code: " + vCCode);
                Debug.Log("Video Clips Name: " + vCName);
                Debug.Log("Video Clips Path: " + vCPath);
                Debug.Log("End of Data");

                newEntry.videoClipsCode = vCCode;
                newEntry.videoClipsName = vCName;
                newEntry.videoClipsPath = vCPath;

                VideoClipsManager.Instance.Struct.list.Add(newEntry);
            }

            dbReader.Close();
        }

        dbConnect.Close();
    }

    public void UpdateVCCode(string prevVCCode, string newVCCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            VideoClipsEntry[] rawData = ReturnVCEArray();

            foreach (VideoClipsEntry entry in rawData)
            {
                if (entry.videoClipsCode.Equals(prevVCCode))
                {
                    sqlQuery = "UPDATE videoclips SET video_clips_code='" +
                        newVCCode + "' WHERE video_clips_code='" + prevVCCode +
                        "';";

                    VideoClipsManager.Instance.UpdateVCCode(prevVCCode,
                        newVCCode);

                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateVCName(string prevVCCode, string newVCName)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            VideoClipsEntry[] rawData = ReturnVCEArray();

            foreach (VideoClipsEntry entry in rawData)
            {
                if (entry.videoClipsCode.Equals(prevVCCode))
                {
                    sqlQuery = "UPDATE videoclips SET video_clips_name='" +
                        newVCName + "' WHERE video_clips_code='" +
                        prevVCCode + "';";

                    VideoClipsManager.Instance.UpdateVCName(prevVCCode,
                        newVCName);

                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateVCPath(string prevVCCode, string newVCPath)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            VideoClipsEntry[] rawData = ReturnVCEArray();

            foreach (VideoClipsEntry entry in rawData)
            {
                if (entry.videoClipsCode.Equals(prevVCCode))
                {
                    sqlQuery = "UPDATE videoclips SET video_clips_path='" +
                        newVCPath + "' WHERE video_clips_code='" +
                        prevVCCode + "';";

                    VideoClipsManager.Instance.UpdateVCPath(prevVCCode,
                        newVCPath);

                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public VideoClipsEntry[] ReturnVCEArray()
    {
        VideoClipsEntry[] RawData;
        RawData = VideoClipsManager.Instance.Struct.list.ToArray();
        return RawData;
    }
}
