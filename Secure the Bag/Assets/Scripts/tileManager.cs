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
            GridSpawner();
            ObstacleSpawner();
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

    // look into creating a class of game objects that handles creating 
    private void GridSpawner() // creates a grid adn populates it with obstacles
    {
        GameObject grid;
        grid = Instantiate(prefab[0], new Vector3(0, 0, gridSpawnCoords), Quaternion.identity);
        gridSpawnCoords += gridOffset;
        activeGrids.Add(grid);
        DestroyGrid();
    }

    // create a class of obstacles and add objects to that class when instantiated
    // or set obstacles as children of their grid and 
    private void ObstacleSpawner() // controls logic behind obstacle spawning
    {
        GameObject obstacle; // a GameObject used to add instantiated obstacles to a list of obstacles in the scene

        int numObstacles; // number of obstacles that will be spawned into a lane
        int randomObstacle; // used to determine what o0bstacle prefab will be instantiated
        int objectsinRow; // track the number of objects in a row 
        int numWalls; // tracks the number of walls in a row 
        float height; // Y value used to spawn in different obstacles
        bool spawnObject; // if true, an obstacle will spawn in this lane


        int left_lane= -10;
        int center_lane= 0;
        int right_lane= 10;

        //DestroyObstacles(); // clear scene before spawning new obstacles

        for (float i = gridSpawnCoords - 10; i < gridSpawnCoords + 70; i += 20)
        // itterates through every other row and determines how many items to spawn, between 0 and 3 or a pittfall, in the row
        // the center of the spawner prefab is located on the second row so we start 10 units before
        {
            objectsinRow = 0;

            // In future itteerations try and bias the algorithm towards more objects the further the player gets
            //numObstacles = Random.Range(1, 5); // randomly decide the number of objects to spawn
            numObstacles = 1;

            switch (numObstacles) // handles item spawn cases
            {
                case 0: // no obstacles are spawned
                    break;

                case 1: // one obstacle is spawned in a random lane
                    // pick a random number between one of three and spawn an object in that lane

                    int randLane = Random.Range(1, 4);
                    randomObstacle = Random.Range(3, 5);
                    if (randomObstacle == 3) height = 1.0f;
                    else height = 2.5f;

                    if (randLane == 1) obstacle = Instantiate(prefab[randomObstacle], new Vector3(left_lane, height, i), Quaternion.Euler(0, 90, 0));
                    else if (randLane == 2) obstacle = Instantiate(prefab[randomObstacle], new Vector3(center_lane, height, i), Quaternion.Euler(0, 90, 0));
                    else obstacle = Instantiate(prefab[randomObstacle], new Vector3(right_lane, height, i), Quaternion.Euler(0, 90, 0));

                    activeObstacles.Add(obstacle);

                    /*
                    for (int j = -10; j <= 10; j += 10)
                        // go iterate through
                    {
                        if (objectsinRow == 1) break;

                        // decide if an obstacle should spawn and what obstacle to spawn
                        randomObstacle = Random.Range(3, 5);
                        spawnObject = (Random.value > 0.5f);


                        // set the height of the obstacle
                        if (randomObstacle == 3) height = 1.0f;
                        else height = 2.5f;

                        if (spawnObject )
                        {
                            objectsinRow++;
                            obstacle = Instantiate(prefab[randomObstacle], new Vector3(j, height, i), Quaternion.Euler(0, 90, 0));

                            activeObstacles.Add(obstacle);
                            //totalNumObj++;
                        }

                        // spawn obstacle if we reach the third row and no obstacles have been generated
                        if (j == 10 && objectsinRow == 0) 
                        {
                            obstacle = Instantiate(prefab[randomObstacle], new Vector3(j, height, i), Quaternion.Euler(0, 90, 0));
                            activeObstacles.Add(obstacle);
                            objectsinRow++;
                            break;
                        }
                    }
                    */
                    break;

                // two obstacles are spawned in random lanes
                case 2: 
                    // pick two numbers and place objects on that 
                    for (int j = -10; j <= 10; j += 10)
                    {
                        if (objectsinRow == 2) break; // if ther are already two objects then exit loop

                        // decide if an obstacle should spawn and what obstacle to spawn
                        randomObstacle = Random.Range(3, 5);
                        spawnObject = (Random.value > 0.5f);

                        // set the height of the obstacle
                        if (randomObstacle == 3) height = 1.0f;
                        else height = 2.5f;

                        if (spawnObject)
                        {
                            objectsinRow++;
                            obstacle = Instantiate(prefab[randomObstacle], new Vector3(j, height, i), Quaternion.Euler(0, 90, 0));
                            activeObstacles.Add(obstacle);
                        }

                        // spawn obstacle if we reach the third row and no obstacles have been generated
                        if (j == 10 && objectsinRow != 2)
                        {
                            obstacle = Instantiate(prefab[randomObstacle], new Vector3(j, height, i), Quaternion.Euler(0, 90, 0));
                            activeObstacles.Add(obstacle);
                            objectsinRow++;
                            break;
                        }
                    }
                    break;

                case 3:// three obstacles are spawned in random lanes
                    numWalls = 0;
                    for (int j = -10; j <= 10; j += 10)
                    {
                        randomObstacle = Random.Range(3, 5);
                        if (randomObstacle == 3) height = 1.0f;
                        else height = 2.5f;

                        switch (randomObstacle)// handels 
                        {
                            case 3:
                                obstacle = Instantiate(prefab[randomObstacle], new Vector3(j, height, i), Quaternion.Euler(0, 90, 0));
                                activeObstacles.Add(obstacle);
                                break;
                            case 4:
                                if (numWalls == 2) // if there are already two walls, spawn a hurdle and exit loop
                                {
                                    obstacle = Instantiate(prefab[3], new Vector3(j, y:1, i), Quaternion.Euler(0, 90, 0));
                                    activeObstacles.Add(obstacle);
                                    break;
                                }
                                obstacle = Instantiate(prefab[randomObstacle], new Vector3(j, height, i), Quaternion.Euler(0, 90, 0));
                                activeObstacles.Add(obstacle);
                                numWalls++;
                                break;
                        }
                    }
                    break;
                case 4:
                    // pick a lane to spawn a pitfall and ensure that no other items can spawn in the lane
                    Debug.Log("-------pifall-------");
                    break;
            }
        }
    }
}
