using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int _score;
    private int _userID;

    [SerializeField] private AudioManager audioManager;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        audioManager.PlayMain();
        // Load current user account
        _userID = PlayerPrefs.GetInt("UserID");
        // Load previous user score if one exists
        _score = DatabaseManager.Instance.GetUserScore(_userID);
    }

    public void EndGame()
    {
        // Save score once game is over
        DatabaseManager.Instance.UpdateUserScore(_userID, _score);
        // Load end scene
        SceneManager.LoadScene("End");
    }

    public void AddScore(int score)
    {
        _score += score;
    }

    public int GetScore()
    {
        return _score;
    }
}
