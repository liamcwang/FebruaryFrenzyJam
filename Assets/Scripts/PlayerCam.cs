using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public const int START_JINGLE= 0, ANTIVIRUS= 1, SCANNING = 2, BOSS_THEME = 3;
    const float Z_POS = -10;
    public static PlayerCam instance;
    public float trackingFactor = 1f;
    private Player player;
    public float width;
    public float height;
    [HideInInspector]public Camera thisCamera;
    public AudioClip[] sounds;
    public AudioSource audioSaus;


    private Vector3 playerPos;
    private Vector3 currentPos;
    private Vector2 heading;
    private Vector2 previousOffset;

    void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSaus = GetComponent<AudioSource>();
        thisCamera = GetComponent<Camera>();
        player = GameManager.instance.player;
        Vector3 topRight = thisCamera.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        Vector3 bottomLeft = thisCamera.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        width = topRight.x - bottomLeft.x;
        height = topRight.y - bottomLeft.y;
       
    }

    public void PlaySound(int i) {
        audioSaus.clip = sounds[i];
        audioSaus.Play(0);
    }

    public void startUp() {
        PlaySound(START_JINGLE);
        StartCoroutine(QueueSound(audioSaus.clip.length, ANTIVIRUS));
    }

    public IEnumerator QueueSound(float time, int i) {
        yield return new WaitForSeconds(time);
        PlaySound(i);
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = new Vector3(player.transform.position.x, player.transform.position.y, Z_POS);
        currentPos = transform.position;
        heading = new Vector2();

        heading.x = playerPos.x - currentPos.x;
        heading.y = playerPos.y - currentPos.y;

        float distanceSquared = heading.x * heading.x + heading.y * heading.y;
        float distance = Mathf.Sqrt(distanceSquared);
        if (distance < 100f) {
            transform.position = Vector3.Lerp(transform.position, playerPos, trackingFactor * Time.deltaTime);
        } else{
            Vector3 newPos = new Vector3(playerPos.x - previousOffset.x, playerPos.y - previousOffset.y, Z_POS);
            transform.position = newPos;
        }

        previousOffset = new Vector2(heading.x, heading.y);

        
    }
}
