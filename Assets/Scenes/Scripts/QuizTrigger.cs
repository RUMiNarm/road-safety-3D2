using UnityEngine;

public class QuizTrigger : MonoBehaviour
{
    public QuizManager quizManager;
    public int locationId;  // この場所のID
    private bool isQuizShown = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isQuizShown)
        {
            quizManager.ShowQuizForLocation(locationId);  // この場所のIDに対応するクイズを表示
            isQuizShown = true;
        }
    }
}
