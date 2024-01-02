using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class PlayerInventoryDBScript : MonoBehaviour
{
    public static PlayerInventoryDBScript PlayerInvDBInstance { get; private set; }

    private readonly string dbName = "/PlayerInventory.s3db";
    private readonly string refName = "/InventoryData.s3db";
    private readonly string filepath = Application.dataPath +
        "/StreamingAssets/Database";
    private readonly string refFilepath = Application.dataPath +
        "/StreamingAssets/Database";
    string conn, sqlQuery;
    IDbConnection dbConnect;
    IDbCommand dbCommand;
    IDataReader dbReader;

    private void Awake()
    {
        string filePath = filepath + dbName;
        conn = "URI=file:" + filePath;

        PlayerInvDBInstance = this;

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

            sqlQuery = "CREATE TABLE IF NOT EXISTS playerinventory (" +
                "[id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "[inventory_data_code] VARCHAR(100) NOT NULL, " +
                "[player_inventory_number] INTEGER, " +
                "[player_inventory_name] VARCHAR(128) NOT NULL UNIQUE, " +
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

            sqlQuery = "DELETE FROM playerinventory;";

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

            sqlQuery = "SELECT COUNT(inventory_data_code) from playerinventory;";

            dbCommand.CommandText = sqlQuery;
            string rawData = dbCommand.ExecuteScalar().ToString();
            int.TryParse(rawData, out int y);
            countData = y;
            dbConnect.Close();
            return countData;
        }
    }

    public void AddInventory(string inventoryCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            sqlQuery = "SELECT * FROM inventorydata;";
            dbCommand.CommandText = sqlQuery;
            dbReader = dbCommand.ExecuteReader();
            int totalInventory = CountData();
            while (dbReader.Read())
            {
                string invID, invName;
                int invUse;
                invID = "" + dbReader["inventory_data_code"];
                if (invID.Equals(inventoryCode))
                {
                    invName = "" + dbReader["inventory_data_name"];
                    int.TryParse(("" + dbReader["inventory_usable_code"]),
                        out int y);
                    invUse = y;
                    totalInventory++;
                    sqlQuery = "INSERT INTO playerinventory (inventory_data_code, " +
                        "player_inventory_number, player_inventory_name, " +
                        "inventory_usable_code) VALUES ('" + invID + "', '" +
                        totalInventory + "', '" + invName + "', '" + invUse + "');";

                    PlayerInventoryManager.PlayerInventoryInstance.InsertNewData(
                        invID, totalInventory, invName, invUse);

                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                }
                
            }
            dbReader.Close();
        }
        dbConnect.Close();
    }

    public void DeleteInventory(string prevInvID)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
        }
        dbConnect.Close();
    }

    public PlayerInventoryEntry[] ReturnPIEArray()
    {
        PlayerInventoryEntry[] RawData;
        RawData = PlayerInventoryManager.PlayerInventoryInstance.inventoryStruct.list.ToArray();
        return RawData;
    }
}
