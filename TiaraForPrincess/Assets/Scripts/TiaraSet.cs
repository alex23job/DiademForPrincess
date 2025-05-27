using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TiaraSet : MonoBehaviour
{
    private List<TiaraElement> tails = new List<TiaraElement>();

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

    public void AddTail(int id, Vector3 localPos, Quaternion localRot)
    {
        TiaraElement elem = new TiaraElement(id, localPos, localRot);
        tails.Add(elem);
        GameManager.Instance.currentPlayer.tiaraData.CopyElementsData(tails);
        GameManager.Instance.SaveGame();
    }

    private void BuildTiara()
    {
        Transform[] childTransforms = new Transform[transform.childCount];
        int i;
        for (i = 0; i < transform.childCount; i++) childTransforms[i] = transform.GetChild(i);
    }

    public void DeconstructTiara()
    {

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

    public int ElementID { get { return ElemID; } }
    public Vector3 LocalPosition {  get { return localPosition; } }
    public Quaternion LocalRotation { get { return localRotation; } }
    public TiaraElement() { }
    public TiaraElement(int id, Vector3 localPos, Quaternion localRot)
    {
        ElemID = id;
        localPosition = localPos;
        localRotation = localRot;
    }
    public TiaraElement(string csvStr, char sep = '=')
    {
        string[] ar = csvStr.Split(sep);
        if (ar.Length == 7)
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
        }
    }
    public string GetCsvString(char sep = '=')
    {
        return $"{ElemID}{sep}{localPosition.x}{sep}{localPosition.y}{sep}{localPosition.z}{sep}{localRotation.eulerAngles.x}{sep}{localRotation.eulerAngles.y}{sep}{localRotation.eulerAngles.z}{sep}";
    }
}

[Serializable]
public class TiaraData
{
    private List<TiaraElement> elements = new List<TiaraElement>();

    public TiaraData() { }
    public TiaraData(string csv)
    {
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
            elements.Add(new TiaraElement(elem.ElementID, elem.LocalPosition, elem.LocalRotation));
        }
    }

    public List<TiaraElement> GetElementsData()
    {
        List<TiaraElement> list = new List<TiaraElement>();
        foreach (TiaraElement elem in elements)
        {
            list.Add(new TiaraElement(elem.ElementID, elem.LocalPosition, elem.LocalRotation));
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