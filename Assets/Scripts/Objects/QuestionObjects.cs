using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestionData
{
    public string question;
    public string correctAnswer;
    public string wrongAnswer;
    public string[] answers;
    public int correctChoice;
}

[System.Serializable]
public class QuestionList
{
    public QuestionData[] questions;
}
