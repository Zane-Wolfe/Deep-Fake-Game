using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlideHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_Text answer1Text;
    [SerializeField] private TMP_Text answer2Text;
    [SerializeField] private TMP_Text answer3Text;
    [SerializeField] private TMP_Text answer4Text;

    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Image resultPanel;

    private Slide currentSlide;
    
    public void NextSlide(Slide slide)
    {
        resultPanel.gameObject.SetActive(false);
        currentSlide = slide;
        questionText.text = slide.Question;
        answer1Text.text = slide.Answer1;
        answer2Text.text = slide.Answer2;
        answer3Text.text = slide.Answer3;
        answer4Text.text = slide.Answer4;
    }

    public bool ChooseOption(int option)
    {
        resultPanel.gameObject.SetActive(true);
        if (currentSlide.CorrectAnswer == option)
        {
            resultText.text = currentSlide.CorrectAnswerText;
            return true;
        }
        resultText.text = currentSlide.WrongAnswerText;
        return false;
    }
    
}
