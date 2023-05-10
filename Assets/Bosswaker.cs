using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bosswaker : MonoBehaviour
{
    public GameObject boss;

    private void OnTriggerEnter(Collider other)
    {
        boss.GetComponent<Boss>().active = true;
    }
}
