using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{

    [SerializeField] private Image healtbar;
    [SerializeField] private Camera cam;

    public Animator animator;

    public GameObject sap;
    
    public float enemyHealth;
    private float maxHealth;
    public bool active;
    public GameObject bossUI;

    private int counter = 0;


    // Start is called before the first frame update
    void Start()
    {
        active = false;
        maxHealth = enemyHealth;
        bossUI.SetActive(false);
        UpdateHealthBar(maxHealth, enemyHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (counter > 1000)
        {
            Kick();
            counter = 0;
        }
        counter += 1;
        Debug.Log(counter);
        if (active)
        {
            bossUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("taking damage");
                TakeDamage(2);
            }
            healtbar.transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        }
    }

    private void Shoot()
    {
        animator.SetInteger("Attack", 2);
        Vector3 shootspot = transform.position + new Vector3(0, 3, 1);
        GameObject Sap = Instantiate(sap, shootspot, transform.rotation);
    }

    private void Kick()
    {
        animator.SetInteger("Attack", 1);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("boss taking damage");
        enemyHealth -= damage;
        UpdateHealthBar(maxHealth, enemyHealth);

        if (enemyHealth <= 0)
        {
            animator.SetBool("Living", false);
        }
    }

    public void UpdateHealthBar(float maxHealth, float enemyHealth)
    {

        healtbar.fillAmount = enemyHealth / maxHealth;
    }
}
