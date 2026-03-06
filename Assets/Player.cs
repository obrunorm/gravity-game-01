using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 20f;
    public Transform groundCheck;
    public LayerMask groundLayer;


    private Rigidbody2D rb;
    private PlayerInputActions input;
    private Vector2 moveInput;
    private bool isGrounded;
    private Animator animator;
    private SpriteRenderer sprite;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDemage = 1;

    public Collider2D[] enemiesHit;

    void Awake()
    {
        input = new PlayerInputActions();
    }


    void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;  

        input.Player.Jump.performed += ctx => Jump();

        input.Player.Attack.performed += ctx => Attack();
    }

    void OnDisable()
    {
        input.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        animator.SetFloat("Speed", Mathf.Abs(moveInput.x));
        Flip();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
    }

    void Jump()
    {
        if(!isGrounded)
            return;

        Physics2D.gravity = -Physics2D.gravity;
    }

    public void Flip()
    {
        float direction = moveInput.x;

        if (Physics2D.gravity.y > 0)
            direction *= -1;

        if (direction > 0)
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        else if (direction < 0)
            transform.eulerAngles = new Vector3(0, 180, transform.eulerAngles.z);

        if (Physics2D.gravity.y > 0)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180);
        else
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    } 

    void Attack()
    {
        animator.SetTrigger("Attack");

        enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in enemiesHit)
            enemy.GetComponent<Enemy>().TakeDamage(attackDemage); 
    }
}
