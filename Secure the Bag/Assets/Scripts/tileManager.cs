using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    public GameObject[] prefab; // used to spawn in tile prefabs
    public GameObject grid; // a gameObject used to save newly instantiated grids to a list of all grids in the scene
    public GameObject obstacle; // a gameObject used to save newly instantiated obstacles to a list of all obstacles in the scene

    // activeGrids is a list of GameObjects that contains references to all grid prefabs  that have been spawned into the scene
    public List<GameObject> activeGrids = new List<GameObject>();
    // activeObstacles is a list of GameObjectsthat contains references to all grid prefabs  that have been spawned into the scene
    public List<GameObject> activeObstacles = new List<GameObject>();

    public float gridSpawnCoords = 100.0f; // the z_coords coordinate that new tiles will spawn on
    public float gridOffset = 80; // spawning offset distance


    

    // Start is called before the first frame update
    void Start()
    {

        StartingGrid();
        /*
        GameObject grid;
        grid = Instantiate(prefab[1], new Vector3(0, 0, 10), Quaternion.identity);
        activeGrids.Add(grid);
        grid = Instantiate(prefab[0], new Vector3(0, 0, gridSpawnCoords), Quaternion.identity);
        activeGrids.Add(grid);
        SpawnObstacles();
        gridSpawnCoords += gridOffset;
        //SpawnGrid();
        */
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other) // when player collides with 
    {
        if (other.gameObject.tag.Equals("terrainSpawner"))
        {
            DestroyGrid();
            SpawnGrid();
        }

    }

    private void StartingGrid()
    {
        GameObject grid;
        grid = Instantiate(prefab[1], new Vector3(0, 0, 10), Quaternion.identity);
        activeGrids.Add(grid);
        grid = Instantiate(prefab[0], new Vector3(0, 0, gridSpawnCoords), Quaternion.identity);
        activeGrids.Add(grid);
        SpawnObstacles();
        gridSpawnCoords += gridOffset;
    }

    private void SpawnGrid() // creates a grid adn populates it with obstacles
    {
        GameObject grid;
        grid = Instantiate(prefab[0], new Vector3(0, 0, gridSpawnCoords), Quaternion.identity);
        activeGrids.Add(grid);
        SpawnObstacles();
        gridSpawnCoords += gridOffset;
        //DestroyGrid();
    }


    private void DestroyGrid()
    {
        if (activeGrids.Count == 0) return;

        Destroy(activeGrids[0]);
        activeGrids.RemoveAt(0);
    }


    private void SpawnObstacles()
    {
        GameObject test;
        int rand1;
        int rand2;
        int rand3;
        int randLane;
        int somanyObjects;

        for (float z_coords = gridSpawnCoords - 10; z_coords < gridSpawnCoords + 70; z_coords += 20)
        {
            //int somanyObjects = Random.Range(1, 5);
            somanyObjects = 1;
            rand1 = ReturnPrefab();

            if (somanyObjects == 1)
            {
                test = Instantiate(prefab[rand1], new Vector3(GetRandLane(), getHeight(rand1), z_coords), Quaternion.Euler(0, 90, 0));
                test.transform.parent = activeGrids[1].transform;
            }
            else if (somanyObjects == 2)
            {
                int lane2;
                rand2 = ReturnPrefab();
                while (true)
                {
                    randLane = GetRandLane();
                    lane2 = GetRandLane();
                    if (lane2 != randLane)
                    {
                        test = Instantiate(prefab[rand1], new Vector3(randLane, getHeight(rand1), z_coords), Quaternion.Euler(0, 90, 0));
                        test.transform.parent = activeGrids[1].transform;
                        test = Instantiate(prefab[rand2], new Vector3(lane2, getHeight(rand2), z_coords), Quaternion.Euler(0, 90, 0));
                        test.transform.parent = activeGrids[1].transform;
                        return;
                    }
                }
            }

            else if (somanyObjects == 3)
            {
                // make three random prefabs, redo if there are three walls
                rand2 = ReturnPrefab();
                rand3 = ReturnPrefab();
                while (true)
                {
                    if (rand1 == rand2 && rand2 == rand3)
                    {
                        rand1 = ReturnPrefab();
                        rand2 = ReturnPrefab();
                        rand3 = ReturnPrefab();
                    }
                    else
                    {
                        test = Instantiate(prefab[rand1], new Vector3(-10, getHeight(rand1), z_coords), Quaternion.Euler(0, 90, 0));
                        test.transform.parent = activeGrids[1].transform;
                        test = Instantiate(prefab[rand2], new Vector3(0, getHeight(rand2), z_coords), Quaternion.Euler(0, 90, 0));
                        test.transform.parent = activeGrids[1].transform;
                        test = Instantiate(prefab[rand3], new Vector3(10, getHeight(rand3), z_coords), Quaternion.Euler(0, 90, 0));
                        test.transform.parent = activeGrids[1].transform;
                        return;
                    }
                }
            }
            else Debug.Log("weve reached the last else");
        }
    }


    private float getHeight(int test)
    {
        if (test == 3) return 1.0f;
        else if (test == 4) return 2.5f;
        return 0.0f;
    }


    private int GetRandLane()
    {
        //int lane = -1;
        int lane = Random.Range(1, 4);
        if (lane == 1) return -10;
        else if (lane == 2) return 0;
        else return 10;
    }


    private int ReturnPrefab()
    {
        int randPrefab = Random.Range(3, 5);
        return randPrefab;
    }


}
