using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private SpawnBalls spawnBalls;

    int currentBallColor = -1;
    int[] arrDel = null;
    int[] arrColors = null;
    GameObject[] arrBalls = null;
    GameObject curBall = null;

    // Start is called before the first frame update
    void Start()
    {
        arrColors = new int[24];
        for (int i = 0; i < 24; i++) arrColors[i] = -1;
        arrDel = new int[3];
        arrBalls = new GameObject[24];
        spawnBalls.SetLevelControl(gameObject.GetComponent<LevelControl>());
        GenerateNewBall();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BallMoveEnd(Vector3 pos)
    {
        
        int px = Mathf.RoundToInt((pos.x + 11f) / 3f);
        int py = Mathf.RoundToInt((pos.y - 1.6f) / 1.95f);
        print($"BallMoveEnd color={currentBallColor} pos={pos} px={px} py={py}");
        int index = px * 6 + py;
        arrBalls[index] = curBall;
        arrColors[index] = currentBallColor;
        if (Test3Balls(px)) Del3Balls();
        GenerateNewBall();
    }

    private void GenerateNewBall()
    {
        currentBallColor = Random.Range(0, spawnBalls.CountMaterials);
        curBall = spawnBalls.SpawnNewBall(currentBallColor);
    }

    private bool Test3Balls(int col)
    {
        bool res = false;
        for (int i = 1; i < 6; i++)
        {
            if (arrColors[6 * col + i] == arrColors[6 * col + i - 1])
            {
                if (i < 5 && arrColors[6 * col + i] == arrColors[6 * col + i + 1])
                {
                    arrDel[0] = 6 * col + i - 1;
                    arrDel[1] = 6 * col + i;
                    arrDel[2] = 6 * col + i + 1;
                    return true;
                }
            }
        }
        return res;
    }

    private void Del3Balls()
    {
        for(int i = 0; i < 3; i++)
        {
            Destroy(arrBalls[arrDel[i]]);
            arrBalls[arrDel[i]] = null;
            arrColors[arrDel[i]] = -1;
        }
    }
}
