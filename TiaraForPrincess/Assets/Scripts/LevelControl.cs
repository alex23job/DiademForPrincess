using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private SpawnBalls spawnBalls;

    int currentBallColor = -1;

    // Start is called before the first frame update
    void Start()
    {
        spawnBalls.SetLevelControl(gameObject.GetComponent<LevelControl>());
        GenerateNewBall();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BallMoveEnd(Vector3 pos)
    {
        print($"BallMoveEnd color={currentBallColor} pos={pos}");


        GenerateNewBall();
    }

    private void GenerateNewBall()
    {
        currentBallColor = Random.Range(0, spawnBalls.CountMaterials);
        spawnBalls.SpawnNewBall(currentBallColor);
    }
}
