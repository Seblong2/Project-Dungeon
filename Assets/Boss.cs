using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{

    [SerializeField] private Image healtbar;
    [SerializeField] private Camera cam;

    public GameObject sap;
    
    public float enemyHealth;
    private float maxHealth;


    // Start is called before the first frame update
    void Start()
    {
        maxHealth = enemyHealth;
        UpdateHealthBar(maxHealth, enemyHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            
            TakeDamage(2);
        }

        healtbar.transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }

    private void Shoot()
    {
        GameObject Sap = Instantiate(sap, transform.position, transform.rotation);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("boss taking damage");
        enemyHealth -= damage;
        UpdateHealthBar(maxHealth, enemyHealth);

        if (enemyHealth <= 0)
        {

            Destroy(gameObject);
        }
    }

    public void UpdateHealthBar(float maxHealth, float enemyHealth)
    {

        healtbar.fillAmount = enemyHealth / maxHealth;
    }

}
