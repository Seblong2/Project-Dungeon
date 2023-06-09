using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject cameraHolder;

    private Vector2 _move;
    private Vector2 _look;
    private Vector2 _jump;
    private Vector2 _sprint;
    private Vector3 _jumpDirection;

    private Vector3 spawnpoint;

    private float _lookRotation;
    private CharacterController characterController;

    public float speed;
    [SerializeField] private float sensitivity;
    public float _jumpForce;
    [SerializeField] private float _groundCheck;
    [SerializeField] private float _rayCheck;

    public float rateOfAttack;
    private float _resetAttack = 0;
    // [SerializeField] private Weapon _playerCombat;
    [SerializeField] private Combat _playerCombat;
    public float speedMultiplyer;


    public bool sprinting = false;
    public bool canDoubleJump = false;
    private bool hasDoubleJumped = false;
    public bool speedBoost = false;
    public float bonusSpeed;

    private Quaternion direction;

    public float playerHealth;
    [SerializeField] private Image healtbar;
    private float maxHealth;
    private float iframes;

    public void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }


    public void OnLook(InputAction.CallbackContext context)
    {
        _look = context.ReadValue<Vector2>();
    }

    private void Awake()
    {
        characterController = new CharacterController();
       // _playerCombat = this.GetComponent<Sword>();
       spawnpoint = new Vector3(304.74f, 24.5f, -99.5f);
    }

    private void OnEnable()
    {
        characterController.Player.Enable();
        characterController.Player.Jump.performed += Jump;
        characterController.Player.Attack.performed += Attack;
        characterController.Player.Sprint.started += x => sprinting = true;
        characterController.Player.Sprint.canceled += x => sprinting = false;
    }

    private void OnDisable()
    {
        characterController.Player.Disable();
        characterController.Player.Jump.performed -= Jump;
        characterController.Player.Sprint.canceled -= x => Sprint();
        characterController.Player.Attack.canceled -= Attack;
        // characterController.Player.SprintReleased.canceled -= x => SprintReleased();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        maxHealth = playerHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        direction = GameObject.Find("Cameraprop").GetComponent<Thecamera>().direction;
        transform.rotation = direction;

        if (iframes > 0)
        {
            iframes -= Time.deltaTime;
            if (iframes <= 0)
            {
                gameObject.GetComponentInChildren<Knightskin>().Fade(false);
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(_move.x, 0, _move.y);


        if (!sprinting)
        {
            targetVelocity *= speed;

            if (!sprinting && speedBoost)
            {
                targetVelocity *= speed + bonusSpeed;
            }
        }
        else if (sprinting)
        {
            targetVelocity *= speed * speedMultiplyer;

            if (sprinting && speedBoost)
            {
                targetVelocity *= speed * speedMultiplyer + bonusSpeed;
            }
        }




        targetVelocity = transform.TransformDirection(targetVelocity);
        Vector3 velocityChange = targetVelocity - currentVelocity;
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);
        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        if (rb.velocity.y < 0f)
        {
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;
        }

    }

    public void TakeDamage(int damage)
    {
        if (iframes <= 0)
        {
            playerHealth -= damage;
            UpdateHealthBar();
            if (playerHealth <= 0)
            {
                Debug.Log("player dead");
                //animator.SetBool("Living", false);
                Death();
            }
            iframes = 1.5f;
            //gameObject.GetComponentInChildren<Knightskin>().Fade(false);
        }
    }

    public void Death()
    {
        playerHealth = maxHealth;
        transform.position = spawnpoint;
    }

    public void UpdateHealthBar()
    {
        healtbar.fillAmount = playerHealth / maxHealth;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (Grounded())
        {
            rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
        }
        else if (!Grounded() && canDoubleJump && !hasDoubleJumped)
        {
            rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
            hasDoubleJumped = true;

        }

    }

    public void Attack(InputAction.CallbackContext context)
    {

        if (Time.time >= _resetAttack)
        {

            _playerCombat.Attack();
            _resetAttack = Time.time + 1f / rateOfAttack;
        }


    }

    public void Sprint()
    {

        sprinting = true;
    }


    public void SprintReleased()
    {
        sprinting = false;
    }

    private bool Grounded()
    {

        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.3f))
        {
            hasDoubleJumped = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Setspawn(Vector3 spawn)
    {
        spawnpoint = spawn;
    }
}
