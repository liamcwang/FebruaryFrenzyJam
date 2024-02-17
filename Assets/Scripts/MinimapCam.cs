using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCam : MonoBehaviour
{
    public BoxCollider2D boundary;
    public Camera cam;
    public float width;
    public float height;
    
    
    private Vector3 topRight;
    private Vector3 bottomLeft;
    // Start is called before the first frame update
    void Start()
    {
        boundary = GetComponent<BoxCollider2D>();
        cam = GetComponent<Camera>();
        topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        width = topRight.x - bottomLeft.x;
        height = topRight.y - bottomLeft.y;

        Debug.Log("Camera scaled pixel height: " + cam.pixelWidth);
        
        boundary.size = new Vector2(width, height);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D other) {
        Debug.Log("I see this: " + other);
        Transform oTransform = other.gameObject.transform;
        float xAxis = oTransform.position.x;
        float yAxis = oTransform.position.y;
        float zAxis = oTransform.position.z;

        Vector3 pos = Camera.main.WorldToViewportPoint(oTransform.position);
        
        if(pos.x < 0f) {
            xAxis = topRight.x + 5;
        }
        if(pos.x > 1f){
            xAxis = bottomLeft.x - 5;
        } 
        if(pos.y < 0f){
            yAxis = topRight.y + 5;
        } 
        if(pos.y > 1f){
            yAxis = bottomLeft.y - 5;
        }
        
        oTransform.position = new Vector3(xAxis,yAxis,zAxis);
    }
}
