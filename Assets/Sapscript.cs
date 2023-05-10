using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sapscript : MonoBehaviour
{
public GameObject prefab;

    private Quaternion direction;
    private Vector3 speed = new Vector3(0f, 0f, 0.5f);

    private GameObject target;
    private bool aggro;
    private SphereCollider attack;

    private Rigidbody rb;

    void Awake()
    {
        attack = transform.GetChild(1).GetComponent<SphereCollider>();
        direction = new Quaternion(0, 0, 0, 0);
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (aggro)
        {
            transform.LookAt(target.transform.position);
            transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            direction = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            rb.velocity += direction * speed;
        }
    }

    public void Chase(GameObject victim, bool hunt)
    {
        if (hunt)
        {
            target = victim;
            aggro = true;
            attack.enabled = true;
        }
        else
        {
            aggro = false;
        }
    }

    public void Pain()
    {
        Debug.Log("deaded.");
        Destroy(gameObject);
    }
}
