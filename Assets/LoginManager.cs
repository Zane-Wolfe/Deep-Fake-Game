using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private GameObject welcomePanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject createPanel;
    [SerializeField] private GameObject leaderboardPanel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GotoLoginPanel()
    {
        welcomePanel.SetActive(false);
        loginPanel.SetActive(true);
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
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
