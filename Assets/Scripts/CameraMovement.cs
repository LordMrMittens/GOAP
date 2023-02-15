using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float speed=1;
    [SerializeField] float zoomSpeed=3;
    float zoomAmount;
    void Start()
    {
        zoomAmount = transform.position.y;
    }
    void Update()
    {
     float xAxis=  Input.GetAxis("Vertical");
     float zAxis=  Input.GetAxis("Horizontal");
     zoomAmount += (Input.GetAxis("Mouse ScrollWheel")*zoomSpeed);
//to clamp
     transform.position= new Vector3(transform.position.x +(xAxis*speed), zoomAmount,transform.position.z +(-zAxis*speed));
    }
}
