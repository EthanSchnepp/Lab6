using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testings : MonoBehaviour
{
    // Start is called before the first frame update
    public int size = 10;
    private Pathfinding pathfinding;
    private PathNode mainCharacter;
    private PathNode objectOfDesire;

    public GameObject mainCharacterPrefab;
    public GameObject objectOfDesirePrefab;
    public GameObject obstaclePrefab;

    private GameObject mainCharacterGO;
    private GameObject objectOfDesireGO;
    private List<GameObject> obstacles;

    private int currentPathIndex;
    private List<Vector3> pathVectorList;

    private bool isMovingAlongPath = false;
    private IEnumerator moveCoroutine;
    private void Start()
    {
       pathfinding = new Pathfinding(size, size);
       obstacles = new List<GameObject>();
        PlaceMainCharacterAndObjectOfDesire();
        PlaceObstacles();
    }

    private void Update()
    {
        if (!isMovingAlongPath)
        {

            List<PathNode> path = pathfinding.FindPath(mainCharacter, objectOfDesire);
            if (path != null)
            {
                moveCoroutine = MoveAlongPath(path);
                StartCoroutine(moveCoroutine);
            }
        }

        }
    
    private void PlaceMainCharacterAndObjectOfDesire()
    {
        Grid<PathNode> grid = pathfinding.GetGrid();
        int halfSize = (size / 2) + 1;

        do
        {
            mainCharacter = grid.GetGridObject(Random.Range(0, size), Random.Range(0, size));
            objectOfDesire = grid.GetGridObject(Random.Range(0, size), Random.Range(0, size));
        } while (CalculateDistanceCost(mainCharacter, objectOfDesire) < halfSize);
        mainCharacterGO = Instantiate(mainCharacterPrefab, new Vector3(mainCharacter.x, mainCharacter.y), Quaternion.identity);
        objectOfDesireGO = Instantiate(objectOfDesirePrefab, new Vector3(objectOfDesire.x, objectOfDesire.y), Quaternion.identity);
        Debug.Log("Main Character: " + mainCharacter);
        Debug.Log("Object of Desire: " + objectOfDesire);
    }
    private void PlaceObstacles()
    {
        Grid<PathNode> grid = pathfinding.GetGrid();
        int numObstacles = size;

        for (int i = 0; i < numObstacles; i++)
        {
            PathNode obstacleNode;
            do
            {
                obstacleNode = grid.GetGridObject(Random.Range(0, size), Random.Range(0, size));
            } while (obstacleNode == mainCharacter || obstacleNode == objectOfDesire || !obstacleNode.isWalkable);

            obstacleNode.isWalkable = false;
            GameObject obstacleGO = Instantiate(obstaclePrefab, new Vector3(obstacleNode.x, obstacleNode.y), Quaternion.identity);
            obstacles.Add(obstacleGO);
        }

        EnsureObstacleBlockingStraightLine();
    }
    private void EnsureObstacleBlockingStraightLine()
    {
        Grid<PathNode> grid = pathfinding.GetGrid();
        int xDiff = objectOfDesire.x - mainCharacter.x;
        int yDiff = objectOfDesire.y - mainCharacter.y;

        if (xDiff == 0 || yDiff == 0)
        {
            PathNode blockingNode = grid.GetGridObject(mainCharacter.x + xDiff / 2, mainCharacter.y + yDiff / 2);
            blockingNode.isWalkable = false;
            GameObject obstacleGO = Instantiate(obstaclePrefab, new Vector3(blockingNode.x, blockingNode.y), Quaternion.identity);
            obstacles.Add(obstacleGO);
        }
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        return xDistance + yDistance;
    }

    private IEnumerator MoveAlongPath(List<PathNode> path)
    {
        isMovingAlongPath = true;
        foreach (PathNode node in path)
        {
            mainCharacterGO.transform.position = new Vector3(node.x, node.y, 0);
            yield return new WaitForSeconds(.5f);

            if (node == objectOfDesire)
            {
                Debug.Log("Main Character reached the Object of Desire!");
                EndGame();
                yield break;
            }
        }
        isMovingAlongPath = false;
    }

    private void EndGame()
    {
        
        Debug.Log("Game Over: Main Character has reached the Object of Desire!");
        Time.timeScale = 0f; // Stop the game
    }
    

}