using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    GameObject Varian;
    [SerializeField] GameObject BlueGem;
    public LayerMask playerLayers;
    public Transform attackPoint;
    public float attackRange = 2.6f;
    public float attackDamage = 20f;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    float Speed = 1;
    private float DistanceFromVarian;
    Animator animator;
    AudioSource audioSource;
    [SerializeField] AudioClip deathSound;
    float health = 1000f;
    bool isAlive = true, hasEntered = false;

    int RandomAttack;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        if (Varian == null)
        {
            Varian = GameObject.FindWithTag("Player");
        }
    }

    void Update()
    {
        if (Varian == null || !isAlive || !hasEntered) return;

        DistanceFromVarian = Vector2.Distance(transform.position, Varian.transform.position);
        print(DistanceFromVarian);
        if (Varian.GetComponent<Varian>().isAlive && DistanceFromVarian > 3.5f)
        {
            Chasing();
        }
        else if (Time.time >= nextAttackTime)
        {
            Attacking();
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
        RandomAttack = Random.Range(0, 2);

        if (RandomAttack == 0)
        {
            animator.SetTrigger("Attacking");
            attackDamage = 20;
        }
        else
        {
            animator.SetTrigger("Cast");
            attackDamage = 10;
        }
        nextAttackTime = Time.time + 2f;
        StartCoroutine(DealDamageAfterDelay(0.5f));
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
            isAlive = false;
            animator.SetBool("Attacking", false);
            animator.SetTrigger("Death");
            audioSource.PlayOneShot(deathSound);
            GameObject.FindObjectOfType<StoryController>().SendMessage("ContinueStory");
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

    void SetHasEntered()
    {
        hasEntered = true;
    }
}
