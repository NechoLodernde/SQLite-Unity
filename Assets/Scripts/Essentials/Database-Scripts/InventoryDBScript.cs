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

    public void CreateDB()
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

    public void DeleteDB()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "DELETE FROM inventorydata;";

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
            dbConnect.Close();
        }
    }

    public int CountData()
    {
        int countData = 0;
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "SELECT COUNT(inventory_data_code) FROM inventorydata;";

            dbCommand.CommandText = sqlQuery;
            string rawData = dbCommand.ExecuteScalar().ToString();
            int.TryParse(rawData, out int y);
            countData = y;
            dbConnect.Close();
            return countData;
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

            InventoryDataManager.Instance.InsertNewData(randomInvID,
                iName, iType, iWeight, iUse);

            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
            dbConnect.Close();
        }
    }

    public void LoadItemsData()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            sqlQuery = "SELECT * FROM inventorydata;";
            dbCommand.CommandText = sqlQuery;
            dbReader = dbCommand.ExecuteReader();

            while (dbReader.Read())
            {
                string invID, invName, invType;
                double invWeight;
                int invUse;
                invID = "" + dbReader["inventory_data_code"];
                invName = "" + dbReader["inventory_data_name"];
                invType = "" + dbReader["inventory_data_type"];

                double.TryParse(("" + dbReader["inventory_data_weight"]),
                    out double x);
                invWeight = x;

                int.TryParse(("" + dbReader["inventory_usable_code"]),
                    out int y);
                invUse = y;

                Debug.Log("New Data");
                Debug.Log("Inventory ID: " + invID);
                Debug.Log("Inventory Name: " + invName);
                Debug.Log("Inventory Type: " + invType);
                Debug.Log("Inventory Weight: " + invWeight);
                Debug.Log("Inventory Use Code: " + invUse);
                Debug.Log("End Data");
            }
            dbReader.Close();
        }
        dbConnect.Close();
    }

    public void SaveInventoryData()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            InventoryDataEntry[] rawData = ReturnIDEArray();

            foreach(InventoryDataEntry inv in rawData)
            {
                sqlQuery = "UPDATE inventorydata SET inventory_data_name = '" +
                    inv.inventoryName + "', inventory_data_type = '" +
                    inv.inventoryType + "', inventory_data_weight = '" +
                    inv.inventoryWeight + "', inventory_usable_code = '" +
                    inv.inventoryUsableCode + "' WHERE inventory_data_code = '"
                    + inv.inventoryID + "';";

                dbCommand.CommandText = sqlQuery;
                dbCommand.ExecuteScalar();
            }

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
        RawData = InventoryDataManager.Instance.Struct.list.ToArray();
        return RawData;
    }
}
