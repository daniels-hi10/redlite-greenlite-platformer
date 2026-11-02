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

    public Player player;

    // public float GreenLength;
    // public float RedLength;
    // public float BlackLength;
    
    public Lights Colors = Lights.Green;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        
        GreenTimer = 10f;
        RedTimer = 5f;
        BlackTimer = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        LightSwitch();

        if (Colors == Lights.Green)
            GreenTimer -= Time.deltaTime;
        
        if (Colors == Lights.Red)
            RedTimer -= Time.deltaTime;

        if (Colors == Lights.Black)
            BlackTimer -= Time.deltaTime;
    }

    public void LightSwitch()
    {
        switch (Colors)
        {
            case Lights.Green:
            {
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
                if (player.CurrentState != States.Idle)
                {
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
                if (BlackTimer <= 0)
                {
                    Colors = Lights.Green;
                    BlackTimer = 3f;
                }
                
                Debug.Log(Colors);
            }
                break;
        }
    }
}
