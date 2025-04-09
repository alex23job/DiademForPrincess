using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayControl : MonoBehaviour
{
    [SerializeField] private GameObject rectPrefab;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private Material[] arrMaterials;

    private int[] arrColor = null;
    private GameObject[] arrRect = null;
    private GameObject[] arrPlatform = null;
    // Start is called before the first frame update
    void Start()
    {
        arrColor = new int[18];
        arrRect = new GameObject[18];
        arrPlatform = new GameObject[18];
        GenerateWay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateWay()
    {
        Vector3 pos = new Vector3(0, -0.2f, 0);
        Vector3 rotate = new Vector3(-90f, 0, 0);
        MeshRenderer mr;
        for (int i = 0; i < 18; i++)
        {
            pos.x = -4.9f + 2 * (i / 3);
            pos.z = -2f + 2 * (i % 3);
            GameObject rect = Instantiate(rectPrefab);
            mr = rect.GetComponent<MeshRenderer>();
            arrColor[i] = Random.Range(0, arrMaterials.Length);
            mr.materials = new Material[] { arrMaterials[arrColor[i]] };
            rect.transform.parent = transform;
            rect.transform.localPosition = pos;
            rect.transform.localRotation = Quaternion.Euler(rotate);
            arrRect[i] = rect;
        }
    }
}
