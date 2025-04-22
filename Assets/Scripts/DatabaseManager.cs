using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite; 
using UnityEngine;
using System;
using UnityEngine.UI;

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

        command.CommandText = "CREATE TABLE IF NOT EXISTS Questions (id INTEGER PRIMARY KEY AUTOINCREMENT, question TEXT, correctAnswer TEXT, wrongAnswer TEXT, correctChoice INT);";
        command.ExecuteNonQuery();

        command.CommandText = "CREATE TABLE IF NOT EXISTS Answers (id INTEGER PRIMARY KEY AUTOINCREMENT, answer TEXT NOT NULL,  questionID INTEGER, FOREIGN KEY (questionID) REFERENCES Questions(id));";
        command.ExecuteNonQuery();

        InsertQuestionsFromJSON();

    }

    private void InsertQuestionsFromJSON()
    {
        // Check if any rows exist in the Questions table
        using (var checkCommand = connection.CreateCommand())
        {
            checkCommand.CommandText = "SELECT COUNT(*) FROM Questions;";
            long count = (long)checkCommand.ExecuteScalar();

            if (count > 0)
            {
                Debug.Log("Questions already inserted. Skipping import.");
                return;
            }
        }

        TextAsset jsonFile = Resources.Load<TextAsset>("Questions");
        string wrappedJson = "{\"questions\":" + jsonFile.text + "}";
        QuestionList questionList = JsonUtility.FromJson<QuestionList>(wrappedJson);

            foreach (var q in questionList.questions)
            {
              
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Questions (question, correctAnswer, wrongAnswer, correctChoice) VALUES (@question, @correct, @wrong, @choice);";
                    command.Parameters.AddWithValue("@question", q.question);
                    command.Parameters.AddWithValue("@correct", q.correctAnswer);
                    command.Parameters.AddWithValue("@wrong", q.wrongAnswer);
                    command.Parameters.AddWithValue("@choice", q.correctChoice);
                    command.ExecuteNonQuery();

                    
                    command.CommandText = "SELECT last_insert_rowid();";
                    long questionID = (long)command.ExecuteScalar();

                    // Insert each of the 4 answers
                    for (int i = 0; i < q.answers.Length; i++)
                    {
                        using (var answerCommand = connection.CreateCommand())
                        {
                            answerCommand.CommandText = "INSERT INTO Answers (answer, questionID) VALUES (@answer, @questionID);";
                            answerCommand.Parameters.AddWithValue("@answer", q.answers[i]);
                            answerCommand.Parameters.AddWithValue("@questionID", questionID);
                            answerCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
       
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

    public List<Slide> GetAllSlides()
    {
        List<Slide> slides = new List<Slide>();
        using var command = connection.CreateCommand();

        command.CommandText = @"
        SELECT 
        q.question, 
        q.correctAnswer, 
        q.wrongAnswer, 
        q.correctChoice,
        a1.answer AS answer1, 
        a2.answer AS answer2, 
        a3.answer AS answer3, 
        a4.answer AS answer4
        FROM Questions q
        JOIN Answers a1 ON a1.questionID = q.id AND a1.id = (
            SELECT id FROM Answers WHERE questionID = q.id ORDER BY id LIMIT 1 OFFSET 0
        )
        JOIN Answers a2 ON a2.questionID = q.id AND a2.id = (
            SELECT id FROM Answers WHERE questionID = q.id ORDER BY id LIMIT 1 OFFSET 1
        )
        JOIN Answers a3 ON a3.questionID = q.id AND a3.id = (
            SELECT id FROM Answers WHERE questionID = q.id ORDER BY id LIMIT 1 OFFSET 2
        )
        JOIN Answers a4 ON a4.questionID = q.id AND a4.id = (
            SELECT id FROM Answers WHERE questionID = q.id ORDER BY id LIMIT 1 OFFSET 3
        )
        ORDER BY q.id;
        ";


        using var reader = command.ExecuteReader();
        int i = 1;
        while (reader.Read())
        {   
            string text = reader.GetString(0);
            string correctFeedback = reader.GetString(1);
            string wrongFeedback = reader.GetString(2);
            int correctChoice = reader.GetInt32(3);

            string answerA = reader.GetString(4);
            string answerB = reader.GetString(5);
            string answerC = reader.GetString(6);
            string answerD = reader.GetString(7);

            Sprite image = Resources.Load<Sprite>("Image" + i);

            slides.Add(new Slide(image, text, answerA, answerB, answerC, answerD, correctChoice, correctFeedback, wrongFeedback));
            i++;
        }

        // Populate all slide data into slides
        return slides;
    }

    public void DeleteUser(string username)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Users WHERE username = @username;";

        var usernameParam = command.CreateParameter();
        usernameParam.ParameterName = "@username";
        usernameParam.Value = username;
        command.Parameters.Add(usernameParam);

        try
        {
            int rowsAffected = command.ExecuteNonQuery();

        }
        catch (SqliteException ex)
        {
            Debug.LogError($"SQLite error during user deletion: {ex.Message}");
        }
    }

    void OnDestroy()
    {
        if (connection != null)
        {
            connection.Close();
        }

        if (Instance == this)
            Instance = null;
    }
}
    



