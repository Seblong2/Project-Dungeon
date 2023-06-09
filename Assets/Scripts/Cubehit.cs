using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubehit : MonoBehaviour
{
    private BoxCollider painzone;
    void Start()
    {
        painzone = gameObject.GetComponent<BoxCollider>();
        //painzone.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponentInChildren<PlayerController>().TakeDamage(2);
            transform.parent.GetComponent<Gelcube>().Land();
            painzone.enabled = false;
        }
    }
}
