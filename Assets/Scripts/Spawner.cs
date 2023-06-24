using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject pipe;
    public GameObject powerup;
    public float spawnRate = 1f;
    public float minHeight = -1f;
    public float maxHeight = 2f;

    private void OnEnable()
    {
        InvokeRepeating(nameof(SpawnPipe), spawnRate, spawnRate);
        InvokeRepeating(nameof(SpawnPowerUP), spawnRate / 2, spawnRate);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(SpawnPipe));
    }

    private void SpawnPipe()
    {
        GameObject pipes = Instantiate(pipe, transform.position, Quaternion.identity);
        pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
    }
    private void SpawnPowerUP()
    {
        float roll = Random.Range(0f, 1f);
        if (roll <= 0.2f)
        {
            GameObject powerupInstance = Instantiate(powerup, transform.position, Quaternion.identity);
            powerupInstance.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
        }
    }

}
