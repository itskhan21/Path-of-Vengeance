using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    GameObject Varian;
    [SerializeField] GameObject BlueGem;
    public LayerMask playerLayers;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float attackDamage = 20f;
    public float attackRate = 1.5f;
    private float nextAttackTime = 0f;
    private float attackDistance = 1.1f;

    float Speed = 1;
    private float DistanceFromVarian;
    Animator animator;
    float health;
    bool isAlive = true;

    AudioSource audioSource;
    [SerializeField] AudioClip death;

    void Start()
    {
        if (gameObject.tag == "Miniboss")
            attackDistance = 2f;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (Varian == null)
        {
            Varian = GameObject.FindWithTag("Player");
        }

        if (gameObject.tag == "Miniboss")
            health = 500;
        else
            health = 100;
    }

    void Update()
    {
        if (Varian == null || !isAlive) return;

        DistanceFromVarian = Vector2.Distance(transform.position, Varian.transform.position);

        if (Varian.GetComponent<Varian>().isAlive)
        {
            if (DistanceFromVarian > attackDistance)
            {
                Chasing();
            }
            else if (Time.time >= nextAttackTime)
            {
                Attacking();
            }
        }
    }

    private void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    void Attacking()
    {
        animator.SetTrigger("Attacking");
        nextAttackTime = Time.time + 1f / attackRate;
        StartCoroutine(DealDamageAfterDelay(0.3f));
    }

    private IEnumerator DealDamageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
        foreach (Collider2D player in hitPlayer)
        {
            Varian varianScript = player.GetComponent<Varian>();
            if (varianScript != null && varianScript.isAlive)
            {
                Debug.Log("Enemy hit " + player.name);
                varianScript.TakeDamage(attackDamage);
            }
        }
    }

    void Chasing()
    {
        if (isAlive)
        {
            Vector2 direction = Varian.transform.position - transform.position;
            direction.Normalize();

            // Flip the enemy to face the player
            if (direction.x > 0 && transform.localScale.x < 0)
            {
                Flip();
            }
            else if (direction.x < 0 && transform.localScale.x > 0)
            {
                Flip();
            }

            transform.position = Vector2.MoveTowards(this.transform.position, Varian.transform.position, Speed * Time.deltaTime);
            animator.SetBool("Attacking", false);
        }
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine("DisplayHurt");
        health -= damage;
        Debug.Log(health);
        if (health <= 0 && isAlive)
        {
            if (gameObject.tag == "Miniboss")
            {
                GameObject.FindObjectOfType<StoryController>().SendMessage("ContinueStory");
            }
            isAlive = false;
            animator.SetBool("Attacking", false);
            animator.SetTrigger("Death");
            audioSource.PlayOneShot(death);
            Invoke("SpawnGem", 1f);
            Destroy(gameObject, 1f);
        }
    }

    private IEnumerator DisplayHurt()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    void SpawnGem()
    {
        Instantiate(BlueGem, transform.position, transform.rotation);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


}
