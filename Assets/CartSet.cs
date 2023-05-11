using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartSet : MonoBehaviour
{
    public GameObject[] carts;

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        for (int i = carts.GetLength(0) - 1; i >= 0; i--)
        {
            carts[i].GetComponent<Movingplat>().Reset();
        }
    }
}
