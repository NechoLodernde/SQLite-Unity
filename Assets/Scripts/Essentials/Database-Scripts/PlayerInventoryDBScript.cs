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
    string conn, refConn, refSqlQuery, sqlQuery;
    IDbConnection dbConnect;
    IDbConnection dbRefConnect;
    IDbCommand dbCommand;
    IDbCommand dbRefCommand;
    IDataReader dbReader;

    private void Awake()
    {
        string filePath = filepath + dbName;
        conn = "URI=file:" + filePath;
        string refFilePath = refFilepath + refName;
        refConn = "URI=file:" + refFilePath;

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
                "[inventory_data_code] VARCHAR(100) NOT NULL UNIQUE, " +
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
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();

            sqlQuery = "SELECT COUNT(inventory_data_code) from playerinventory;";

            dbCommand.CommandText = sqlQuery;
            string rawData = dbCommand.ExecuteScalar().ToString();
            int.TryParse(rawData, out int y);
            int countData = y;
            dbConnect.Close();
            return countData;
        }
    }

    public void AddInventory(string inventoryCode)
    {
        using (dbRefConnect = new SqliteConnection(refConn))
        {
            dbRefConnect.Open();
            dbRefCommand = dbRefConnect.CreateCommand();
            refSqlQuery = "SELECT * FROM inventorydata;";
            dbRefCommand.CommandText = refSqlQuery;
            dbReader = dbRefCommand.ExecuteReader();
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
                    using (dbConnect = new SqliteConnection(conn))
                    {
                        dbConnect.Open();
                        dbCommand = dbConnect.CreateCommand();
                        sqlQuery = "INSERT INTO playerinventory (inventory_data_code, " +
                        "player_inventory_number, player_inventory_name, " +
                        "inventory_usable_code) VALUES ('" + invID + "', '" +
                        totalInventory + "', '" + invName + "', '" + invUse + "');";

                        PlayerInventoryManager.PlayerInventoryInstance.InsertNewData(
                            invID, totalInventory, invName, invUse);

                        dbCommand.CommandText = sqlQuery;
                        dbCommand.ExecuteScalar();
                    }
                    dbConnect.Close();
                }
                
            }
            dbReader.Close();
        }
        dbRefConnect.Close();
    }

    public void DeleteInventory(string prevInvID)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            Debug.Log("Input id: " + prevInvID);
            sqlQuery = "DELETE FROM playerinventory WHERE" +
                " inventory_data_code='" + prevInvID + "';";
            PlayerInventoryManager.PlayerInventoryInstance.DeleteData(prevInvID);
            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
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
