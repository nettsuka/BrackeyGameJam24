using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 700f;
    public float jumpForce = 50f;

    public Animator animator;
    public Transform weaponTransform;
    public Rigidbody rigidBody;

    private bool isDefending = false;
    private bool isAttacking = false;
    private bool isJumping = false;
    private bool isGrounded = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Handle movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 rotatedMovement = transform.TransformDirection(movement);
        transform.Translate(rotatedMovement * movementSpeed * Time.deltaTime, Space.World);

        // Handle rotation
        float mouseX = Input.GetAxis("Mouse X");
        Vector3 rotation = new Vector3(0f, mouseX, 0f) * rotationSpeed * Time.deltaTime;
        transform.Rotate(rotation);

        // Handle animations
        HandleAnimations();

        // Handle actions
        if (Input.GetButtonDown("Fire1") && !isAttacking && !isDefending)
        {
            StartCoroutine(MeleeAttack());
            Debug.Log("Melee");
            Debug.Log(Time.timeScale);
        }

        if (Input.GetButtonDown("Fire2") && !isAttacking)
        {
            StartCoroutine(Defend());
            Debug.Log("Block");
        }

        if (Input.GetButtonDown("Fire3") && !isDefending && !isAttacking)
        {
            StartCoroutine(Shoot());
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
            Debug.Log("Jump");
        }
    }

    void HandleAnimations()
    {
        float movementInput = Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"));
        animator.SetFloat("Speed", movementInput);

        animator.SetBool("Block", isDefending);
        animator.SetBool("Melee", isAttacking);
    }

    void Jump()
    {
        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

        IEnumerator MeleeAttack()
    {
        isAttacking = true;
        animator.SetBool("Melee", isAttacking);

        // Add logic for dealing damage to enemies or other actions

        yield return new WaitForSeconds(0.9f);

        isAttacking = false;
        animator.SetBool("Melee", isAttacking);
    }

    IEnumerator Defend()
    {
        isDefending = true;
        animator.SetBool("Block", isDefending);

        // Add logic for reduced damage or other actions while defending
        if (Input.GetButtonUp("Fire2") && !isAttacking)
        {
            yield break;
            isDefending = false;
            animator.SetBool("Block", !isDefending);
        }

        yield return new WaitForSeconds(1.0f);

        isDefending = false;
        animator.SetBool("Block", isDefending);
    }

    IEnumerator Shoot()
    {
        animator.SetTrigger("Shoot");

        // Add logic for shooting, instantiate bullets, apply damage, etc.

        yield return null; // You might want to replace this with actual shoot logic

        // Reset animation state or cooldown if needed
    }
}
