using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertasAutomaticas : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
