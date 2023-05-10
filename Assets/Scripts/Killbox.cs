using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Killbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerController>().Death();
    }
}
