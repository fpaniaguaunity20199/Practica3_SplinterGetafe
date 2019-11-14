using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vigilante : MonoBehaviour
{
    public Transform viewPoint;
    private GameObject player;
    private Transform playerDetectionPoint;
    //public TextMesh textoInformativo;
    public LayerMask mascaraDeteccion;
    public GameObject exclamacion;

    [Range(1,10)]
    public float viewDistance;
    [Range(10, 80)]
    public float viewAngle;
    [Range(0, 20)]
    public float listenDistanceSigil;
    [Range(0, 20)]
    public float listenDistanceWalk;
    [Range(0, 20)]
    public float listenDistanceRun;

    public enum Estado { Idle, Walking, Celebrating, Obsessed }
    public Estado estado = Estado.Idle;
    private NavMeshAgent nma;
    private Animator animator;
    private int destinationID = 0;
    private Vector3 nextDestination;

    public List<Transform> puntosPatrullaje;
    public float delayVigilante = 1;
    [Header("If true -> Los puntos de vigilancia se eligen al azar")]
    public bool randomRoute;//If true -> Los puntos de vigilancia se eligen al azar

    void Start()
    {
        player = GameObject.Find("Infiltrado");
        playerDetectionPoint = GameObject.Find("PlayerDetectionPoint").transform;
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
    void Update()
    {
        if (estado != Estado.Idle && nma.remainingDistance==0)
        {
            animator.SetBool("Walking", false);
            nextDestination = randomRoute ? GetRandomDestination() : GetNextDestination();
            transform.LookAt(nextDestination);
            Invoke("ChangeDestination", delayVigilante);
            estado = Estado.Idle;
            exclamacion.SetActive(false);
        }
        PlayerDetection();
        PlayerDetectionBySound();
    }
    public void SetDestination(Vector3 position)
    {
        nextDestination = position;
        ChangeDestination();
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
    private void PlayerDetectionBySound()
    {
        if (estado == Estado.Obsessed || player.GetComponent<Infiltrado>().estado == Infiltrado.Estado.Idle)
        {
            return;
        }
        float distanceToPlayer = Vector3.Distance(viewPoint.position, playerDetectionPoint.position);

        if (player.GetComponent<Infiltrado>().estado == Infiltrado.Estado.Walking && 
            distanceToPlayer <= listenDistanceWalk)
        {
            SetDestination(player.transform.position);
            estado = Estado.Obsessed;
            exclamacion.SetActive(true);
        }
        if (player.GetComponent<Infiltrado>().estado == Infiltrado.Estado.Stealthing &&
            distanceToPlayer <= listenDistanceSigil)
        {
            SetDestination(player.transform.position);
            estado = Estado.Obsessed;
            exclamacion.SetActive(true);
        }
    }
    private void PlayerDetection()
    {
        float distanceToPlayer = Vector3.Distance(viewPoint.position, playerDetectionPoint.position);
        float viewAngleToPlayer = Vector3.Angle(viewPoint.forward, playerDetectionPoint.position - viewPoint.position);
        //textoInformativo.text = distanceToPlayer.ToString() + ":" + viewAngleToPlayer;
        if (distanceToPlayer < viewDistance && viewAngleToPlayer < viewAngle)
        {
            IntentarMatar();
        }
    }
    private void IntentarMatar()
    {
        RaycastHit hit;
        Ray ray = new Ray(viewPoint.position, playerDetectionPoint.position - viewPoint.position);
        Debug.DrawRay(viewPoint.position, playerDetectionPoint.position - viewPoint.position, Color.red);
        if (Physics.Raycast(ray, out hit, viewDistance, mascaraDeteccion))
        {
            if (hit.transform.gameObject.GetComponentInParent<Infiltrado>()!=null)
            {
                Matar();
            }
        }
        
    }
    private void Matar()
    {
        player.GetComponent<Infiltrado>().Morir();
    }
}
