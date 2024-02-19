using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Give this to main camera
/// Manages the play area that the game takes place in
/// Also acts as a minimap for the important objects in the game
/// </summary>
public class MinimapCam : MonoBehaviour
{
    // TODO: seamless world problem
    public BoxCollider2D boundary;
    [HideInInspector] public Camera thisCamera;
    public float width;
    public float height;
    public GameObject towerPrefab;
    public GameObject enemySpawner;
    public LayerMask ignoreLayer;
    private float xWrap;
    private float yWrap;
    
    private Vector3 topRight;
    private Vector3 bottomLeft;

    void Awake() {
        GameManager.instance.minimapCam = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        boundary = GetComponent<BoxCollider2D>();
        thisCamera = GetComponent<Camera>();
        topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        width = topRight.x - bottomLeft.x;
        height = topRight.y - bottomLeft.y;
        
        // these are used when deciding how to wrap the player across the screen
        xWrap = PlayerCam.instance.width / 2;
        yWrap = PlayerCam.instance.height / 2;

        boundary.size = new Vector2(width, height);

        PlaceTowers();
        // all the towers are placed at the start of the game
    }

    /// <summary>
    /// Method to randomly spawn towers, attached to this camera
    /// because the camera also sets the boundaries of the play area.
    /// </summary>
    public void PlaceTowers() {
        for (int i = 0; i < 15; i++) {
            bool invalidPlace = true;
            int loopCounter = 0;
            while(invalidPlace) {
                float xRand = Random.value;
                float yRand = Random.value;

                

                Vector3 randPos = new Vector3(xRand, yRand, Camera.main.nearClipPlane);
                randPos = GetComponent<Camera>().ViewportToWorldPoint(randPos);
                Vector2 boxSize = new Vector2(5f, 10f);
                Debug.Log(randPos);
                RaycastHit2D hit = Physics2D.BoxCast(randPos, boxSize, 0f, Vector2.up, 0f, ~ignoreLayer);
                Debug.Log(hit.collider);
                if (hit.collider == null) {
                    //if (hit.collider.tag != "Tower") {
                    GameObject newTower = Instantiate(towerPrefab, randPos, Quaternion.identity);
                    Instantiate(enemySpawner, randPos, Quaternion.identity);
                    Tower towerComp = newTower.GetComponent<Tower>();
                    int mode = i % 3;
                    towerComp.setMode(mode);
                    invalidPlace = false;
                }            
                if (loopCounter >= 100) {
                    Debug.LogWarning("Couldn't place tower after 100 times!");
                    invalidPlace = false;
                }
                loopCounter++;
            }
        }
    }



    private void OnTriggerExit2D(Collider2D other) {
        // Debug.Log("I see this: " + other);
        if (other.gameObject.GetComponent<Projectile>() == null) {
            Transform oTransform = other.gameObject.transform;
            float xAxis = oTransform.position.x;
            float yAxis = oTransform.position.y;
            float zAxis = oTransform.position.z;

            Vector3 pos = Camera.main.WorldToViewportPoint(oTransform.position);
            
            // all of this is quite unreliable at times :(
            if(pos.x < 0.01f) {
                xAxis = topRight.x + xWrap;
            }
            if(pos.x > 0.99f){
                xAxis = bottomLeft.x - xWrap;
            } 
            if(pos.y < 0.01f){
                yAxis = topRight.y + yWrap;
            } 
            if(pos.y  > 0.99f){
                yAxis = bottomLeft.y - yWrap;
            }
            
            oTransform.position = new Vector3(xAxis,yAxis,zAxis);
        }else {
            Destroy(other.gameObject);
        }
    } 
}
