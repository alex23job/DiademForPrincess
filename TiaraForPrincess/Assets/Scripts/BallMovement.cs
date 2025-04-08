using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    LevelControl levelControl = null;

    Rigidbody _rb;
    bool isMove = false;
    Vector3 beginPos;
    Vector3 delta;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
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
            //if (delta != mp) print($"Input.mousePosition={Input.mousePosition} mp={mp}");
            //print($"delta={delta} mp={mp}");
            //Vector3 mp = Input.mousePosition;
            if (mp.x < -11.1f || mp.x > -1.9f) return;
            Vector3 figPos = transform.position;
            //figPos.x = mp.x + delta.x; figPos.z = mp.z + delta.z;
            figPos.x += mp.x - delta.x; //figPos.z += 1.35f * (mp.z - delta.z);
            transform.position = figPos;
            delta = mp;
        }
    }

    public void SetLevelControl(LevelControl lc)
    {
        levelControl = lc;
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMove = true;
            //beginPos = transform.position;
            print($"Tail OnMouseDown beginPos = {beginPos}");
            Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            delta = mp;
        }
    }

    private void OnMouseUp()
    {
        if (!isMove) return;
        if (!Input.GetMouseButtonDown(0))
        {
            isMove = false;
            delta = Vector3.zero;
            Vector3 figPos = transform.position;
            figPos.x = Mathf.RoundToInt((figPos.x + 11.0f) / 3f) * 3f - 11f;
            transform.position = figPos;
            //_rb.isKinematic = false;
            _rb.useGravity = true;
        }
    }

    private void OnMouseEnter()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMove = true;
            //beginPos = transform.position;
            print($"Tail OnMouseEnter beginPos = {beginPos}");
            Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            delta = mp;
        }
    }

    private void OnMouseExit()
    {
        isMove = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ball"))
        {
            if (levelControl != null)
            {
                levelControl.BallMoveEnd(transform.position);
            }
        }
    }
}
