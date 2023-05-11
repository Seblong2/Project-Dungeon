using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleDoor : MonoBehaviour
{
    public GameManager manager;
    public GameObject door;
    public GameObject doortext;

    private void Start()
    {
        doortext.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (manager.score >= 20)
            {
                Destroy(door);
            }
            else
            {
                doortext.SetActive(true);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            doortext.SetActive(false);
            
        }
    }
}
