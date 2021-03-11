using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileManager : MonoBehaviour
{

    public GameObject[] prefab; // used to spawn in tile prefabs
    public GameObject grid; // a gameObject used to save newly instantiated grids to a list of all grids in the scene
    public GameObject obstacle; // a gameObject used to save newly instantiated obstacles to a list of all obstacles in the scene

    // activeGrids is a list of GameObjects that contains references to all grid prefabs  that have been spawned into the scene
    public List<GameObject> activeGrids = new List<GameObject>();
    // activeObstacles is a list of GameObjectsthat contains references to all grid prefabs  that have been spawned into the scene
    public List<GameObject> activeObstacles = new List<GameObject>();

    public float gridSpawnCoords = 100.0f; // the z coordinate that new tiles will spawn on
    public float gridOffset = 80; // spawning offset distance

    // Start is called before the first frame update
    void Start()
    {
        // spawn two
        GameObject grid;
        grid = Instantiate(prefab[1], new Vector3(0, 0, 10), Quaternion.identity);
        activeGrids.Add(grid);
        grid = Instantiate(prefab[0], new Vector3(0, 0, gridSpawnCoords), Quaternion.identity);
        activeGrids.Add(grid);

        ObstacleSpawner();
        gridSpawnCoords += gridOffset;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) // when player collides with 
    {
        if (other.gameObject.tag.Equals("terrainSpawner"))
        {
            //DestroyObstacles();
            ObstacleSpawner();
            GridSpawner();            
        }

    }


    private void DestroyGrid()
    {
        if (activeGrids.Count == 0)
        {
            return;
        }
        Destroy(activeGrids[0]);
        activeGrids.RemoveAt(0);
    }


    private void DestroyObstacles()
    {
        if (activeObstacles.Count == 0)
        {
            return;
        }

        GameObject[] objects = GameObject.FindGameObjectsWithTag("shortBlock");
        Debug.Log(objects.Length);
        /*
        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(activeObstacles[i]);
            activeObstacles.RemoveAt(i);
        }
        */
        //activeGrids.RemoveRange(0,7);
    }


    private void GridSpawner() // creates a grid adn populates it with obstacles
    {
        GameObject grid;
        grid = Instantiate(prefab[0], new Vector3(0, 0, gridSpawnCoords), Quaternion.identity);
        gridSpawnCoords += gridOffset;
        activeGrids.Add(grid);
        DestroyGrid();
    }


    private void ObstacleSpawner() // controls logic behind obstacle spawning
    {
        GameObject obstacle; // a GameObject used to add instantiated obstacles to a list of obstacles in the scene

        int numObstacles; // number of obstacles that will be spawned into a lane
        int randomObstacle; // used to determine what obstacle will be instantiated
        int objectsinRow; // keeps track of the number of obstacles in a row 
        int numWalls = 0; // tracks the number of walls in a row 

        bool spawnObject = false; // if true an obstacle will spawn on the lane

        //DestroyObstacles();

        for (float i = gridSpawnCoords - 10; i < gridSpawnCoords + 70; i += 20)// spawns in an obstacle on each row
        {
            //Debug.Log(gridSpawnCoords);
            objectsinRow = 0;
            numObstacles = Random.Range(1, 5);

            switch (numObstacles)
            {
                case 0: // no obstacles are spawned
                    break;

                case 1: // one obstacle is spawned in a random lane
                    Debug.Log("one obstacle");
                    for (int j = -10; j <= 10; j += 10)
                    {
                        randomObstacle = Random.Range(3, 5);
                        spawnObject = (Random.value > 0.5f);
                        //if (objectsinRow == 1) break;

                        if (spawnObject && objectsinRow == 0)
                        {
                            switch (randomObstacle)
                            {
                                case 3:
                                    objectsinRow++;
                                    obstacle = Instantiate(prefab[randomObstacle], new Vector3(j, 1, i), Quaternion.Euler(0, 90, 0));
                                    activeObstacles.Add(obstacle);
                                    break;
                                case 4:
                                    objectsinRow++;
                                    obstacle = Instantiate(prefab[randomObstacle], new Vector3(j, 2.5f, i), Quaternion.Euler(0, 90, 0));
                                    activeObstacles.Add(obstacle);
                                    break;
                            }

                        }
                        
                        if(j == 10 && objectsinRow == 0)
                        {
                            obstacle = Instantiate(prefab[3], new Vector3(j, 1, i), Quaternion.Euler(0, 90, 0));
                            activeObstacles.Add(obstacle);
                            objectsinRow++;
                        }
                    }
                    break;
                    
                case 2: // two obstacles are spawned in random lanes
                    //Debug.Log("Two obstacles");
                    for (int j = -10; j <= 10; j += 10)
                    {
                        randomObstacle = Random.Range(3, 5);
                        spawnObject = (Random.value > 0.5f);

                        if (spawnObject && objectsinRow < 2)
                        {
                            switch (randomObstacle)
                            {
                                case 3:
                                    objectsinRow++;
                                    obstacle = Instantiate(prefab[randomObstacle], new Vector3(j, 1, i), Quaternion.Euler(0, 90, 0));
                                    activeObstacles.Add(obstacle);
                                    break;
                                case 4:
                                    objectsinRow++;
                                    obstacle = Instantiate(prefab[randomObstacle], new Vector3(j, 2.5f, i), Quaternion.Euler(0, 90, 0));
                                    activeObstacles.Add(obstacle);
                                    break;
                            }

                        }

                        if (j == 10 && objectsinRow <= 1)
                        {
                            obstacle = Instantiate(prefab[3], new Vector3(j, 1, i), Quaternion.Euler(0, 90, 0));
                            activeObstacles.Add(obstacle);
                            objectsinRow++;
                        }
                    }
                    break;

                case 3:// three obstacles are spawned in random lanes
                    Debug.Log("Three obstacles ");

                    for (int j = -10; j <= 10; j += 10)
                    {
                        randomObstacle = Random.Range(3, 5);

                        switch (randomObstacle)
                        {
                            case 3:
                                obstacle = Instantiate(prefab[randomObstacle], new Vector3(j, 1, i), Quaternion.Euler(0, 90, 0));
                                activeObstacles.Add(obstacle);
                                break;
                            case 4:
                                if (numWalls == 2) // if there are already two walls we spawn a hurdle
                                {
                                    obstacle = Instantiate(prefab[3], new Vector3(j, 1, i), Quaternion.Euler(0, 90, 0));
                                    activeObstacles.Add(obstacle);
                                    break;
                                }

                                obstacle = Instantiate(prefab[randomObstacle], new Vector3(j, 2.5f, i), Quaternion.Euler(0, 90, 0));
                                activeObstacles.Add(obstacle);
                                numWalls++;
                                break;
                        } 
                    }
                    break;
                case 4:

                    //Debug.Log("pifall");
                    break;
            }
        }
    }
}
