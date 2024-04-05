using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Follows the player, has a listener on it, so it also plays sounds
/// Put this on the camera that follows the player
/// </summary>
public class PlayerCam : MonoBehaviour
{
    public const int START_JINGLE= 0, ANTIVIRUS= 1, SCANNING = 2, VIRUS_DETECTED = 3, BOSS_THEME = 4, VICTORY = 5;
    const float Z_POS = -10;
    public static PlayerCam instance;
    public float trackingFactor = 1f;
    [SerializeField] private float yTrackOffset; 
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
        GameManager.instance.playerCam = this;

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
        // I don't know why this doesn't work
        /*
        foreach (AudioClip clip in sounds) {
            bool fail = clip.LoadAudioData();
            if (fail) {
                Debug.LogWarning($"Failed to load Audio clip: {clip}");
            }
        }*/
       
    }
    
    // Update is called once per frame
    void Update()
    {
        // REMINDER: Fast distance calculation
        playerPos = new Vector3(player.transform.position.x, player.transform.position.y + yTrackOffset, Z_POS);
        currentPos = transform.position;
        heading = new Vector2();

        heading.x = playerPos.x - currentPos.x;
        heading.y = playerPos.y - currentPos.y;

        float distanceSquared = heading.x * heading.x + heading.y * heading.y;
        float distance = Mathf.Sqrt(distanceSquared); // yes, this distance calculation is faster, apparently
        if (distance < 100f) {
            transform.position = Vector3.Lerp(transform.position, playerPos, trackingFactor * Time.deltaTime);
        } else{
            Vector3 newPos = new Vector3(playerPos.x - previousOffset.x, playerPos.y - previousOffset.y, Z_POS);
            transform.position = newPos;
        }

        previousOffset = new Vector2(heading.x, heading.y);

        
    }

    /// <summary>
    /// Plays sound with the audio source
    /// Done with integers because I was in a rush
    /// </summary>
    /// <param name="i"></param>
    public void PlaySound(int i) {
        // TODO: Change these to clips, not integers?
        audioSaus.clip = sounds[i]; // RESEARCH: Pitfalls of audiosources
        audioSaus.Play(0);
    }

    /// <summary>
    /// This is the startup sequence for sounds.
    /// I was in a rush.
    /// </summary>
    public void startUp() {
        PlaySound(START_JINGLE);
        StartCoroutine(QueueSound(audioSaus.clip.length, ANTIVIRUS));
    }

    /// <summary>
    /// When play sound isn't enough.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    public IEnumerator QueueSound(float time, int i) {
        yield return new WaitForSeconds(time);
        PlaySound(i);
    }

    /// <summary>
    /// Sound sequence for the boss
    /// </summary>
    public void BossTime() {
        PlaySound(VIRUS_DETECTED);
        StartCoroutine(QueueSound(audioSaus.clip.length, BOSS_THEME));
        audioSaus.loop = true;
    }

    /// <summary>
    /// Sound sequence for victory
    /// </summary>
    public void VictoryTheme() {
        PlaySound(VICTORY);
        audioSaus.loop = false;
    }

    
}

public static class CameraEx
{
    public static bool IsObjectVisible(this UnityEngine.Camera @this, Renderer renderer)
    {
        return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(@this), renderer.bounds);
    }
}