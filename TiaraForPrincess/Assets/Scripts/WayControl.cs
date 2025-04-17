using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WayControl : MonoBehaviour
{
    [SerializeField] private GameObject rectPrefab;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private Material[] arrMaterials;
    [SerializeField] private ManMovement manMovement;

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
        Vector3 pos = new Vector3(0, 0.3f, 0);
        List<int> candidat = new List<int>() { 3, 5, 6, 8, 9, 11, 12, 14};
        int numPos = Random.Range(0, candidat.Count);
        while (posBonus.Contains(candidat[numPos]))
        {
            candidat.RemoveAt(numPos);
            if (candidat.Count == 0) return;
            numPos = Random.Range(0, candidat.Count);
        }
        TailControl tc = bonusPrefab.GetComponent<TailControl>();
        GameObject bonus = Instantiate(bonusPrefab);
        bonus.transform.parent = transform;
        pos.x = -4.9f + 2 * (candidat[numPos] / 3);
        pos.z = -2f + 2 * (candidat[numPos] % 3);
        bonus.transform.localScale = new Vector3(70f, 70f, 70f);
        bonus.transform.localPosition = pos;
        if (tc != null) bonus.GetComponent<TailControl>().SetTailID(tc.TailID);
        bonus.tag = "bonus";
        bonus.GetComponent<BoxCollider>().isTrigger = true;
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
        TestPath();
    }

    public void TestPath()
    {
        int res = 0;
        if ((arrPlatform[17] != null) || (arrPlatform[16] != null) || (arrPlatform[15] != null)) res++;
        if ((arrPlatform[posBonus[0]] != null) && (arrPlatform[posBonus[1]] != null)) res++;
        if (res == 2)
        {   //  есть платформы под бонусами и в последнем р€ду -> можно пробовать строить маршрут дл€ принца
            List<int> path = new List<int>();
            List<Vector3> pathPoints = new List<Vector3>();
            int firstTarget = posBonus[1], secondTarget = posBonus[0];
            if (posBonus[0] < posBonus[1])
            {
                firstTarget = posBonus[0];
                secondTarget = posBonus[1];
            }
            int x = firstTarget % 3;
        }
    }

    public void Finish()
    {
        WavePath wp = new WavePath();
        wp.CreateBoard(arrRect);
        wp.SetStartEnd(posBonus[0], posBonus[1]);
        print($"startPos={posBonus[0]} endPos={posBonus[1]}");
        if (wp.FindPath())
        {
            int[] pt = wp.GetPath();
            StringBuilder sb = new StringBuilder("Path -> ");
            for (int i = 0; i < pt.Length; i++)
            {
                sb.Append($"{pt[i]} ");
            }
            print(sb.ToString());
        }
    }
}
