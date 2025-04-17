using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static List<Slide> slides;
    private static int currentSlideIndex = 0;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private SlideHandler slideHandler;
    
    private void Start()
    {
        LoadSlides();
    }

    private void LoadSlides()
    {
        // Fetch slides from database here and populate slides list
    }

    void Update()
    {
        
    }

    public void chooseOption(int option)
    {
        slideHandler.ChooseOption(option);
    }
    
    public void NextSlide()
    {
        Slide slide = slides[currentSlideIndex];
        slideHandler.NextSlide(slide);
        currentSlideIndex++;
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
