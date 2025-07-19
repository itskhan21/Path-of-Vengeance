using UnityEngine;
using System.Collections;

public class SpinningScythe : MonoBehaviour
{
    [Header("Axe Properties")]
    public GameObject axePrefab;

    [Header("Upgrade Levels")]
    public AxeUpgrade[] upgradeLevels; // Array of predefined upgrade levels
    public int currentLevel = 0;      // Current upgrade level

    private float currentAngle = 0f;        // Current orbit angle around the player
    private float currentAxeSpinAngle = 0f; // Current local spin angle around the axe's own Z-axis
    private GameObject activeAxe;


    void Start()
    {
        // Start the spawning cycle when the game starts
        ApplyUpgrade(currentLevel); // Apply initial upgrade
        StartCoroutine(AxeSpawnCycle());
    }

    void Update()
    {
        // If we have an active axe, update its position and rotation
        if (activeAxe != null)
        {
            // Increment the orbit angle
            currentAngle += upgradeLevels[currentLevel].rotationSpeed * Time.deltaTime;

            // Convert orbit angle to radians for Cos/Sin
            float angleRad = currentAngle * Mathf.Deg2Rad;

            // Calculate the orbit position
            Vector3 offset = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0f) * upgradeLevels[currentLevel].spinRadius;
            activeAxe.transform.position = transform.position + offset;

            // Now spin the axe around its own Z-axis
            currentAxeSpinAngle += upgradeLevels[currentLevel].axeSpinSpeed * Time.deltaTime;
            activeAxe.transform.rotation = Quaternion.Euler(0f, 0f, currentAxeSpinAngle);
        }
    }

    private IEnumerator AxeSpawnCycle()
    {
        while (true)
        {
            // Spawn the axe
            SpawnAxe();

            // Keep the axe alive for 'timeToLive' seconds
            yield return new WaitForSeconds(upgradeLevels[currentLevel].timeToLive);

            // Destroy the axe
            if (activeAxe != null)
            {
                Destroy(activeAxe);
                activeAxe = null;
            }

            // Wait for 'spawnDelay' seconds before spawning the next axe
            yield return new WaitForSeconds(upgradeLevels[currentLevel].spawnDelay);
        }
    }

    private void SpawnAxe()
    {
        // Reset angles so the axe starts at a consistent position each time it spawns
        currentAngle = 0f;
        currentAxeSpinAngle = 0f;

        // Spawn a new axe
        activeAxe = Instantiate(axePrefab, transform.position + new Vector3(upgradeLevels[currentLevel].spinRadius, 0, 0), Quaternion.identity);
        activeAxe.transform.localScale = upgradeLevels[currentLevel].axeSize;
    }

    public void UpgradeAxe()
    {
        // Increase the level, if within bounds
        if (currentLevel < upgradeLevels.Length - 1)
        {
            currentLevel++;
            ApplyUpgrade(currentLevel);
        }
        else
        {
            Debug.Log("Axe is already at max level!");
        }
    }

    private void ApplyUpgrade(int level)
    {
        if (level >= 0 && level < upgradeLevels.Length)
        {
            Debug.Log($"Axe upgraded to Level {level + 1}: TimeToLive = {upgradeLevels[level].timeToLive}, Damage = {upgradeLevels[level].axeDamage}");
        }
    }

    public string GetUpgradeString()
    {
        if (currentLevel == 0 && !this.isActiveAndEnabled)
        {
            return "Unlocks a spinning sword that damages enemies";
        }
        else
        {
            return "Upgrade spinning sword damage, speed or size.";
        }
    }

    public string GetLevel()
    {
        if (currentLevel == 0 && !this.isActiveAndEnabled)
            return "Unlock Sword";
        else
            return "Lv. " + currentLevel + 1 + " Sword";
    }

}

[System.Serializable]
public class AxeUpgrade
{
    public float timeToLive = 3f;       // How long the axe stays alive
    public float spawnDelay = 2f;      // How long to wait before spawning a new one
    public float rotationSpeed = 100f; // Orbit speed (degrees/sec)
    public float spinRadius = 2f;      // Orbit radius from the player
    public Vector3 axeSize = Vector3.one; // Axe scale
    public float axeSpinSpeed = 180f;  // Speed of spinning on its own Z-axis
    public float axeDamage = 20f;      // Damage dealt by the axe
}

