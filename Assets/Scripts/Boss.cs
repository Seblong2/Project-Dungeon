using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    public Animator animator;
    public GameObject sap;
    public float maxHealth;
    public float currentHealth;
    public bool active;
    public GameObject bossUI;
    public GameObject player;
    public int stompDamage;
    public int rangeDamage;
    public float nextFireTime;
    public Transform projectileSpawnPoint;
    public float sapExplosionRaidus;
    public float stompRange;
    public float moveSpeed;

    public enum BossState
    {
        Stomp,
        Fire,
        FireAndStomp,
    }

    public float fireRateIncreaseThreshold;
    public float moveSpeedIncreaseThreshold;

    public BossState currentState = BossState.Stomp;
    private float fireRate = 2f;
    private float lastFireTime;
    private float speed = 2f;
    private float originalSpeed;

    void Start()
    {
        currentHealth = maxHealth;
        bossUI.SetActive(false);
        UpdateHealthBar(maxHealth, currentHealth);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (active)
        {
            bossUI.SetActive(true);
                if (currentHealth > 0)
                {
                    switch (currentState)
                    {
                        case BossState.Stomp:
                            Stomp();
                            if (currentHealth <= maxHealth * 0.6f)
                            {
                                currentState = BossState.Fire;
                            }
                            break;

                        case BossState.Fire:
                            Fire();
                            if (currentHealth <= maxHealth * 0.3f)
                            {
                                currentState = BossState.FireAndStomp;
                            }
                            break;

                        case BossState.FireAndStomp:
                            Fire();
                            Stomp();
                            break;

                        default:
                            Debug.LogError("Invalid state");
                            break;
                    }
                }

            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("taking damage");
                TakeDamage(2);
            }
        }
    }

    private void Fire()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);

        // Increase fire rate and move speed at low health
        if (currentHealth <= maxHealth * moveSpeedIncreaseThreshold)
        {
            animator.speed = 2f;
        }

        if (currentHealth <= maxHealth * fireRateIncreaseThreshold)
        {
            fireRate = 0.2f;
        }

        // Fire a projectile
        if (Time.time >= nextFireTime)
        {
           
            GameObject projectile = Instantiate(sap, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            nextFireTime = Time.time + fireRate;

            // Check if the projectile hit the player
            if (Vector3.Distance(sap.transform.position, player.transform.position) <= sapExplosionRaidus)
            {
                player.GetComponent<PlayerController>().TakeDamage(rangeDamage);
            }
        }
    }


    private void Stomp()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, player.transform.position) <= stompRange)
        {
            animator.SetInteger("Attack", 1);
            player.GetComponent<PlayerController>().TakeDamage(stompDamage);
        }
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthBar(maxHealth, currentHealth);

        if (currentHealth <= 0)
        {
            animator.SetBool("Living", false);
            active = false;
        }
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}

//older version down below 

/*
public class Boss : MonoBehaviour
{

    [SerializeField] private Image healtbar;
  //  [SerializeField] private Camera cam;

    public Animator animator;

    public GameObject sap;
    
    public float enemyHealth;
    private float maxHealth;
    public bool active;
    public GameObject bossUI;


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
        if (active)
        {
            bossUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("taking damage");
                TakeDamage(2);
            }
//            healtbar.transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
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
*/