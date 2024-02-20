using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns enemy on itself
/// Make a prefab for this
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public float spawnTimer = 5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    // TODO: Contemplate making a static public method for spawning enemies anywhere
    private IEnumerator SpawnEnemy() {
        while(true) {
            if (enemy == null) {
                Debug.LogWarning("No enemy available to spawn");
            }
            Instantiate(enemy, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnTimer);
        }
    } 

}
