using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndManager : MonoBehaviour
{
    [SerializeField] private TMP_Text finalText;
    [SerializeField] private Button returnToMenuButton;
    
    public int _finalScore = 0;
    private string _endMessage = "Congratulations!\nYou completed the game!\nYour new high score is ";
    void Start()
    {
        int userID = PlayerPrefs.GetInt("UserID");
        _finalScore = DatabaseManager.Instance.GetUserScore(userID);
        _endMessage += _finalScore.ToString();
    }


    [SerializeField] private float typingSpeed = 0.5f;
    private float _timeLastTyped = 0;
    private int charsTyped = 0;
    
    void Update()
    {
        if (Time.timeSinceLevelLoad > _timeLastTyped + typingSpeed && charsTyped < _endMessage.Length)
        {
            _timeLastTyped = Time.timeSinceLevelLoad;
            charsTyped++;
            finalText.text = _endMessage.Substring(0, charsTyped);
        }

        if (charsTyped == _endMessage.Length)
        {
            returnToMenuButton.gameObject.SetActive(true);
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Login");
    }
    
}
