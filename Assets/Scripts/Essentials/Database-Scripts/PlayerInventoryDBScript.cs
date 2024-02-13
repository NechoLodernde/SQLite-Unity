using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class PlayerInventoryDBScript : MonoBehaviour
{
    public static PlayerInventoryDBScript Instance { get; private set; }

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

                        PlayerInventoryManager.Instance.InsertNewData(
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
            PlayerInventoryManager.Instance.DeleteData(prevInvID);
            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
        }

        dbConnect.Close();
    }

    public void SavePlayerInventories()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            PlayerInventoryEntry[] rawData = ReturnPIEArray();

            foreach (PlayerInventoryEntry entry in rawData)
            {
                sqlQuery = "UPDATE playerinventory SET inventory_data_code='" +
                    entry.inventoryID + "', player_inventory_number='" +
                    entry.inventoryNumber + "', player_inventory_name='" +
                    entry.inventoryName + "', inventory_usable_code='" +
                    entry.inventoryUsableCode + "' WHERE inventory_data_code='" +
                    entry.inventoryID + "';";

                dbCommand.CommandText = sqlQuery;
                dbCommand.ExecuteScalar();
            }

            dbConnect.Close();
        }
    }

    public void LoadPlayerInventories()
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            sqlQuery = "SELECT * FROM playerinventory;";
            dbCommand.CommandText = sqlQuery;
            dbReader = dbCommand.ExecuteReader();

            while (dbReader.Read())
            {
                PlayerInventoryEntry newEntry = new();
                string pIC, pIName;
                int pINumber, iUCode;
                pIC = "" + dbReader["inventory_data_code"];
                pIName = "" + dbReader["player_inventory_name"];

                int.TryParse(("" + dbReader["player_inventory_number"]),
                    out int x);
                pINumber = x;
                int.TryParse(("" + dbReader["inventory_usable_code"]),
                    out int y);
                iUCode = y;

                Debug.Log("The Data:");
                Debug.Log("Player Inventory Code: " + pIC);
                Debug.Log("Player Inventory Name: " + pIName);
                Debug.Log("Player Inventory Number: " + pINumber);
                Debug.Log("Inventory Usable Code: " + iUCode);
                Debug.Log("End of Data");

                newEntry.inventoryID = pIC;
                newEntry.inventoryNumber = pINumber;
                newEntry.inventoryName = pIName;
                newEntry.inventoryUsableCode = iUCode;

                PlayerInventoryManager.Instance.Struct.list.Add(newEntry);
            }

            dbReader.Close();
        }

        dbConnect.Close();
    }

    public void UpdatePID(string prevPID, string newPID)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            PlayerInventoryEntry[] rawData = ReturnPIEArray();

            foreach (PlayerInventoryEntry entry in rawData)
            {
                if (entry.inventoryID.Equals(prevPID))
                {
                    sqlQuery = "UPDATE playerinventory SET inventory_data_code='" +
                        newPID + "' WHERE inventory_data_code='" + prevPID + "';";

                    PlayerInventoryManager.Instance.UpdateInvID(prevPID, newPID);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdatePINumber(string prevPID, int newPIN)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            PlayerInventoryEntry[] rawData = ReturnPIEArray();

            foreach (PlayerInventoryEntry entry in rawData)
            {
                if (entry.inventoryID.Equals(prevPID))
                {
                    sqlQuery = "UPDATE playerinventory set player_inventory_number='" +
                        newPIN + "' WHERE inventory_data_code='" + prevPID + "';";

                    PlayerInventoryManager.Instance.UpdateInvNumber(prevPID, newPIN);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdatePIName(string prevPID, string newPIName)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            PlayerInventoryEntry[] rawData = ReturnPIEArray();

            foreach (PlayerInventoryEntry entry in rawData)
            {
                if (entry.inventoryID.Equals(prevPID))
                {
                    sqlQuery = "UPDATE playerinventory set player_inventory_name='" +
                        newPIName + "' WHERE inventory_data_code='" + prevPID + "';";

                    PlayerInventoryManager.Instance.UpdateInvName(prevPID, newPIName);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public void UpdateIUCode(string prevPID, int newIUCode)
    {
        using (dbConnect = new SqliteConnection(conn))
        {
            dbConnect.Open();
            dbCommand = dbConnect.CreateCommand();
            PlayerInventoryEntry[] rawData = ReturnPIEArray();

            foreach (PlayerInventoryEntry entry in rawData)
            {
                if (entry.inventoryID.Equals(prevPID))
                {
                    sqlQuery = "UPDATE playerinventory set inventory_usable_code='" +
                        newIUCode + "' WHERE inventory_data_code='" + prevPID + "';";

                    PlayerInventoryManager.Instance.UpdateUsableCode(prevPID, newIUCode);
                    dbCommand.CommandText = sqlQuery;
                    dbCommand.ExecuteScalar();
                    break;
                }
            }

            dbConnect.Close();
        }
    }

    public PlayerInventoryEntry[] ReturnPIEArray()
    {
        PlayerInventoryEntry[] RawData;
        RawData = PlayerInventoryManager.Instance.Struct.list.ToArray();
        return RawData;
    }
}
