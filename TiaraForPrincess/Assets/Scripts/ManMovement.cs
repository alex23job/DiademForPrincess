using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationRate = 360;

    private Rigidbody rb;
    private Animator anim;
    Vector3 movement;

    private List<Vector3> arrPoint;
    private Vector3 target;
    private bool isIdle = true;
    private bool isMove = false;
    private int idleNumber = 0;
    private int moveNumber = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        arrPoint = new List<Vector3>();
        arrPoint.Add(new Vector3(-6.5f, 0.5f, 2f));
        arrPoint.Add(new Vector3(-6.5f, 0.5f, 0f));
        arrPoint.Add(new Vector3(-6.5f, 0.5f, -2f));
        arrPoint.Add(new Vector3(-6.5f, 0.5f, 0f));
        //arrPoint.Add(new Vector3(-4.5f, 0.5f, 0f));
        //arrPoint.Add(new Vector3(-6.5f, 0.5f, 0f));
        target = arrPoint[0];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delta = transform.localPosition - target;
        if (delta.sqrMagnitude < 0.01f)
        {
            print($"1 delta={delta} deltaN={delta.normalized}");
            transform.localPosition = target;
            if (isIdle)
            {
                idleNumber++;
                if (arrPoint.Count != 0) idleNumber %= arrPoint.Count; else idleNumber = 0;
                target = arrPoint[idleNumber];
            }
            if (isMove)
            {
                moveNumber++;
                if (moveNumber < arrPoint.Count) target = arrPoint[moveNumber];
                else // финальный путь пройден -> закончить прохождение уровня
                {
                    
                }
            }
            delta = transform.localPosition - target;
            print($"2 delta={delta} deltaN={delta.normalized}");
        }
        else
        {
            movement = delta.normalized * moveSpeed * Time.deltaTime;
            //delta = transform.localPosition - movement;
            if (delta.magnitude > movement.magnitude) transform.localPosition = transform.localPosition - movement;
            else transform.localPosition = target;
            float rotY = Mathf.Atan2(delta.x, delta.z) * 180 / Mathf.PI;
            //transform.localRotation = Quaternion.Euler(new Vector3(0, rotY, 0));
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(delta), 6 * Time.deltaTime);
            //transform.LookAt(target);
            //rb.AddForce(movement);
            //transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            //transform.Rotate(0, input * rotationRate * Time.deltaTime, 0);

            //transform.Rotate(delta * rotationRate * Time.deltaTime);
        }
    }

    public void SetArrPoints(List<Vector3> points)
    {
        arrPoint = points;
        target = arrPoint[0];
        isMove = true;
        isIdle = false;
    }
}
