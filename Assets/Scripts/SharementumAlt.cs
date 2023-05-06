using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharementumAlt : MonoBehaviour
{
    private MovingPlatform pa;
    private bool ridden;
    private Vector3 velo;
    private Collider passenger;
    void Start()
    {
        ridden = false;
        pa = transform.parent.GetComponent<MovingPlatform>();
        velo = pa.velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        passenger = other;
        ridden = true;
        velo = pa.velocity / 50;
    }

    private void OnTriggerExit(Collider other)
    {
        ridden = false;
    }

    private void FixedUpdate()
    {
        if (ridden)
        {
            velo = pa.velocity / 50;
            Debug.Log(velo);
            passenger.transform.position += velo;
        }
    }
}
