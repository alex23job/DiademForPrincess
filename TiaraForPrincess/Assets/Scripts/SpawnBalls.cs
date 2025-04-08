using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBalls : MonoBehaviour
{
    [SerializeField] private GameObject prefabBall;
    [SerializeField] private Material[] arrMat;

    LevelControl levelControl;

    public int CountMaterials { get { return arrMat.Length; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject SpawnNewBall(int numColor)
    {
        if (numColor < 0 || numColor > arrMat.Length - 1) return null;
        GameObject ball = Instantiate(prefabBall, transform.position, Quaternion.identity);
        ball.GetComponent<BallMovement>().SetLevelControl(levelControl);
        MeshRenderer mr = ball.GetComponent<MeshRenderer>();
        mr.materials = new Material[] { arrMat[numColor] };
        return ball;
    }

    public void SetLevelControl(LevelControl lc)
    {
        levelControl = lc;
    }
}
