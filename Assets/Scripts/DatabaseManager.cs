using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using Mono.Data.Sqlite; 
using UnityEngine;
using UnityEditor.MemoryProfiler;
using System;
using UnityEngine.SocialPlatforms;

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
        command.CommandText = "CREATE TABLE IF NOT EXISTS Users (id INTEGER PRIMARY KEY AUTOINCREMENT, username TEXT, password TEXT, score INT);";
        command.ExecuteNonQuery();

        command.CommandText = "CREATE TABLE IF NOT EXISTS Questions (id INTEGER PRIMARY KEY AUTOINCREMENT, question TEXT, feedback TEXT);";
        command.ExecuteNonQuery();
    }

    public List<UserScore> GetTopScores(int limit = 10)
    {
        List<UserScore> leaderboard = new List<UserScore>();

        using var command = connection.CreateCommand();
        command.CommandText = $"SELECT username, score FROM Users ORDER BY score DESC LIMIT {limit};";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            string username = reader.GetString(0);
            int score = reader.GetInt32(1);
            leaderboard.Add(new UserScore(username, score));
        }

        return leaderboard;
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

    public void UpdateUserScore(int id, int newScore)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "UPDATE Users SET score = @score WHERE id = @id;";

        var scoreParam = command.CreateParameter();
        scoreParam.ParameterName = "@score";
        scoreParam.Value = newScore;
        command.Parameters.Add(scoreParam);

        var idParam = command.CreateParameter();
        idParam.ParameterName = "@id";
        idParam.Value = id;
        command.Parameters.Add(idParam);

        try
        {
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Debug.Log($"Score updated for user ID {id} to {newScore}.");
            }
            else
            {
                Debug.LogWarning($"No user found with ID {id}.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to update score: {ex.Message}");
        }
    }

    public int GetUserScore(int id)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT score FROM Users WHERE id = @id;";

        var idParam = command.CreateParameter();
        idParam.ParameterName = "@id";
        idParam.Value = id;
        command.Parameters.Add(idParam);

        var result = command.ExecuteScalar();
        int score = Convert.ToInt32(result);
        return score;
    }

    public int GetUserID(string username, string password)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT id FROM Users WHERE username = @username AND password = @password;";

        var usernameParam = command.CreateParameter();
        usernameParam.ParameterName = "@username";
        usernameParam.Value = username;
        command.Parameters.Add(usernameParam);

        var passwordParam = command.CreateParameter();
        passwordParam.ParameterName = "@password";
        passwordParam.Value = password;
        command.Parameters.Add(passwordParam);

        var result = command.ExecuteScalar();
        int id = Convert.ToInt32(result);
        return id;
    }


    void OnDestroy()
    {
        connection.Close();
    }


    
}

public class UserScore
{
    public string Username;
    public int Score;

    public UserScore(string username, int score)
    {
        Username = username;
        Score = score;
    }
}

