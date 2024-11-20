using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class QuizQuestion
{
    public int locationId;
    public string question;
    public string[] options;
    public string correctAnswer;
}

[System.Serializable]
public class QuizQuestionList
{
    public QuizQuestion[] questions;
}

public class QuizManager : MonoBehaviour
{
    public GameObject quizUI;
    public Text questionText;
    public Button[] optionButtons;
    public PlayerController playerController;

    private List<QuizQuestion> quizQuestions;
    private QuizQuestion currentQuestion;

    void Start()
    {
        quizUI.SetActive(false);
        LoadQuestionsFromJSON();
    }

    private void LoadQuestionsFromJSON()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "QuizData/questions.json");
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            QuizQuestionList loadedData = JsonUtility.FromJson<QuizQuestionList>(dataAsJson);
            quizQuestions = new List<QuizQuestion>(loadedData.questions);
        }
        else
        {
            Debug.LogError("質問ファイルが見つかりません: " + filePath);
        }
    }

    // 特定の場所に対応するクイズを表示する関数
    public void ShowQuizForLocation(int locationId)
    {
        // 指定した locationId に対応するクイズを検索
        currentQuestion = quizQuestions.Find(q => q.locationId == locationId);
        
        if (currentQuestion != null)
        {
            quizUI.SetActive(true);
            questionText.text = currentQuestion.question;

            playerController.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            for (int i = 0; i < optionButtons.Length; i++)
            {
                optionButtons[i].gameObject.SetActive(i < currentQuestion.options.Length);
                optionButtons[i].GetComponentInChildren<Text>().text = currentQuestion.options[i];
                optionButtons[i].onClick.RemoveAllListeners();
                string selectedOption = currentQuestion.options[i];
                optionButtons[i].onClick.AddListener(() => CheckAnswer(selectedOption));
            }
        }
        else
        {
            Debug.LogError("指定された場所IDのクイズが見つかりません: " + locationId);
        }
    }

    public void CheckAnswer(string selectedOption)
    {
        if (selectedOption == currentQuestion.correctAnswer)
        {
            Debug.Log("正解です！");
        }
        else
        {
            Debug.Log("不正解です！");
        }

        quizUI.SetActive(false);
        playerController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
