using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static List<Slide> slides;
    private int slideIndex = 0;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        
    }

    void Update()
    {
        
    }

 
    public void NextSlide()
    {
        slideIndex++;

    }
}
