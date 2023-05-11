using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    public int value;
    public GameManager gameManager;

    void Start()
    {
        
    }

   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            gameManager.Collect(value);
            Destroy(gameObject);
        }
    }

}
