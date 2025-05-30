using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TiaraSet : MonoBehaviour
{
    [SerializeField] private BoxMove boxMove;

    private List<TiaraElement> tails = new List<TiaraElement>();
    private int linkPointCoint = 0;

    public int CountTails { get { return tails.Count; } }

    // Start is called before the first frame update
    void Start()
    {
        tails = GameManager.Instance.currentPlayer.tiaraData.GetElementsData();
        if (CountTails > 0) BuildTiara();
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    public void AddTail(int id, Vector3 localPos, Quaternion localRot, Vector3 parentPos)
    {
        TiaraElement elem = new TiaraElement(id, localPos, localRot, parentPos);
        tails.Add(elem);
        GameManager.Instance.currentPlayer.tiaraData.CopyElementsData(tails);
        GameManager.Instance.SaveGame();
    }

    private void BuildTiara()
    {
        linkPointCoint = transform.childCount;
        Transform[] childTransforms = new Transform[transform.childCount];
        int i, j;
        for (i = 0; i < transform.childCount; i++) childTransforms[i] = transform.GetChild(i);
        List<TiaraElement> tmpTails = new List<TiaraElement>();
        foreach(TiaraElement elem in tails)
        {
            tmpTails.Add(new TiaraElement(elem.ElementID, elem.LocalPosition, elem.LocalRotation, elem.ParentPosition));
        }
        for (i = 0; i < childTransforms.Length; i++)
        {
            for (j = 0; j < tmpTails.Count; j++)
            {
                if ((tmpTails[j].ElementID >= 20) && (childTransforms[i].localPosition == tmpTails[j].LocalPosition))
                {
                    GameObject tail = Instantiate(TailPrefabPak.Instance.GetTail(tmpTails[j].ElementID - 20));
                    tail.GetComponent<TailControl>().SetTailID(tmpTails[j].ElementID);
                    tail.transform.parent = transform;
                    tail.transform.localPosition = tmpTails[j].LocalPosition;
                    tail.transform.localRotation = tmpTails[j].LocalRotation;
                    tmpTails.RemoveAt(j);
                    Transform[] stonePoints = new Transform[tail.transform.childCount - 1];
                    if (stonePoints.Length > 0)
                    {
                        for (int k = 0; k < stonePoints.Length; k++)
                        {
                            stonePoints[k] = tail.transform.GetChild(k + 1);
                            for (j = 0; j < tmpTails.Count; j++)
                            {
                                if ((tmpTails[j].ElementID < 20) && (stonePoints[k].localPosition == tmpTails[j].LocalPosition))
                                {
                                    GameObject stone = Instantiate(TailPrefabPak.Instance.GetStone(tmpTails[j].ElementID));
                                    stone.transform.parent = tail.transform;
                                    stone.transform.localPosition = tmpTails[j].LocalPosition;
                                    stone.transform.localRotation = tmpTails[j].LocalRotation;
                                    tmpTails.RemoveAt(j);
                                    break;
                                }
                            }
                        }
                    }                    
                    break;
                }
            }
        }
    }

    public void DeconstructTiara()
    {
        boxMove.SetNapr(-1);
        Invoke("BoxUp", 10f);
        Vector3 point = boxMove.EndPos;
        point.y += 2f;
        float countDelay = 0;
        for(int i = transform.childCount - 1; i > linkPointCoint - 1; i--)
        {
            Transform childTail = transform.GetChild(i);
            for(int j = childTail.childCount - 1; j >= 0; j--)
            {
                Transform childStone = childTail.GetChild(j);
                if (childStone != null)
                {
                    if (childStone.name.Contains("Point"))
                    {
                        break;
                    }
                    else
                    {
                        childStone.parent = null;
                        childStone.gameObject.AddComponent<TailMoveBox>();
                        childStone.gameObject.AddComponent<StoneInfo>();
                        countDelay += 0.2f;
                        childStone.gameObject.GetComponent<TailMoveBox>().Destruct(point, countDelay, 2f);
                    }
                }
            }
            childTail.parent = null;
            childTail.gameObject.AddComponent<TailMoveBox>();
            countDelay += 0.5f;
            childTail.gameObject.GetComponent<TailMoveBox>().Destruct(point, countDelay, 2f);
        }
        return;
        tails.Clear();
        GameManager.Instance.currentPlayer.tiaraData.CopyElementsData(tails);
        GameManager.Instance.SaveGame();
    }

    private void BoxUp()
    {
        boxMove.SetNapr(1);
    }

    /*public string ToCsvString(char sep = '#')
    {
        StringBuilder sb = new StringBuilder();
        foreach (TiaraElement elem in tails)
        {
            sb.Append($"{elem.GetCsvString()}{sep}");
        }
        return sb.ToString();
    }*/
}

[Serializable]
public class TiaraElement
{
    int ElemID = 0;
    Vector3 localPosition = Vector3.zero;
    Quaternion localRotation = Quaternion.identity;
    Vector3 parentPosition = Vector3.zero;

    public int ElementID { get { return ElemID; } }
    public Vector3 LocalPosition {  get { return localPosition; } }
    public Quaternion LocalRotation { get { return localRotation; } }
    public Vector3 ParentPosition {  get { return parentPosition; } }
    public TiaraElement() { }
    public TiaraElement(int id, Vector3 localPos, Quaternion localRot, Vector3 parentPos)
    {
        ElemID = id;
        localPosition = localPos;
        localRotation = localRot;
        parentPosition = parentPos;
    }
    public TiaraElement(string csvStr, char sep = '=')
    {
        string[] ar = csvStr.Split(sep);
        if (ar.Length >= 10)
        {
            if (int.TryParse(ar[0], out int id))
            {
                ElemID = id;
            }
            if (float.TryParse(ar[1], out float x) && float.TryParse(ar[2], out float y) && float.TryParse(ar[3], out float z))
            {
                localPosition = new Vector3(x, y, z);
            }
            if (float.TryParse(ar[4], out float ex) && float.TryParse(ar[5], out float ey) && float.TryParse(ar[6], out float ez))
            {
                localRotation = Quaternion.Euler(new Vector3(ex, ey, ez));
            }
            if (float.TryParse(ar[7], out float px) && float.TryParse(ar[8], out float py) && float.TryParse(ar[9], out float pz))
            {
                parentPosition = new Vector3(px, py, pz);
            }
        }
    }
    public string GetCsvString(char sep = '=')
    {
        return $"{ElemID}{sep}{localPosition.x}{sep}{localPosition.y}{sep}{localPosition.z}{sep}{localRotation.eulerAngles.x}{sep}{localRotation.eulerAngles.y}{sep}{localRotation.eulerAngles.z}{sep}{parentPosition.x}{sep}{parentPosition.y}{sep}{parentPosition.z}{sep}";
    }
}

[Serializable]
public class TiaraData
{
    private List<TiaraElement> elements = new List<TiaraElement>();

    public int ElementsCount { get { return elements.Count; } }
    public TiaraData() { }
    public TiaraData(string csv)
    {
        //Debug.Log($"csvTiaraData => {csv}");
        string[] ar = csv.Split('#', options: System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string s in ar)
        {
            elements.Add(new TiaraElement(s));
        }
    }

    public void CopyElementsData(List<TiaraElement> tails)
    {
        elements.Clear();
        foreach(TiaraElement elem in tails)
        {
            elements.Add(new TiaraElement(elem.ElementID, elem.LocalPosition, elem.LocalRotation, elem.ParentPosition));
        }
    }

    public List<TiaraElement> GetElementsData()
    {
        List<TiaraElement> list = new List<TiaraElement>();
        foreach (TiaraElement elem in elements)
        {
            list.Add(new TiaraElement(elem.ElementID, elem.LocalPosition, elem.LocalRotation, elem.ParentPosition));
        }
        return list;
    }
    public string ToCsvString(char sep = '#')
    {
        StringBuilder sb = new StringBuilder();
        foreach (TiaraElement elem in elements)
        {
            sb.Append($"{elem.GetCsvString()}{sep}");
        }
        return sb.ToString();
    }
}