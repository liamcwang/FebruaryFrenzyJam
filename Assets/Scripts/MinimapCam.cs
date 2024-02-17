using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCam : MonoBehaviour
{
    public BoxCollider2D boundary;
    public Camera camera;
    public float width;
    public float height;
    private float xWrap;
    private float yWrap;
    
    private Vector3 topRight;
    private Vector3 bottomLeft;
    // Start is called before the first frame update
    void Start()
    {
        boundary = GetComponent<BoxCollider2D>();
        camera = GetComponent<Camera>();
        topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        width = topRight.x - bottomLeft.x;
        height = topRight.y - bottomLeft.y;
        
        xWrap = PlayerCam.instance.width / 2;
        yWrap = PlayerCam.instance.height / 2;

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
            xAxis = topRight.x + xWrap;
        }
        if(pos.x > 1f){
            xAxis = bottomLeft.x - xWrap;
        } 
        if(pos.y < 0f){
            yAxis = topRight.y + yWrap;
        } 
        if(pos.y > 1f){
            yAxis = bottomLeft.y - yWrap;
        }
        
        oTransform.position = new Vector3(xAxis,yAxis,zAxis);
    }
}
