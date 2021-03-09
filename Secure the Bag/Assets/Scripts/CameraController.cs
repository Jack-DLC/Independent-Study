using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private GameObject playerPosition;
    private Vector3 cameraPosition;
    //private Vector3 cameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(new Vector3(16, 0, 0));
        //Vector3 cameraOffset = new Vector3(0, 5, playerPosition.transform.position.z);
        //transform.LookAt(cameraPosition);
        


        cameraPosition.y = 8;
        cameraPosition.z = playerPosition.transform.position.z - 14;
        transform.position = cameraPosition;
    }
}
