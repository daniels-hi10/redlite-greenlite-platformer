using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public enum States
{
    Idle,
    Walk,
    Jump,
    Lunge
}
public class Player : MonoBehaviour
{
    public List<GameObject> Platforms;
    public States CurrentState = States.Idle; // default animation state
    public Rigidbody2D rb; // rigidbody for physics
    public SpriteRenderer sr; // sprite renderer for sprites
    public bool OnPlatform; // am i on the platform?

    //character stats
    public float Speed; // character speed
    public float JumpVel; // character jump velocity
    
    // lunge stats
    public float LungeRate; // lunge rate, speed multiplier when character runs
    public float Duration; // lunge duration
    public float Cooldown; // lunge cooldown

    // timers
    public float CooldownTimer;
    public float LungeTimer;

    public AudioSource jump;
    public AudioSource dash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentState = States.Idle; // set default to idle
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        Speed = 3;
        JumpVel = 4;
        
        LungeRate = 2;
        Duration = 0.4f;
        Cooldown = 0.6f;
        OnPlatform = false;
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardControl();
        SwitchState();
    }

    public void KeyboardControl()
    {
        CurrentState = States.Idle;
        
        LungeTimer -= Time.deltaTime; // timer for lunge
        CooldownTimer -= Time.deltaTime; // timer for cooldowns
        
        Vector2 vel = new Vector2(0, rb.linearVelocityY); // character velocity
        
        
        if (Keyboard.current.rightArrowKey.isPressed) // if right arrow key pressed
        {
            vel.x = Speed; // go right
            CurrentState = States.Walk; // change anim state
        }
        if (Keyboard.current.leftArrowKey.isPressed) // if left arrow key pressed
        {
            vel.x = -Speed; // go left
            CurrentState = States.Walk; // change anim state
        }

        
        if (Keyboard.current.cKey.wasPressedThisFrame && OnGround()) // if space pressed + if im not midair
        {
            vel.y = JumpVel; // jump
            jump.Play();
            CurrentState = States.Jump;
            // OnPlatform = false; // prevents doublejumping while midair
        }

        if (OnPlatform == false)
        {
            CurrentState = States.Jump; // change anim state
        }

        
        if (Keyboard.current.xKey.wasPressedThisFrame && CooldownTimer <= 0) // if C key pressed + cooldown timer is 0
        {
            // resets timer
            LungeTimer = Duration;
            CooldownTimer = Cooldown;
        }

        if (LungeTimer > 0)
        {
            dash.Play();
            // dash to the direction of where im facing
            if (Keyboard.current.rightArrowKey.isPressed)
            {
                vel.x = Speed * LungeRate;
            }
            else if (Keyboard.current.leftArrowKey.isPressed)
            {
                vel.x = -Speed * LungeRate;
            }

            CurrentState = States.Lunge;
        }

        rb.linearVelocity = vel;
    }
    public void SwitchState() // change anim state
    {
        switch (CurrentState)
        {
            case States.Idle:
                sr.color = Color.white;
                break;

            case States.Walk:
                sr.color = Color.green;
                break;

            case States.Jump:
                sr.color = Color.orange;
                break;
            
            case States.Lunge:
                sr.color = Color.cyan;
                break;
        }
    }
    

    public bool OnGround()
    {
        return Platforms.Count > 0;
    }

    // collision stuff
    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject CollidedObject = other.gameObject;
        if (!Platforms.Contains(CollidedObject))
        {
            Platforms.Add(CollidedObject);
            OnPlatform = true;  // changes bool to true if im not mid-air
        }

        if (CollidedObject.CompareTag("Hazard"))
        {
            transform.position = new Vector2(-7.03f, -2.95f);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        Platforms.Remove(other.gameObject);
        OnPlatform = false;
    }
}
