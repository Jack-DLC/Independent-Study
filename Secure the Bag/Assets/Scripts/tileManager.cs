using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileManager : MonoBehaviour
{

    public GameObject[] prefab; // used to spawn in tile prefabs

    public float gridSpawnCoords = 80.0f;
    public float gridOffset = 80;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        bool has_triggered = false;
        if (other.gameObject.tag == "SpawnPoint" && has_triggered == false)
        {
            has_triggered = true;
            Debug.Log("player");
            Instantiate(prefab[0], new Vector3(0, 0, gridSpawnCoords), Quaternion.identity);
            gridSpawnCoords += gridOffset;
        }

    }
}
