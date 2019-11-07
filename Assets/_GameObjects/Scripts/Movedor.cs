using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movedor : MonoBehaviour
{
    public Transform origen;
    public Transform destino;
    public float speed;
    private float pct = 0;
    private Vector3 nextPosition;
    void Update()
    {
        pct += Time.deltaTime * speed;
        nextPosition = Vector3.Lerp(origen.position, destino.position, pct);
        if (pct >= 1)
        {
            pct = 1;
            speed = -speed;
        } else if (pct <= 0)
        {
            pct = 0;
            speed = -speed;
        }
        transform.position = nextPosition;
    }
}