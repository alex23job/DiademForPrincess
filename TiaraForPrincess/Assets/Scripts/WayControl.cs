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
    private List<int> posBonus = null;
    // Start is called before the first frame update
    void Start()
    {
        arrColor = new int[18];
        arrRect = new GameObject[18];
        arrPlatform = new GameObject[18];
        posBonus = new List<int>();
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
            rect.transform.localScale = new Vector3(100f, 100f, 100f);
            arrRect[i] = rect;
        }
    }

    public void SetBonus(GameObject bonusPrefab)
    {
        Vector3 pos = new Vector3(0, 1f, 0);
        List<int> candidat = new List<int>() { 3, 5, 6, 8, 9, 11, 12, 14};
        int numPos = Random.Range(0, candidat.Count);
        while (posBonus.Contains(candidat[numPos]))
        {
            candidat.RemoveAt(numPos);
            if (candidat.Count == 0) return;
            numPos = Random.Range(0, candidat.Count);
        }
        GameObject bonus = Instantiate(bonusPrefab);
        bonus.transform.parent = transform;
        pos.x = -4.9f + 2 * (candidat[numPos] / 3);
        pos.z = -2f + 2 * (candidat[numPos] % 3);
        bonus.transform.localPosition = pos;
        posBonus.Add(candidat[numPos]);
    }

    public bool GeneratePlatform(int numCol)
    {
        // список номеров €чеек с цветом из параметра функции
        List<int> candidat = new List<int>();
        int i, numCnd;
        for (i = 0; i < 18; i++)
        {
            if (arrColor[i] == numCol && arrPlatform[i] == null) candidat.Add(i);
        }
        for(i = 0; i < posBonus.Count; i++)
        {   //  пробуем создать платформу под бонусом если р€дом уже есть платформы 
            if (candidat.Contains(posBonus[i]))
            {
                numCnd = posBonus[i];
                if ((arrPlatform[numCnd - 3] != null) || (arrPlatform[numCnd + 3] != null))   //  в предидущем или следующем р€ду есть платформа
                {
                    CreatePlatform(numCnd, numCol);
                    return true;
                }
                if (((numCnd % 3) < 2) && (arrPlatform[numCnd + 1] != null))    //  справа есть платформа
                {
                    CreatePlatform(numCnd, numCol);
                    return true;
                }
                if (((numCnd % 3) > 0) && (arrPlatform[numCnd - 1] != null))    //  слева есть платформа
                {
                    CreatePlatform(numCnd, numCol);
                    return true;
                }
            }
        }
        for (i = 0; i < candidat.Count; i++)
        {
            numCnd = candidat[i];
            if (numCnd < 3)
            {
                CreatePlatform(numCnd, numCol);
                return true;
            }
            else
            {
                if (arrPlatform[numCnd - 3] != null)   //  в предидущем р€ду есть платформа
                {
                    CreatePlatform(numCnd, numCol);
                    return true;
                }
                if (((numCnd % 3) < 2) && (arrPlatform[numCnd + 1] != null))    //  справа есть платформа
                {
                    CreatePlatform(numCnd, numCol);
                    return true;
                }
                if (((numCnd % 3) > 0) && (arrPlatform[numCnd - 1] != null))    //  слева есть платформа
                {
                    CreatePlatform(numCnd, numCol);
                    return true;
                }
            }
        }
        return false;
    }

    private void CreatePlatform(int num, int numCol)
    {
        Vector3 pos = new Vector3(0, -0.1f, 0);
        Vector3 rotate = new Vector3(-90f, 0, 0);
        MeshRenderer mr;
        GameObject platform = Instantiate(platformPrefab);
        mr = platform.GetComponent<MeshRenderer>();
        mr.materials = new Material[] { arrMaterials[numCol] };
        platform.transform.parent = transform;
        pos.x = -4.9f + 2 * (num / 3);
        pos.z = -2f + 2 * (num % 3);
        platform.transform.localPosition = pos;
        platform.transform.localRotation = Quaternion.Euler(rotate);
        platform.transform.localScale = new Vector3(100f, 100f, 100f);
        arrPlatform[num] = platform;
    }
}
