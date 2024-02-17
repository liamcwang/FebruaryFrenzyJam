using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public Player.PowerUp type;
    public float value = 10f;

    // Start is called before the first frame update
    void Start()
    {

        //if (type == null) Debug.LogWarning("No powerup was set.");   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Player p = GameManager.instance.player;
            Effect newEff = new Effect(true, value);
            p.updatePowers(type, newEff);
            Destroy(gameObject);
        }
    }
}
