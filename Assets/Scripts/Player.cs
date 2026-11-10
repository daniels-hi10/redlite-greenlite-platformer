using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.LowLevelPhysics2D;

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
    public int Life; // character life
    public bool FacingLeft; // checks if character is facing left
    
    // lunge stats
    public float LungeRate; // lunge rate, speed multiplier when character runs
    public float Duration; // lunge duration
    public float Cooldown; // lunge cooldown
    // public float JumpTime;

    // timers + score
    public float CooldownTimer;
    public float LungeTimer;
    public static int Score;

    public AudioSource jump;
    public AudioSource dash;
    public Animator anim;
    public ParticleSystem part;

    public Background background;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentState = States.Idle; // set default to idle
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        Speed = 3;
        JumpVel = 4;
        Life = 3;
        
        LungeRate = 2;
        Duration = 0.4f;
        Cooldown = 0.6f;
        Score = 0;
        
        //OnPlatform = false;
        FacingLeft = false;
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

        OnPlatform = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, LayerMask.GetMask("Platform")).collider != null;
        
        if (Keyboard.current.rightArrowKey.isPressed) // if right arrow key pressed
        {
            vel.x = Speed; // go right
            FacingLeft = false;
            CurrentState = States.Walk; // change anim state
        }
        if (Keyboard.current.leftArrowKey.isPressed) // if left arrow key pressed
        {
            vel.x = -Speed; // go left
            FacingLeft = true;
            CurrentState = States.Walk; // change anim state
        }

        if (Keyboard.current.cKey.wasPressedThisFrame && OnGround())
        // if (Keyboard.current.cKey.wasPressedThisFrame && (OnPlatform || JumpTime < 0.3f)) // if space pressed + if im not midair
        {
            vel.y = JumpVel; // jump
            jump.Play();
            CurrentState = States.Jump;
            
            /* if (OnPlatform)
            {
                jump.Play();
                CurrentState = States.Jump; 
            } */
        }

        if (OnPlatform == false)
            CurrentState = States.Jump;

        /* if (OnPlatform)
        {
            JumpTime = 0;
        }
        else
        {
            JumpTime += Time.deltaTime;
            if (!Keyboard.current.cKey.isPressed)
                JumpTime = 999;
        } */

        
        if (Keyboard.current.xKey.wasPressedThisFrame && CooldownTimer <= 0) // if C key pressed + cooldown timer is 0
        {
            // resets timer
            LungeTimer = Duration;
            CooldownTimer = Cooldown;
            dash.Play();
        }

        if (LungeTimer > 0)
        {
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
        sr.flipX = FacingLeft;
    }
    
    public void SwitchState() // change anim state
    {
        switch (CurrentState)
        {
            case States.Idle:
                anim.SetBool("IsRunning", false); anim.SetBool("HasJumped", false);
                break;

            case States.Walk:
                anim.SetBool("IsRunning", true); anim.SetBool("HasJumped", false);
                break;

            case States.Jump:
                anim.SetBool("HasJumped", true); anim.SetBool("IsRunning", false);
                break;
            
            case States.Lunge:
                part.Emit(1);
                break;
        }
    }

    public void DepleteHealth()
    {
        if (gameObject == null) return;
        {
            Life -= 1;
        }
        
        if (Life <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    public bool OnGround()
    {
        // LayerMask l = LayerMask.GetMask("Platform");
        // RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, l.value);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, l.value);
        return Platforms.Count > 0;
        // OnPlatform = true;
        // return hit.collider != null;
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

        if (CollidedObject.CompareTag("Hazard")) // respawns if i hit a spike
        {
            transform.position = new Vector2(-7.03f, -2.95f);
            background.ChangeScore(-1);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        Platforms.Remove(other.gameObject);
        OnPlatform = false;
    }
}
