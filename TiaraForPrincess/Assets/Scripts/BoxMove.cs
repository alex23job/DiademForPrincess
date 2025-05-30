using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;


    private int moveDirection = 0;
    private Vector3 startPos, endPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        endPos = startPos;
        endPos.y -= 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveDirection != 0)
        {
            if (moveDirection > 0)
            {   //  вверх
                Vector3 currentPos = transform.position;
                currentPos.y += moveSpeed * moveDirection * Time.deltaTime;
                if (currentPos.y < startPos.y) transform.position = currentPos;
                else
                {
                    transform.position = startPos;
                    moveDirection = 0;
                }
            }
            if (moveDirection < 0)
            {   //  вверх
                Vector3 currentPos = transform.position;
                currentPos.y += moveSpeed * moveDirection * Time.deltaTime;
                if (currentPos.y > endPos.y) transform.position = currentPos;
                else
                {
                    transform.position = endPos;
                    moveDirection = 0;
                }
            }
        }
    }

    public void SetNapr(int direct)
    {
        moveDirection = direct;
    }
}
