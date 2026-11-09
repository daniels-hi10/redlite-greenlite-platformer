using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public enum Lights
{
    Green,
    Red,
    Black
}
public class Background : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI lifeText;
    
    public float GreenTimer;
    public float RedTimer;
    public float BlackTimer;

    public SpriteRenderer sr;
    public Player player;

    public AudioSource buzzer;
    
    public Lights Colors = Lights.Green;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        sr = GetComponent<SpriteRenderer>();
        
        GreenTimer = 4f;
        RedTimer = 3f;
        BlackTimer = 2f;
        
        scoreText.text = "Score: " + player.Score;
        lifeText.text = "Life: " + player.Life;
    }

    // Update is called once per frame
    void Update()
    {
        LightSwitch();
    }

    public void LightSwitch()
    {
        switch (Colors)
        {
            case Lights.Green:
            {
                sr.color = new Color(0.41f, 0.67f, 0.4f, 1f);
                GreenTimer -= Time.deltaTime;
                lifeText.text = "Life: " + player.Life;
                scoreText.text = "Score: " + player.Score;
                timerText.text = "" + (int)GreenTimer;
                
                if (GreenTimer <= 1)
                {
                    Colors = Lights.Red;
                    GreenTimer = 4f;
                }
            }
                break;

            case Lights.Red:
            {
                sr.color = new Color(0.67f, 0.4f, 0.4f, 1f);
                RedTimer -= Time.deltaTime;
                timerText.text = "" + (int)RedTimer;
                
                if (player.CurrentState != States.Idle)
                {
                    buzzer.Play();
                    Colors = Lights.Black;
                    RedTimer = 3f;
                    
                    ChangeScore(-1);
                    lifeText.text = "Life: " + player.Life;
                    Debug.Log(player.Score);
                    scoreText.text = "Score: " + player.Score;
                    // ScoreTracker();
                }
                
                if (RedTimer <= 1)
                {
                    Colors = Lights.Green;
                    RedTimer = 3f;
                    
                    ChangeScore(1);
                    lifeText.text = "Life: " + player.Life;
                    Debug.Log(player.Score);
                    scoreText.text = "Score: " + player.Score;
                }
                
                //Debug.Log(Colors);
            }
                break;

            case Lights.Black:
            {
                sr.color = Color.black;
                BlackTimer -= Time.deltaTime;
                scoreText.text = "Score: " + player.Score;
                timerText.text = "" + (int)BlackTimer;
                
                if (BlackTimer <= 1)
                {
                    Colors = Lights.Green;
                    player.transform.position = new Vector2(-7.03f, -2.95f);
                    BlackTimer = 2f;
                    player.DepleteHealth();
                }
                
                // Debug.Log(Colors);
            }
                break;
        }
    }

    public void ChangeScore(int amt)
    {
        if (player == null) return;
        player.Score += amt;
        scoreText.text = "Score: " + player.Score;
    }
}
