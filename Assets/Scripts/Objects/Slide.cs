using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slide : MonoBehaviour
{
    [SerializeField] private List<Image> images = new();
    [SerializeField] private int correctIndex;
    [SerializeField] private Image correctImage;
    [SerializeField] private string question;
    [SerializeField] private string wrongAnswerMessage;
    
}
