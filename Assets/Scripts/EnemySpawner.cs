using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public float spawnTimer = 5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy() {
        while(true) {
            if (enemy == null) {
                Debug.LogWarning("No enemy available to spawn");
            }
            // Instantiate(enemy, transform.position, Quaternion.zero);
            yield return new WaitForSeconds(spawnTimer);
        }
    } 

}
