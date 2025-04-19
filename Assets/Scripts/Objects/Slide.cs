using UnityEngine;
using UnityEngine.UI;

public class Slide
{
    public Slide(Sprite image, string question,
        string answer1, string answer2, string answer3, string answer4, int correctAnswer,
        string correctAnswerText, string wrongAnswerText)
    {
        Image = image;
        Question = question;
        Answer1 = answer1;
        Answer2 = answer2;
        Answer3 = answer3;
        Answer4 = answer4;
        CorrectAnswer = correctAnswer;
        CorrectAnswerText = correctAnswerText;
        WrongAnswerText = wrongAnswerText;
    }

    public Sprite Image { get;}
    public string Question { get; }
    public string Answer1 { get; }
    public string Answer2 { get; }
    public string Answer3 { get; }
    public string Answer4 { get; }
    public int CorrectAnswer { get; }
    public string CorrectAnswerText { get; }
    public string WrongAnswerText { get; }
}
