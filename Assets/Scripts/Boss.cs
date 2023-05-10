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
    public float rotationSpeed;
    public float projectileSpeed;
    private GameObject projectile;

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

        Walk();
        if (Time.time >= nextFireTime)
        {
            // Calculate the direction of the projectile towards the player
            Vector3 projectileDirection = (player.transform.position - projectileSpawnPoint.position).normalized;

            projectile = Instantiate(sap, projectileSpawnPoint.position, Quaternion.LookRotation(projectileDirection));
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            projectileRb.velocity = projectileDirection * projectileSpeed;

            nextFireTime = Time.time + fireRate;

            // Destroy the projectile after 2 seconds
            Destroy(projectile, 4f);
        }

        // Check for collision with player
        if (projectile != null && Vector3.Distance(projectile.transform.position, player.transform.position) <= sapExplosionRaidus)
        {
            player.GetComponent<PlayerController>().TakeDamage(rangeDamage);
            Destroy(projectile);
        }

    }

    private void Stomp()
    {
        // Cast a ray downwards to get the ground position
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            // Calculate the move direction relative to the ground position
            Vector3 groundPosition = hit.point;
            Vector3 moveDirection = (player.transform.position - groundPosition).normalized;

            // Move towards player with gravity
            Rigidbody rb = GetComponent<Rigidbody>();
            Vector3 forceDirection = moveDirection * moveSpeed + Physics.gravity;
            rb.AddForce(forceDirection * Time.deltaTime, ForceMode.VelocityChange);

            // Rotate towards player
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Perform stomp attack
            if (Vector3.Distance(transform.position, player.transform.position) <= stompRange)
            {
                animator.SetInteger("Attack", 1);
                player.GetComponent<PlayerController>().TakeDamage(stompDamage);
            }
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
            Destroy(gameObject);
        }
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}


  