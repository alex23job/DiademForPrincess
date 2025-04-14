using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailControl : MonoBehaviour
{
    private Vector3 startPos;
    private bool isMove = false;
    private bool isChild = false;
    private Vector3 delta = Vector3.zero;
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
            figPos.x += mp.x - delta.x; figPos.y += 1.35f * (mp.y - delta.y);
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
            Transform child = transform.GetChild(0);
            LinkPointControl lpc = child.gameObject.GetComponent<LinkPointControl>();
            print($"UP {lpc.ConnectionPoint.name}");
            if (lpc.ConnectionPoint != null)
            {
                transform.parent = lpc.ConnectionPoint.transform.parent;
                transform.localPosition = lpc.ConnectionPoint.transform.localPosition;
                transform.localRotation = lpc.ConnectionPoint.transform.localRotation;
                transform.gameObject.GetComponent<BoxCollider>().enabled = false;
                child.gameObject.SetActive(false);
                lpc.ConnectionPoint.SetActive(false);
                isChild = true;
            }
        }
    }
}
