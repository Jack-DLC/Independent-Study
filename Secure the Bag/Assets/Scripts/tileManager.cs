using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TileManager : MonoBehaviour
{

    public GameObject[] prefab; // used to spawn in tile prefabs

    public SceneManager currentScene;

    // activeTileSets is a list of GameObjects that contains references to all grid prefabs  that have been spawned into the scene
    public List<GameObject> activeTileSets = new List<GameObject>();

    public float gridSpawnCoords = 40.0f; // the z_coords coordinate that new tiles will spawn on
    public float gridOffset = 80; // spawning offset distance


    // Start is called before the first frame update
    void Start()
    {
        //= new GameObject();
        GameObject startingGrid; 
        startingGrid = Instantiate(prefab[6], new Vector3(0, 0, 10), Quaternion.identity);
        activeTileSets.Add(startingGrid);
        GenerateTileSet();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y-2, transform.position.z), transform.TransformDirection(Vector3.forward), out hit, 5, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - 2, transform.position.z), transform.TransformDirection(Vector3.forward) * 5, Color.white);
            Debug.Log("Did not Hit");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene("MainGame");
        }
        else if (collision.gameObject.CompareTag("terrainSpawner"))
        {
            Destroy(collision.gameObject);
            DestroyTileSet();
            GenerateTileSet();
        }
    }

    
    private void GenerateTileSet()
    {
        int obstacleOne;
        int obstacleTwo;
        int obstacleThree;
        int numberOfObstacles;

        int spawnPositionOne; // ontains a random lane number to spawn an obstacle
        int spawnPositionTwo; // contains a random lane number to spawn an obstacle

        GameObject gridSet;
        gridSet = new GameObject("GridSet") ;
        activeTileSets.Add(gridSet);

        gridSet = Instantiate(prefab[0], new Vector3(0, 0, gridSpawnCoords + 25), Quaternion.identity);
        gridSet.transform.parent = activeTileSets[1].transform;

        gridSet = Instantiate(prefab[7], new Vector3(0, 5, gridSpawnCoords + 10), Quaternion.identity);
        gridSet.transform.parent = activeTileSets[1].transform;

        for (float z_coords = gridSpawnCoords; z_coords < gridSpawnCoords + 80; z_coords += 20)
        {
            numberOfObstacles = Random.Range(1, 5);

            obstacleOne = ReturnPrefab();
            switch (numberOfObstacles)
            {
                case 1:
                    SpawnFloorTiles(2, z_coords);
                    SpawnObstacle(obstacleOne, GetRandLane(), z_coords);
                    break;
                case 2:
                    SpawnFloorTiles(2, z_coords);
                    obstacleTwo = ReturnPrefab();
                    while (true)
                    {
                        spawnPositionOne = GetRandLane();
                        spawnPositionTwo = GetRandLane();
                        if (spawnPositionTwo != spawnPositionOne)
                        {
                            SpawnObstacle(obstacleOne, spawnPositionOne, z_coords);
                            SpawnObstacle(obstacleTwo, spawnPositionTwo, z_coords);
                            break;
                        }
                    }
                    break;
                case 3:
                    SpawnFloorTiles(2, z_coords);
                    obstacleTwo = ReturnPrefab();
                    obstacleThree = ReturnPrefab();
                    while (true)
                    {
                        if (obstacleOne == 4 && obstacleTwo == 4 && obstacleThree == 4)
                        {
                            obstacleOne = ReturnPrefab();
                            obstacleTwo = ReturnPrefab();
                            obstacleThree = ReturnPrefab();
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
                case 4:
                    SpawnFloorTiles(1, z_coords);
                    SpawnObstacle(5, 0, z_coords);
                    break;
            }
        }
        gridSpawnCoords += gridOffset;
    }


    private void DestroyTileSet()
    {
        if (activeTileSets.Count == 0) return;

        Destroy(activeTileSets[0]);
        activeTileSets.RemoveAt(0);
    }


    private void SpawnFloorTiles(int floorSet, float z_position)
    {
        GameObject tileSet;
        if (floorSet == 1) z_position -= 10;
        else if (floorSet == 2) z_position -= 5;
        tileSet = Instantiate(prefab[floorSet], new Vector3(0, 0, z_position), Quaternion.identity);
        tileSet.transform.parent = activeTileSets[1].transform;
    }


    private void SpawnObstacle(int fab, int lane, float z_position)// generate obstacle
    {
        GameObject obstacle;
        float height = 0;
        if (fab == 3) height = 1.0f;
        else if (fab == 4) height = 2.5f;

        obstacle = Instantiate(prefab[fab], new Vector3(lane, height, z_position), Quaternion.identity);
        obstacle.transform.parent = activeTileSets[1].transform;
    }


    private int GetRandLane()
    {
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
