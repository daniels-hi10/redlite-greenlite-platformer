using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Lights
{
    Green,
    Red,
    Black
}
public class Background : MonoBehaviour
{
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
        
        GreenTimer = 10f;
        RedTimer = 5f;
        BlackTimer = 3f;
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
                
                if (GreenTimer <= 0)
                {
                    Colors = Lights.Red;
                    GreenTimer = 10f;
                }
                
                Debug.Log(Colors);
            }
                break;

            case Lights.Red:
            {
                sr.color = new Color(0.67f, 0.4f, 0.4f, 1f);
                RedTimer -= Time.deltaTime;
                
                if (player.CurrentState != States.Idle)
                {
                    buzzer.Play();
                    Colors = Lights.Black;
                    RedTimer = 5f;
                }
                
                if (RedTimer <= 0)
                {
                    {
                        Colors = Lights.Green;
                        RedTimer = 5f;
                    }
                }
                
                Debug.Log(Colors);
            }
                break;

            case Lights.Black:
            {
                sr.color = Color.black;
                BlackTimer -= Time.deltaTime;
                
                if (BlackTimer <= 0)
                {
                    Colors = Lights.Green;
                    player.transform.position = new Vector2(-7.03f, -2.95f);
                    BlackTimer = 3f;
                }
                
                Debug.Log(Colors);
            }
                break;
        }
    }
}
