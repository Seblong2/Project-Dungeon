using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject cameraHolder;

    private Vector2 _move;
    private Vector2 _look;
    private Vector2 _jump;
    private Vector2 _sprint;
    private Vector3 _jumpDirection;



    private float _lookRotation;
    private CharacterController characterController;

    [SerializeField] private float speed;
    [SerializeField] private float sensitivity;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _groundCheck;
    [SerializeField] private float _rayCheck;

    public float rateOfAttack;
    private float _resetAttack = 0;
    // [SerializeField] private Weapon _playerCombat;
    [SerializeField] private Sword _playerCombat;
    public float speedMultiplyer;

    public bool sprinting = false;
    public bool canDoubleJump = false;
    private bool hasDoubleJumped = false;
    public bool speedBoost = false;
    public float bonusSpeed;

    private Quaternion direction;

    public void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
        Debug.Log("moving");

    }


    public void OnLook(InputAction.CallbackContext context)
    {
        _look = context.ReadValue<Vector2>();
    }

    private void Awake()
    {
        characterController = new CharacterController();
       // _playerCombat = this.GetComponent<Sword>();
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
    }

    void Update()
    {
        direction = GameObject.Find("Cameraprop").GetComponent<Thecamera>().direction;
        transform.rotation = direction; //Vector3.up * _look.x * sensitivity);
        //_lookRotation += -_look.y * sensitivity;
        //_lookRotation = Mathf.Clamp(_lookRotation, -90, 90);
        //cameraHolder.transform.eulerAngles = new Vector3(_lookRotation, cameraHolder.transform.eulerAngles.y, cameraHolder.transform.eulerAngles.z);

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

    public void Jump(InputAction.CallbackContext context)
    {
        if (Grounded())
        {
            rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
            Debug.Log("Called");
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
            Debug.Log("is grounded");
            hasDoubleJumped = false;
            return true;
        }
        else
        {
            Debug.Log("False");
            return false;
        }
    }
}
