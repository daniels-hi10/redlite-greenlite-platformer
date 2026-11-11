using UnityEngine;

public class Medals : MonoBehaviour
{
    public SpriteRenderer sr;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        AwardMedal();
    }
    
    void Update()
    {
        AwardMedal();
    }

    void AwardMedal()
    {
        if (Player.Score >= 25) sr.color = new Color(0.99f, 0.93f, 0.41f, 1f);
        else if (Player.Score >= 15) sr.color = new Color(0.74f, 0.74f, 0.74f, 1f);
        else if (Player.Score >= 5) sr.color = new Color(0.72f, 0.4f, 0.1f, 1f);
        else sr.color = Color.black;
    }
}

