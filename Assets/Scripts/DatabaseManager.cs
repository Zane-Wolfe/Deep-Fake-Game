using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using Mono.Data.Sqlite; 
using UnityEngine;
using UnityEditor.MemoryProfiler;
using System;

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
        using var command = connection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS Users (id SERIAL PRIMARY KEY, username TEXT, password TEXT, score INT);";
        command.ExecuteNonQuery();

        command.CommandText = "CREATE TABLE IF NOT EXISTS Questions (id SERIAL PRIMARY KEY, question TEXT, feedback TEXT);";
        command.ExecuteNonQuery();
    }

    public void CreateUser(string username, string password)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Users (username, password, score) VALUES (@username, @password, 0);";

        var usernameParam = command.CreateParameter();
        usernameParam.ParameterName = "@username";
        usernameParam.Value = username;
        command.Parameters.Add(usernameParam);

        var passwordParam = command.CreateParameter();
        passwordParam.ParameterName = "@password";
        passwordParam.Value = password;
        command.Parameters.Add(passwordParam);

        try
        {
            command.ExecuteNonQuery();
            Debug.Log("User created successfully.");
        }
        catch (SqliteException ex)
        {
            Debug.LogError($"SQLite error: {ex.Message}");
        }
    }

    public bool UserExists(string username, string password)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Users WHERE username = @username AND password = @password;";

        var usernameParam = command.CreateParameter();
        usernameParam.ParameterName = "@username";
        usernameParam.Value = username;
        command.Parameters.Add(usernameParam);

        var passwordParam = command.CreateParameter();
        passwordParam.ParameterName = "@password";
        passwordParam.Value = password;
        command.Parameters.Add(passwordParam);

        var result = command.ExecuteScalar();
        int count = Convert.ToInt32(result);
        return count > 0;
    }

    public void UpdateUserScore(string username, string password, int newScore)
    {

    }


    void OnDestroy()
    {
        connection.Close();
    }


    
}

