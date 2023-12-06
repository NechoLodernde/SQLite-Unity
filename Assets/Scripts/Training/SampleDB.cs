using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using IDbCommand = System.Data.IDbCommand;
using IDbConnection = System.Data.IDbConnection;
using System.Security.Cryptography;
//using sRandom = System.Random;
using System;

// This is a test script from tutorial on SQLite
public class SampleDB : MonoBehaviour
{
    // The name of Database, in this case the name is "Players"
    private readonly string dbName = "/Players.db";
    private readonly string filepath = Application.dataPath + "/StreamingAssets/Database";
    string conn;

    // Start is called before the first frame update
    private void Start()
    {
        BetterRandomString();
        // Run the method to create DB and Table
        //CreateDB();
        
        // Method to add a player info
        // **Note: This will add a record for this each time the script runs,
        // so typically you wouldn't have it here but instead triggered by a button or event,
        // otherwise you will have a ton of repeated info
        //AddPlayer("Abraham Lincoln", "Male");

        // Method to display the records to the console
        //DisplayInfo();
    }

    public static void BetterRandomString()
    {
        // create a stronger hash code using RNGCryptoServiceProvider
        byte[] random = new byte[64];
        byte[] randID = new byte[32];
        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        // populate with random bytes
        rng.GetBytes(random);
        rng.GetBytes(randID);

        // convert random bytes to string
        string randomBase64 = Convert.ToBase64String(random);
        string randomBase32 = Convert.ToBase64String(randID);
        // display
        Debug.Log("Random string64: " + randomBase64);
        Debug.Log("Random string32: " + randomBase32);
        Debug.Log("Length of string64: " + randomBase64.Length);
        Debug.Log("Length of string32: " + randomBase32.Length);
    }

    // Method to Create a table if it doesn't exist already
    public void CreateDB()
    {
        string filePath = filepath + dbName;
        conn = "URI=file:" + filePath;
        // Create the db connection
        using (var connection = new SqliteConnection(conn))
        {
            connection.Open();

            // Set up an object (called "command") to allow database control
            using (var command = connection.CreateCommand())
            {
                // Create a table called 'players' if it doesn't exist already
                // It has 4 fields: id (an integer) name (up to 20 characters),
                // level (an integer), and gender (up to 6 characters)
                command.CommandText = "CREATE TABLE IF NOT EXISTS players (" + 
                    "[id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " + 
                    "[name] VARCHAR(40) NOT NULL, " + 
                    "[level] INTEGER DEFAULT '1' NOT NULL, " + 
                    "[gender] VARCHAR(6) NOT NULL);";
                command.ExecuteNonQuery(); // This runs the SQL command
            }

            connection.Close();
        }
    }

    // This method will add a player. The parameters accept 
    public void AddPlayer(string playerName, string playerGender)
    {
        string filePath = filepath + dbName;
        conn = "URI=file:" + filePath;

        using (var connection = new SqliteConnection(conn))
        {
            connection.Open();

            // Set up an object (called "command") to allow database control
            using (var command = connection.CreateCommand())
            {
                // Write the SQL command to insert a record with the values passed in to this method
                // Note that we had tot concatenate to get the parameter values into the syntax
                // Statement syntax: "INSERT INTO tablename (field1, field2, etc) VALUES ('value1', 'value2', etc);"
                command.CommandText = 
                    "INSERT INTO players (name, gender) VALUES ('"+ playerName + "', '" + playerGender + "');";
                command.ExecuteNonQuery(); // This runs the SQL command
            }

            connection.Close();
        }
    }

    // This method selects what data you want retrieved
    // Then loops through to display
    public void DisplayInfo()
    {
        string filePath = filepath + dbName;
        conn = "URI=file:" + filePath;

        using (var connection = new SqliteConnection(conn))
        {
            connection.Open();

            // Set up an object to allow database control
            using (var command = connection.CreateCommand())
            {
                // Select what you want to get
                // This just sets the parameters of what will be returned
                command.CommandText = "SELECT * FROM players;";

                // Iterate through the recordset that was returned from the statement above
                // Just type this line in - just change what's inside the while statement
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Show to console what is in field 'name', 'level', dan 'gender' for each row
                        // for reader["xxxxxx"] - replace the xxxxxx with the field name you want to show
                        // this will display as many times as there are records returned
                        Debug.Log("Name: " + reader["name"] + "\nLevel: " + reader["level"]
                            + "\nGender: " + reader["gender"] + "\n");

                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
    }

    private void GenerateRandomID()
    {

    }
}
