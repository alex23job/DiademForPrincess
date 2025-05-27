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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddTail(int id, Vector3 localPos, Quaternion localRot)
    {
        TiaraElement elem = new TiaraElement(id, localPos, localRot);
        tails.Add(elem);
    }
}

[Serializable]
public class TiaraElement
{
    int ElemID = 0;
    Vector3 localPosition = Vector3.zero;
    Quaternion localRotation = Quaternion.identity;

    public TiaraElement() { }
    public TiaraElement(int id, Vector3 localPos, Quaternion localRot)
    {
        ElemID = id;
        localPosition = localPos;
        localRotation = localRot;
    }
}

[Serializable]
public class TiaraData
{
    private List<TiaraElement> elements = new List<TiaraElement>();

    public TiaraData() { }
    public TiaraData(string csv)
    {

    }


    public string ToCsvString()
    {
        StringBuilder sb = new StringBuilder();

        return sb.ToString();
    }
}