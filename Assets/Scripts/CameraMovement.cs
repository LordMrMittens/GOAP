using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    WorldStatusManager worldStatusManager;
    [SerializeField] float speed=1;
    [SerializeField] float zoomSpeed=3;
    float zoomAmount;
    public Vector3 lastPos {get;set;}
    public Quaternion lastRotation {get;set;}
    bool isCloseUp;
    [SerializeField] LayerMask layersToHide;
    LayerMask defaultLayersToShow;
    void Start()
    {
        worldStatusManager = WorldStatusManager.WSMInstance;
        zoomAmount = transform.position.y;
        defaultLayersToShow = Camera.main.cullingMask;
    }
    void Update()
    {
        if (!isCloseUp)
        {
            float xAxis = Input.GetAxis("Vertical");
            float zAxis = Input.GetAxis("Horizontal");
            zoomAmount += (Input.GetAxis("Mouse ScrollWheel") * zoomSpeed);
            //to clamp
            transform.position = new Vector3(transform.position.x + (xAxis * speed), zoomAmount, transform.position.z + (-zAxis * speed));
        }
    }

    public void SetCloseUpPosition( Vector3 pos, Transform lookAt){
        isCloseUp = true;
        Camera.main.cullingMask = layersToHide;
        SetLastPositionAndRotation();
        transform.position = pos;
        Camera.main.transform.LookAt(lookAt);
        worldStatusManager.timeSpeed = 0;
    }

    void SetLastPositionAndRotation()
    {
        lastPos = transform.position;
        lastRotation = transform.rotation;
    }
    public void ResetLastPositionAndRotation()
    {
         Camera.main.cullingMask = defaultLayersToShow;
        worldStatusManager.timeSpeed = 1;
        transform.position = lastPos;
        transform.rotation = lastRotation;
        isCloseUp = false;
        
    }
}
