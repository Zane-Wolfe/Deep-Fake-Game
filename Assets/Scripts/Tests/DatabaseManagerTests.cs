using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DatabaseManagerTests
{
    private DatabaseManager db;
    private readonly List<string> testUsers = new();

    [SetUp]
    public void SetUp()
    {
        db = new GameObject("DatabaseManager").AddComponent<DatabaseManager>();

    }

    [TearDown]
    public void TearDown()
    {
        foreach (var username in testUsers)
        {
            db.DeleteUser(username);
        }

        Object.DestroyImmediate(db.gameObject);
    }

    [UnityTest]
    public IEnumerator CreateUserAndCheckExistence()
    {
        yield return null;

        string username = "testuser";
        string password = "password123!";
        testUsers.Add(username);

        db.CreateUser(username, password);
        bool exists = db.UserExists(username, password);

        Assert.IsTrue(exists, "User should exist after creation.");
    }

    [UnityTest]
    public IEnumerator GetTopScores_ReturnsUsers()
    {
        yield return null;

        string user1 = "username1";
        string user2 = "username2";
        string pass1 = "password1!";
        string pass2 = "password2!";

        testUsers.Add(user1);
        testUsers.Add(user2);

        db.CreateUser(user1, pass1);
        db.CreateUser(user2, pass2);

        int id1 = db.GetUserID(user1, pass1);
        int id2 = db.GetUserID(user2, pass2);

        db.UpdateUserScore(id1, 50);
        db.UpdateUserScore(id2, 100);

        List<UserScore> scores = db.GetTopScores(2);

        Assert.AreEqual(2, scores.Count);
        Assert.AreEqual(user2, scores[0].Username);
        Assert.AreEqual(100, scores[0].Score);
    }
}
