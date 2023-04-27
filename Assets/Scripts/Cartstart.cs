using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartstart : MonoBehaviour
{
    public GameObject[] carts;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        for (int i = carts.GetLength(0) - 1; i >= 0; i--)
        {
            carts[i].GetComponent<Movingplat>().begun = true;
        }
    }
}
