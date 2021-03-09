using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileManager : MonoBehaviour
{

    public GameObject[] prefab; // used to spawn in tile prefabs
    public GameObject grid; // a gameObject used to save newly instantiated grids to a list of all grids in the scene

    // activeGrids is a list of GameObjects that contains references to all grid prefabs  that have been spawned into the scene
    public List<GameObject> activeGrids = new List<GameObject>();
    // activeObstacles is a list of GameObjectsthat contains references to all grid prefabs  that have been spawned into the scene
    public List<GameObject> activeObstacles = new List<GameObject>();

    public float gridSpawnCoords = 80.0f; // the z coordinate that new tiles will spawn on
    public float gridOffset = 80; // spawning offset distance

    // Start is called before the first frame update
    void Start()
    {
        GameObject grid;
        grid = Instantiate(prefab[1], new Vector3(0, 0, 10), Quaternion.identity);
        activeGrids.Add(grid);
        grid = Instantiate(prefab[0], new Vector3(0, 0, gridSpawnCoords), Quaternion.identity);
        obstacleSpawner();
        gridSpawnCoords += gridOffset;
        activeGrids.Add(grid);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //GameObject grid;
        //bool has_triggered = false;

        if (other.gameObject.tag == "SpawnPoint" )
        {
            //has_triggered = true;
            //Debug.Log("player");

            gridSpawner();
            obstacleSpawner();
        }

    }

    private void destroyGrid()
    {
        Destroy(activeGrids[0]);
        activeGrids.RemoveAt(0);
    }

    private void gridSpawner() // creates a grid adn populates it with obstacles
    {
        GameObject grid;
        grid = Instantiate(prefab[0], new Vector3(0, 0, gridSpawnCoords), Quaternion.identity);
        gridSpawnCoords += gridOffset;
        activeGrids.Add(grid);
        destroyGrid();
    }

    private void obstacleSpawner() // controls logic behind obstacle spawning
    {
        GameObject obstacles;
        for (float i = gridSpawnCoords - 10; i < gridSpawnCoords + 70; i += 10)
        {
            obstacles = Instantiate(prefab[3], new Vector3(0, 0, i), Quaternion.Euler(0, 90, 0));
            activeObstacles.Add(obstacles);

        }
    }
}
