using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    private static float spawnTimer;
    public static float spawnRate {get {
        return spawnTimer;
    } set {
        spawnTimer = 1/value;
    }}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private IEnumerator SpawnEnemy() {
        while(true) {
            // Instantiate(enemy, transform.position, Quaternion.zero);
            yield return new WaitForSeconds(spawnRate);
        }
    } 

}
