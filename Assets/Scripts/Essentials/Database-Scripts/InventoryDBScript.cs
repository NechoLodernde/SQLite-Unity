using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using RNGCrisp = System.Security.Cryptography.RNGCryptoServiceProvider;
using Con64 = System.Convert;

public class InventoryDBScript : MonoBehaviour
{
    public static InventoryDBScript InventoryDBInstance { get; private set; }

    private readonly string dbName = "/InventoryData.s3db";
    private readonly string filepath = Application.dataPath + "/StreamingAssets/Database";
    string conn, sqlQuery;
    IDbConnection dbConnect;
    IDbCommand dbCommand;
    IDataReader dbReader;

    private void Awake()
    {
        string filePath = filepath + dbName;
        conn = "URI=file:" + filePath;

        InventoryDBInstance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        dbConnect = new SqliteConnection(conn);

        CreateDB();
    }

    private void CreateDB()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "CREATE TABLE IF NOT EXISTS inventorydata (" +
                "[id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "[inventory_data_code] VARCHAR(100) NOT NULL UNIQUE, " +
                "[inventory_data_name] VARCHAR(128) NOT NULL UNIQUE, " +
                "[inventory_data_type] VARCHAR(128) NOT NULL, " +
                "[inventory_data_weight] DOUBLE PRECISION(100, 8) NOT NULL, " +
                "[inventory_usable_code] INTEGER DEFAULT '0' NOT NULL);";

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
            dbConnect.Close();
        }
    }

    public void AddItem(string iName, string iType, 
        double iWeight, int iUse)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            string randomInvID = RandomBaseString64();
            sqlQuery = "INSERT INTO inventorydata (inventory_data_code, " +
                "inventory_data_name, inventory_data_type, " +
                "inventory_data_weight, inventory_usable_code) VALUES " +
                "('" + randomInvID + "', '" + iName + "', '" + iType + "', '" +
                iWeight + "', '" + iUse + "');";

            InventoryDataManager.InventoryDataInstance.InsertNewData(randomInvID,
                iName, iType, iWeight, iUse);

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
            dbConnect.Close();
        }
    }

    // Method to create random string with length of 88
    private string RandomBaseString64()
    {
        byte[] random = new byte[64];
        RNGCrisp rng = new RNGCrisp();
        rng.GetBytes(random);

        string randomBase64 = Con64.ToBase64String(random);
        return randomBase64;
    }

    public InventoryDataEntry[] ReturnIDEArray()
    {
        InventoryDataEntry[] RawData;
        RawData = InventoryDataManager.InventoryDataInstance.inventoryStruct.list.ToArray();
        return RawData;
    }
}
