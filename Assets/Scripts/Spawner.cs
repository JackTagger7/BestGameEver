using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject pipe;
    public GameObject powerup;
    public GameObject spowerup;
    public float spawnRate = 1f;
    public float minHeight = -1f;
    public float maxHeight = 2f;
    private int minPowerFreq = 0;


    public void StopSpawning()
    {
        CancelInvoke(nameof(SpawnPipe));
        CancelInvoke(nameof(SpawnPowerUP));
        CancelInvoke(nameof(SpawnSPowerUp));
    }
    public void StartSpawning()
    {
        InvokeRepeating(nameof(SpawnPipe), spawnRate, spawnRate);
        InvokeRepeating(nameof(SpawnPowerUP), (spawnRate / 2) + (spawnRate * 10), spawnRate);
        InvokeRepeating(nameof(SpawnSPowerUp), (spawnRate / 2) + (spawnRate * 10), spawnRate);
    }

    private void SpawnPipe()
    {
        GameObject pipes = Instantiate(pipe, transform.position, Quaternion.identity);
        pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
    }
    private void SpawnPowerUP()
    {
        if (minPowerFreq <= 0)
        {

            float roll = Random.Range(0f, 1f);
            if (roll <= 0.2f)
            {
                minPowerFreq = 15;
                GameObject powerupInstance = Instantiate(powerup, transform.position, Quaternion.identity);
                powerupInstance.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
            }
        }
        minPowerFreq -= 1;
    }

    private void SpawnSPowerUp()
    {
        if (minPowerFreq <= 0)
        {

            float roll = Random.Range(0f, 1f);
            if (roll <= 0.2f)
            {
                minPowerFreq = 15;
                GameObject spowerupInstance = Instantiate(spowerup, transform.position, Quaternion.identity);
                spowerupInstance.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
            }
        }
        minPowerFreq -= 1;
    }

}
