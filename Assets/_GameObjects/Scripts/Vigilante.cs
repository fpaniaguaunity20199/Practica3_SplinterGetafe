using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vigilante : MonoBehaviour
{
    public enum Estado { Idle, Walking, Running, Celebrating }
    public Estado estado = Estado.Idle;
    private NavMeshAgent nma;
    private Animator animator;
    private int destinationID = 0;
    private Vector3 nextDestination;

    public List<Transform> puntosPatrullaje;
    public float delayVigilante = 1;
    [Header("If true -> Los puntos de vigilancia se eligen al azar")]
    public bool randomRoute;//If true -> Los puntos de vigilancia se eligen al azar

    // Start is called before the first frame update
    void Start()
    {
        nma = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        nma.SetDestination(puntosPatrullaje[destinationID].position);
        Walk();
    }

    private void Walk()
    {
        animator.SetBool("Walking", true);
        estado = Estado.Walking;
    }

    // Update is called once per frame
    void Update()
    {
        if (estado == Estado.Walking && nma.remainingDistance==0)
        {
            animator.SetBool("Walking", false);
            nextDestination = randomRoute ? GetRandomDestination() : GetNextDestination();
            transform.LookAt(nextDestination);
            Invoke("ChangeDestination", delayVigilante);
            estado = Estado.Idle;
        }
    }

    private void ChangeDestination()
    {
        nma.SetDestination(nextDestination);
        Walk();
    }

    private Vector3 GetNextDestination()
    {
        destinationID++;
        if (destinationID == puntosPatrullaje.Count)
        {
            destinationID = 0;
        }
        return puntosPatrullaje[destinationID].position;
    }
    private Vector3 GetRandomDestination()
    {
        int destinoAleatorio = 0;
        do
        {
            destinoAleatorio = Random.Range(0, puntosPatrullaje.Count);
        } while (destinoAleatorio == destinationID);
        destinationID = destinoAleatorio;
        return puntosPatrullaje[destinationID].position;
    }
}
