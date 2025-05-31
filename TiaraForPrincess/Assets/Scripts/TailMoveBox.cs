using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailMoveBox : MonoBehaviour
{
    private Vector3 target = Vector3.zero;
    private bool isMove = false;
    private float moveSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            Vector3 delta = transform.position - target;
            if (delta.magnitude > 0.1f)
            {
                if (target.y > transform.position.y)
                {
                    transform.position += Vector3.up * moveSpeed * Time.deltaTime;
                }
                else transform.position -= delta.normalized * moveSpeed * Time.deltaTime;
            }
            else
            {
                isMove = false;
                gameObject.AddComponent<Rigidbody>();
                gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            }
        }
    }

    public void Destruct(Vector3 centerPoint, float delay = 0.5f, float speed = 3f)
    {
        target = centerPoint;
        moveSpeed = speed;
        TailControl tc = GetComponent<TailControl>();
        if (tc != null)
        {
            target.x += 1.6f;            
        }
        StoneInfo si = GetComponent<StoneInfo>();
        if (si != null)
        {
            target.x -= 1.6f;
        }
        transform.parent = null;
        Invoke("StartMove", delay + 0.5f);
    }

    public void StartMove()
    {
        //Destroy(gameObject, 1.5f);
        Destroy(gameObject, 7f);
        isMove = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isMove)
        {
            if (collision.gameObject.CompareTag("Finish"))
            {
                transform.parent = collision.transform;
            }
        }
    }
}
