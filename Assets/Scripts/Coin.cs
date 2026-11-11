using UnityEngine;
using System.Collections.Generic;

public class Coin : MonoBehaviour
{
    public List <Vector3> pos = new List<Vector3>();
    public Background background;
    public Player player;
    
    public AudioSource coin;
    public Animator anim;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        anim = GetComponent<Animator>();
        Positions();
    }

    void Positions()
    {
        pos.Add(new Vector2(-5.45f, -3f));
        pos.Add(new Vector2(-4.6f, -2f));
        pos.Add(new Vector2(0, -2.7f));
        pos.Add(new Vector2(-3.15f, -0.95f));
        pos.Add(new Vector2(-6.3f, -1.6f));
        pos.Add(new Vector2(-0.55f, -0.05f));
        pos.Add(new Vector2(2.15f, 0.05f));
        pos.Add(new Vector2(5.7f, -2.6f));
        pos.Add(new Vector2(6.7f, -1.7f));

        int start = Random.Range(0, pos.Count);
        Vector2 startPos = pos[start];
        transform.position = startPos;
    }
    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject CollidedObject = other.gameObject;
        if (CollidedObject.CompareTag("Player"))
        {
            int i = Random.Range(0, pos.Count);
            int j = Random.Range(0, pos.Count);
            Vector2 targetPos = pos[i];
            Vector2 LastPos = pos[j];

            bool SamePos = LastPos == targetPos;

            if (SamePos)
            {
                transform.position = targetPos;
            }

            transform.position = targetPos;
            background.ChangeScore(1);
            coin.Play();
        }
    }
}
