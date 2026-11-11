using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class GameOverButtons : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    
    void Start()
    {
        ScoreText.text = "Highscore: " + Player.Score;
    }

    
    public void Restart()
    {
        SceneManager.LoadScene("Action");
    }

    public void Controls()
    {
        SceneManager.LoadScene("Instructions");
    }
}