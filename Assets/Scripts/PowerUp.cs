using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public Player.PowerUp type;
    public Player.Effect effect;

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
        GameObject go = other.gameObject;
        if (go.GetComponent<Player>() != null) {

        }
    }
}
