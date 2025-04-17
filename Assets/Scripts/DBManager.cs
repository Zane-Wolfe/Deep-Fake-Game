using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using Mono.Data.Sqlite; 
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    private string dbPath;
    private SqliteConnection connection;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        dbPath = "URI=file:" + Application.dataPath + "/game.db";
        connection = new SqliteConnection(dbPath);
        connection.Open();
    }

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
        using var command = connection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS Users (id SERIAL PRIMARY KEY, username TEXT, password TEXT, score INT);";
        command.ExecuteNonQuery();

        command.CommandText = "CREATE TABLE IF NOT EXISTS Questions (id SERIAL PRIMARY KEY, question TEXT, feedback TEXT);";
        command.ExecuteNonQuery();
    }

    public static void AddUser(string username, string password)
    {

    }

    public static void UpdateUserScore(string username, string password, int newScore)
    {

    }


    void OnDestroy()
    {
        connection.Close();
    }


    
}

