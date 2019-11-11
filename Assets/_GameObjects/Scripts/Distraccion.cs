using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distraccion : MonoBehaviour
{
    Vigilante vigilante;
    private void Start()
    {
        vigilante = GameObject.Find("Eddie").GetComponent<Vigilante>();
        GetComponent<Rigidbody>().AddForce(Vector3.right * 500);
    }
    private void OnCollisionEnter(Collision collision)
    {
        vigilante.SetDestination(transform.position);
    }
}
