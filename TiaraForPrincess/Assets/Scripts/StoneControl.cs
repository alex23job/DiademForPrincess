using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneControl : MonoBehaviour
{
    private Vector3 startPos;
    private bool isMove = false;
    private bool isChild = false;
    private Vector3 delta = Vector3.zero;
    private MeshRenderer mr = null;
    private GameObject connectPoint = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 figPos = transform.position;
            //figPos.x = mp.x + delta.x; figPos.z = mp.z + delta.z;
            //figPos.x += mp.x - delta.x; figPos.y += 1.35f * (mp.y - delta.y);
            figPos.x += mp.x - delta.x; figPos.y += 1f * (mp.y - delta.y);
            float x = Mathf.Abs(-1.8f - figPos.x);
            figPos.z = -2.08f * Mathf.Cos(Mathf.Asin(x / 2.08f));
            transform.position = figPos;
            delta = mp;
        }
    }

    private void OnMouseDown()
    {
        if (isChild) return;
        if (Input.GetMouseButtonDown(0))
        {
            startPos = transform.position;
            isMove = true;
            Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            delta = mp;
        }
    }

    private void OnMouseUp()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            isMove = false;
            delta = Vector3.zero;
            //Transform child = transform.GetChild(0);
            //LinkPointControl lpc = child.gameObject.GetComponent<LinkPointControl>();
            print($"UP {connectPoint.name}");
            if (connectPoint != null)
            {
                transform.parent = connectPoint.transform.parent;
                transform.localPosition = connectPoint.transform.localPosition;
                Vector3 rot = connectPoint.transform.localRotation.eulerAngles;
                //rot.x = 90f;
                transform.localRotation = Quaternion.Euler(rot);
                transform.gameObject.GetComponent<BoxCollider>().enabled = false;
                //child.gameObject.SetActive(false);
                connectPoint.SetActive(false);
                isChild = true;
                Destroy(gameObject.GetComponent<StoneControl>());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //print($"CircleTail enter {other.name} {other.transform.parent.name}");
        if (other.CompareTag("linkPoint"))
        {
            connectPoint = other.gameObject;
            mr = other.gameObject.GetComponent<MeshRenderer>();
            Material mat = TailPrefabPak.Instance.GetMaterial(3);
            mr.materials = new Material[] { mat };
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //print($"CircleTail exit {other.name} {other.transform.parent.name}");
        if (other.CompareTag("linkPoint"))
        {
            connectPoint = null;
            mr = other.gameObject.GetComponent<MeshRenderer>();
            Material mat = TailPrefabPak.Instance.GetMaterial(1);
            mr.materials = new Material[] { mat };
        }
    }
}

public class StoneInfo : MonoBehaviour
{
    private int id = -1;
    private int cat = -1;
    public int StoneID { get { return id; } }
    public int StoneCategory { get { return cat; } }
    public StoneInfo() { }
    public StoneInfo(int stoneID)
    {
        id = stoneID;
        cat = (id / 5) + 1;
    }

    public void SetStoneID(int stoneID)
    {
        id = stoneID;
        cat = (id / 5) + 1;
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
