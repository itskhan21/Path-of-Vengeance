using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Varian : MonoBehaviour
{
    // Character variables
    public Vector2 moveDir;
    Rigidbody2D rb;
    float speed = 6.0f;
    float health = 100f;
    float maxHealth = 100f;
    float healthRegen = 0f;
    private bool canTakeDamage = true, canMove = true;
    public bool isAlive = true;

    // Attacking animation variables
    int attackType;

    // Attack variables
    public LayerMask enemyLayers;
    public float attackRange = 1.2f;
    public Transform attackPoint;
    private float attackDamage = 40f;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    Animator animator;
    AudioSource audioSource;
    [SerializeField] AudioClip swordSwing, death;
    [SerializeField] GameObject DeathScreen;

    // Health Bar
    [SerializeField] RectTransform HealthBar;
    [SerializeField] Text healthText;
    float originalHealthWidth;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        attackType = 0;
        originalHealthWidth = HealthBar.rect.width;
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive && gameManager.IsGameplay())
        {
            InputManager();
            Attack();
            UpdateHealthBar();
        }
        if (healthRegen > 0 && health < maxHealth && isAlive)
        {
            health += healthRegen * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (isAlive && canMove)
            Move();
        else
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    void UpdateHealthBar()
    {
        float width = (health / maxHealth) * originalHealthWidth;
        HealthBar.sizeDelta = new Vector2(width, HealthBar.rect.height);
        healthText.text = health.ToString("F1") + "/" + maxHealth;
    }

    void InputManager()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;
    }

    private void Move()
    {
        Vector2 movement = Vector2.zero;

        if (Input.GetKey(KeyCode.A))
        {
            movement.x = -1;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement.x = 1;
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }

        if (moveDir != Vector2.zero)
            animator.SetInteger("AnimState", 1);
        else
            animator.SetInteger("AnimState", 0);

        rb.velocity = new Vector2(moveDir.x * speed, moveDir.y * speed);
  
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime && canMove)
        {
            audioSource.PlayOneShot(swordSwing);
            // Set the appropriate attack trigger based on the attackType
            if (attackType == 0)
                animator.SetTrigger("Attack1");
            else if (attackType == 1)
                animator.SetTrigger("Attack2");
            else if (attackType == 2)
                animator.SetTrigger("Attack3");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {
                Debug.Log("We hit " + enemy.name);

                if (enemy.name == "Final Boss")
                    enemy.GetComponent<FinalBoss>().TakeDamage(attackDamage);
                else
                    enemy.GetComponent<EnemyAI>().TakeDamage(attackDamage);
            }

            // Cycle through attack types
            attackType++;
            attackType %= 3;
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }


    private IEnumerator WaitForAttackAnimation()
    {
        // Get the current animation state info
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Wait for the duration of the current animation plus a small buffer to account for transitions
        yield return new WaitForSeconds(stateInfo.length - 0.1f);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void TakeDamage(float damage)
    {
        if (canTakeDamage)
        {
            canTakeDamage = false;
            StartCoroutine("DisplayHurt");
            health -= damage;
            Debug.Log(health);
            if (health <= 0)
            {
                health = 0;
                UpdateHealthBar();
                isAlive = false;
                animator.SetTrigger("Death");
                audioSource.PlayOneShot(death);
                DeathScreen.SetActive(true);
            }
        }
    }

    private IEnumerator DisplayHurt()
    {
        animator.SetTrigger("Hurt");
        //GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        //GetComponent<SpriteRenderer>().color = Color.white;
        canTakeDamage = true;
    }

    public void SetMaxHealth(float max)
    {
        health += (max - maxHealth);
        maxHealth = max;
        if (health > maxHealth)
            health = max;
        
    }

    public void SetRegen(float regen)
    {
        healthRegen = regen;
    }

    void DisableMovement()
    {
        canMove = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Room Trigger")
        {
            GameObject.Find("Final Boss").SendMessage("SetHasEntered");
        }
    }
}
