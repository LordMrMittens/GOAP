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
    public bool isCloseUp {get; set;}
    [SerializeField] LayerMask layersToHide;
    [SerializeField] GameObject camLight;
    LayerMask defaultLayersToShow;
    void Start()
    {
        worldStatusManager = WorldStatusManager.WSMInstance;
        zoomAmount = transform.position.y;
        defaultLayersToShow = Camera.main.cullingMask;
        camLight.SetActive(false);
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

    public void SetCloseUpPosition( Transform pos, Transform lookAt){
        isCloseUp = true;
        camLight.SetActive(true);
        Camera.main.cullingMask = layersToHide;
        SetLastPositionAndRotation();
        transform.position = pos.transform.position;
        transform.parent = pos;
        Camera.main.transform.LookAt(lookAt);
    }

    void SetLastPositionAndRotation()
    {
        lastPos = transform.position;
        lastRotation = transform.rotation;
    }
    public void ResetLastPositionAndRotation()
    {
        Camera.main.cullingMask = defaultLayersToShow;
        camLight.SetActive(false);
        transform.parent = null;
        transform.position = lastPos;
        transform.rotation = lastRotation;
        isCloseUp = false;
        
    }
}
