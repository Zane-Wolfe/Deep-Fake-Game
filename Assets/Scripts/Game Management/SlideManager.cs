using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlideManager : MonoBehaviour
{
    private SlideHandler slideHandler;
    private List<Slide> slides = new List<Slide>();
    private int currentSlideIndex = -1;
    
    [SerializeField] private TMP_Text scoreText;
    
    void Start()
    {
        slideHandler = GetComponent<SlideHandler>();
        LoadSlides();
        // Set user score here
        
        // Start first slide
        NextSlide();
        
        // Load initial score
        scoreText.text = "Score\n" + GameManager.Instance.GetScore();   
    }
    
    void Update()
    {
        
    }
    
    private void LoadSlides()
    {
        // Fetch slides from database here and populate slides list
        slides = DatabaseManager.Instance.GetAllSlides();
    }
    
    public void ChooseOption(int option)
    {
        bool wasCorrectOption = slideHandler.ChooseOption(option);
        // Increment score variable
        if (wasCorrectOption)
        {
            int scoreIncrement = 5;
            GameManager.Instance.AddScore(scoreIncrement);
            scoreText.text = "Score\n" + GameManager.Instance.GetScore();   
        }
    }
    
    public void NextSlide()
    {
        // currentSlideIndex starts at -1 when the game first loads
        currentSlideIndex++;
        // Slides have finished, end game
        // Debug.Log(currentSlideIndex);
        // Debug.Log(slides.Count);
        if (currentSlideIndex == slides.Count)
        {
            GameManager.Instance.EndGame();
            return;
        }
        Slide slide = slides[currentSlideIndex];
        slideHandler.NextSlide(slide);
    }
}
