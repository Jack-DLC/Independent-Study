using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private GameObject playerPosition;
    private Vector3 cameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        cameraPosition.y = 8;
        cameraPosition.z = playerPosition.transform.position.z - 14;
        transform.position = cameraPosition;
    }
}
