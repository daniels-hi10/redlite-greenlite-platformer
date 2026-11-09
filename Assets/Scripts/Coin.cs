using UnityEngine;
using System.Collections.Generic;

public class Coin : MonoBehaviour
{
    public List <Vector3> pos = new List<Vector3>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Positions()
    {
        pos.Add(;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    public void GetBumped()
    {
        int i = Random.Range(0, pos.Count);
        Vector2 targetPos = pos[i];
        
        Instantiate(gameObject, targetPos, Quaternion.identity);
        background.ChangeScore(1);
    }
}
