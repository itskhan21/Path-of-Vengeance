using UnityEngine;

public class ElectricShield : MonoBehaviour
{
    [Header("Shield Properties")]
    public float radius = 2f; // Initial radius
    public float damage = 20f; // Initial damage
    public int currentLevel = 0; // Current upgrade level

    [Header("Upgrade Levels")]
    public ShieldUpgrade[] upgradeLevels; // Array of predefined upgrade levels

    private CircleCollider2D shieldCollider;

    void Start()
    {
        // Ensure the shield has a CircleCollider2D for the trigger
        shieldCollider = GetComponent<CircleCollider2D>();
        if (shieldCollider == null)
        {
            shieldCollider = gameObject.AddComponent<CircleCollider2D>();
        }
        shieldCollider.isTrigger = true;

        // Apply initial upgrade values
        ApplyUpgrade(currentLevel);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Miniboss") || collision.name == "Final Boss")
        {
            EnemyAI enemyAI = collision.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(damage*Time.deltaTime);
            }
        }
    }

    public void UpgradeShield()
    {
        // Increase the level, if within bounds
        if (currentLevel < upgradeLevels.Length - 1)
        {
            currentLevel++;
            ApplyUpgrade(currentLevel);
        }
        else
        {
            Debug.Log("Shield is already at max level!");
        }
    }

    private void ApplyUpgrade(int level)
    {
        if (level >= 0 && level < upgradeLevels.Length)
        {
            radius = upgradeLevels[level].radius;
            damage = upgradeLevels[level].damage;

            transform.localScale = new Vector3(radius, radius, 1); 

            // Update the shield collider radius
            if (shieldCollider != null)
            {
                shieldCollider.radius = radius;
            }

            Debug.Log($"Shield upgraded to Level {level + 1}: Radius = {radius}, Damage = {damage}");
        }
    }

    public string GetUpgradeString()
    {
        if (currentLevel == 0 && !gameObject.activeSelf)
        {
            return "Unlocks an electric shield that damages enemies";
        }
        else
        {
            return string.Format("Sets radius to {0:F2} and damage to {1}", upgradeLevels[currentLevel].radius, upgradeLevels[currentLevel].damage);
        }
    }

    public string GetLevel()
    {
        if (currentLevel == 0 && !gameObject.activeSelf)
            return "Unlock Shield";
        else
            return "Lv. " + currentLevel + 1 + " Shield";
    }

}

[System.Serializable]
public class ShieldUpgrade
{
    public float radius;
    public float damage;
}