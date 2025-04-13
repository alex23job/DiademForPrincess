using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiaraMovement : MonoBehaviour
{
    [SerializeField] private float speedRot;

    Vector3 startRot;
    // Start is called before the first frame update
    void Start()
    {
        startRot = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        Vector3 rot = transform.rotation.eulerAngles;
        rot.z += hor * speedRot * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rot);
    }
}
