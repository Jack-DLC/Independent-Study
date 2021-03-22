using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TileManager : MonoBehaviour
{

    public GameObject[] prefab; // a list of item prefabs

    public SceneManager currentScene; // used to reset the scene when the player dies

    // activeTileSets is a list of GameObjects that contains references to all grid sets that have 
    // been spawned into the scene.
    // grid sets contain all obstacles, floor tiles, and walls that are instantiated by GenerateTileSet()
    public List<GameObject> activeTileSets = new List<GameObject>();

    public float gridSpawnCoords = 40.0f; // the z_coords coordinate that new tiles will spawn on
    public float gridSpawnOffset = 80; // spawning offset distance


    // Start is called before the first frame update
    void Start()
    {
        GameObject startingGrid; // a game object to store and add starting grid to list activeTileSets
        startingGrid = Instantiate(prefab[6], new Vector3(0, 0, 10), Quaternion.identity);
        activeTileSets.Add(startingGrid);
        GenerateTileSet();
    }


    // detect collisions with obstacles(hurdle, wall, pitfall, and spawner)
    private void OnCollisionEnter(Collision collision)
    {
        // player loses and scene resets
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            
            SceneManager.LoadScene("MainGame"); 
        }
        // destroy the spawner and tileset behind the player then generate the next tileset
        else if (collision.gameObject.CompareTag("terrainSpawner"))
        {

            Destroy(collision.gameObject);
            DestroyTileSet();
            GenerateTileSet();
        }
    }

    
    private void GenerateTileSet()
    {
        int obstacleOne;// used to select a random obstacle
        int obstacleTwo;// used to select a random obstacle
        int obstacleThree;// used to select a random obstacle
        
        int numberOfObstacles;// used to determine how many obstacles will spawn on a single row

        
        GameObject gridSet;
        gridSet = new GameObject("GridSet") ;
        activeTileSets.Add(gridSet);

        // Spawn grid borders
        gridSet = Instantiate(prefab[0], new Vector3(0, 0, gridSpawnCoords + 25), Quaternion.identity);
        gridSet.transform.parent = activeTileSets[1].transform;

        // Spawn the gridSpawner
        gridSet = Instantiate(prefab[7], new Vector3(0, 5, gridSpawnCoords + 10), Quaternion.identity);
        gridSet.transform.parent = activeTileSets[1].transform;

        for (float z_coords = gridSpawnCoords; z_coords < gridSpawnCoords + 80; z_coords += 20)
        {
            numberOfObstacles = Random.Range(1, 4);
            //numberOfObstacles = 1;

             
            switch (numberOfObstacles)
            {
                case 1:// spawn one item, either a pitfall or a single wall or hurdle.
                    obstacleOne = GetRandomPrefab(true);
                    if (obstacleOne == 5)
                    {
                        SpawnFloorTiles(1, z_coords);
                        SpawnObstacle(5, 0, z_coords);
                        break;
                    }
                    SpawnFloorTiles(2, z_coords);
                    SpawnObstacle(obstacleOne, GetRandomLane(), z_coords);
                    break;
                case 2:// spawn two items
                    int itemOnePosition;// contains a random lane number to spawn an obstacle
                    int itemTwoPosition;// contains a random lane number to spawn an obstacle

                    SpawnFloorTiles(2, z_coords);
                    obstacleOne = GetRandomPrefab(false);
                    obstacleTwo = GetRandomPrefab(false);
                    while (true)
                    {
                        itemOnePosition = GetRandomLane();
                        itemTwoPosition = GetRandomLane();
                        if (itemTwoPosition != itemOnePosition)
                        {
                            SpawnObstacle(obstacleOne, itemOnePosition, z_coords);
                            SpawnObstacle(obstacleTwo, itemTwoPosition, z_coords);
                            break;
                        }
                    }
                    break;
                case 3:// spawn three items
                    SpawnFloorTiles(2, z_coords);
                    obstacleOne = GetRandomPrefab(false);
                    obstacleTwo = GetRandomPrefab(false);
                    obstacleThree = GetRandomPrefab(false);
                    while (true)
                    {
                        if (obstacleOne == 4 && obstacleTwo == 4 && obstacleThree == 4)
                        {
                            obstacleOne = GetRandomPrefab(false);
                            obstacleTwo = GetRandomPrefab(false);
                            obstacleThree = GetRandomPrefab(false);
                        }
                        else
                        {
                            SpawnObstacle(obstacleOne, -10, z_coords);
                            SpawnObstacle(obstacleTwo, 0, z_coords);
                            SpawnObstacle(obstacleThree, 10, z_coords);
                            break;
                        }
                    }
                    break;
            }
        }
        gridSpawnCoords += gridSpawnOffset;
    }


    // destroy the tile set behind the player and remove it from list activeTileSets
    private void DestroyTileSet()
    {
        // if there are no tiles in the scene, leave function
        if (activeTileSets.Count == 0) return;

        Destroy(activeTileSets[0]);
        activeTileSets.RemoveAt(0);
    }


    // spawn floor tile, floorSet, at position z_position
    private void SpawnFloorTiles(int floorSet, float z_position)
    {
        GameObject floor;// used to instantiate and add the floor to the tile set
        if (floorSet == 1) z_position -= 10;
        else if (floorSet == 2) z_position -= 5;
        floor = Instantiate(prefab[floorSet], new Vector3(0, 0, z_position), Quaternion.identity);
        floor.transform.parent = activeTileSets[1].transform;
    }


    // given prefab fab, this function instantiates object fab at x position lane, and z position 
    // z_position. The height the object is instantiated is determined within the function based on prefab fab.
    private void SpawnObstacle(int fab, int lane, float z_position)
    {
        GameObject obstacle;
        float obstacleHeight = 0;
        if (fab == 3) obstacleHeight = 1.0f;
        else if (fab == 4) obstacleHeight = 2.5f;

        obstacle = Instantiate(prefab[fab], new Vector3(lane, obstacleHeight, z_position), Quaternion.identity);
        obstacle.transform.parent = activeTileSets[1].transform;
    }


    // return a random x coordinate coresponding to lanes 1-3
    private int GetRandomLane()
    {
        int lane = Random.Range(1, 4);
        if (lane == 1) return -10;
        else if (lane == 2) return 0;
        else return 10;
    }


    // return a random obstacle prefab. 3 == hurdle, 4==wall, 5==pitfall.
    // bool canReturnPitfall determines if can return a pitfall or not
    private int GetRandomPrefab(bool canReturnPitfall)
    {
        if (canReturnPitfall) return Random.Range(3, 6);

        return Random.Range(3, 5);
    }


}
