using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectBonus : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bonus") || other.CompareTag("stone"))
        {
            Destroy(other.gameObject);
        }
    }
}
