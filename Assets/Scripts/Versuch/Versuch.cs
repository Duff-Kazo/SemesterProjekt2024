using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Versuch : MonoBehaviour
{
    [SerializeField] private GameObject map;
    SimpleRandomWalkSO dings;
    void Start()
    {
        dings = new SimpleRandomWalkSO();
        dings.walkLength = 15;
        dings.startRandomlyEachIteration = false;
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        roomPositions = RunRandomWalk(dings, new Vector2Int(0, 0));
        //roomPositions.Add(new Vector2Int(0, 0));
        //roomPositions.Add(new Vector2Int(0, 1));
        //roomPositions.Add(new Vector2Int(1, 0));
        //roomPositions.Add(new Vector2Int(-1, 0));
        //roomPositions.Add(new Vector2Int(-1, 1));
        foreach (var pos in roomPositions)
        {
            Instantiate(map, new Vector3(pos.x * 12, pos.y * 12, 0), Quaternion.identity);
        }
    }



    private HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int position)
    {
        var currentPosition = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < parameters.iterations; i++)
        {
            var path = ProcedualGenerationAlgorythms.SimpleRandomWalk(currentPosition, parameters.walkLength);
            floorPositions.UnionWith(path);
            if (parameters.startRandomlyEachIteration)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        return floorPositions;
    }
}
