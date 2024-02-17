using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float trackingFactor = 1f;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        Vector3 currentPos = transform.position;
        Vector3 heading = new Vector3();

        heading.x = playerPos.x - currentPos.x;
        heading.y = playerPos.y - currentPos.y;
        heading.z = playerPos.z - currentPos.z;

        float distanceSquared = heading.x * heading.x + heading.y * heading.y + heading.z * heading.z;
        float distance = Mathf.Sqrt(distanceSquared);
        if (distance < 100f) {
            transform.position = Vector3.Lerp(transform.position, playerPos, trackingFactor * Time.deltaTime);
        } else{
            transform.position = playerPos;
        }

        
    }
}
