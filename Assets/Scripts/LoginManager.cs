using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private GameObject welcomePanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject createPanel;
    [SerializeField] private GameObject leaderboardPanel;
    
    [SerializeField] private TMP_InputField usernameCreateInput;
    [SerializeField] private TMP_InputField passwordCreateInput;
    [SerializeField] private TMP_Text feedbackText;

    [SerializeField] private TMP_InputField usernameLoginInput;
    [SerializeField] private TMP_InputField passowordLoginInput;
    [SerializeField] private TMP_Text feedbackLoginText;

    [SerializeField] private TMP_Text leaderboard;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleGuestLogin()
    {
        //send information over to Main that we are guest
        int id = DatabaseManager.Instance.GetUserID("guest", "password1!");
        PlayerPrefs.SetInt("UserID", id);
        LoadMainScene();
    }
    public void HandleLogin()
    {
        string username = usernameLoginInput.text;
        string password = passowordLoginInput.text;

        bool userExists = DatabaseManager.Instance.UserExists(username, password);
        if (!userExists)
        {
            feedbackLoginText.text = "Incorrect information, please try again.";
            feedbackLoginText.color = Color.yellow;
            return;
        }

        int userID = DatabaseManager.Instance.GetUserID(username, password);
        PlayerPrefs.SetInt("UserID", userID);
        LoadMainScene();
    }

    public void HandleCreateAccount()
    {
        string username = usernameCreateInput.text;
        string password = passwordCreateInput.text;

        if (!IsValidPassword(password))
        {
            feedbackText.text = "Password must be at least 10 characters, include a number, and a special character.";
            feedbackText.color = Color.yellow;
            return;
        }

        //add to db and change scene
        DatabaseManager.Instance.CreateUser(username, password);
        LoadMainScene();
        
    }

    
    public void GotoLoginPanel()
    {
        welcomePanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    public void GotoWelcomePanel()
    {
        leaderboardPanel.SetActive(false);
        loginPanel.SetActive(false);
        createPanel.SetActive(false);
        welcomePanel.SetActive(true);
    }

    public void GotoCreatePanel()
    {
        welcomePanel.SetActive(false);
        createPanel.SetActive(true);
    }

    public void GotoLeaderboard()
    {
        welcomePanel.SetActive(false);
        leaderboardPanel.SetActive(true);
        leaderboard.text = " ";

        var scores = DatabaseManager.Instance.GetTopScores();

        for (int i = 0; i < scores.Count; i++)
        {
            leaderboard.text += $"{i + 1}. {scores[i].Username} — {scores[i].Score} points\n";
        }

    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    private bool IsValidPassword(string password)
    {

        if (password.Length < 10)
            return false;

        if (!Regex.IsMatch(password, @"\d"))
            return false;

        if (!Regex.IsMatch(password, @"[\W_]"))
            return false;

        return true;
    }
}
