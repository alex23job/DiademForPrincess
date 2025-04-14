using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkPointControl : MonoBehaviour
{
    private Vector3 pointConnect = Vector3.zero;
    private Vector3 rotateConnect = Vector3.zero;
    private MeshRenderer mr = null;
    private GameObject connectPoint = null;

    public GameObject ConnectionPoint { get { return connectPoint; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //print($"CircleTail enter {other.name} {other.transform.parent.name}");
        if (other.CompareTag("linkPoint"))
        {
            connectPoint = other.gameObject;
            pointConnect = other.transform.parent.localPosition;
            rotateConnect = other.transform.parent.localRotation.eulerAngles;
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
            pointConnect = Vector3.zero;
            rotateConnect = Vector3.zero;
            mr = other.gameObject.GetComponent<MeshRenderer>();
            Material mat = TailPrefabPak.Instance.GetMaterial(1);
            mr.materials = new Material[] { mat };
        }
    }

    private void OnMouseUp()
    {
        /*if (!Input.GetMouseButtonDown(0))
        {
            if (connectPoint != null)
            {
                transform.parent.parent = connectPoint.transform.parent;
                transform.parent.localPosition = pointConnect;
                transform.parent.localRotation = Quaternion.Euler(rotateConnect);
                transform.parent.gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }*/
    }
}
