using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Infiltrado : MonoBehaviour
{
    public enum Estado { Idle, Walking, Running, Stealthing}
    public Estado estado = Estado.Idle;
    public GameObject prefab;
    public LayerMask layerMask;
    public GameObject targetPoint;
    private NavMeshAgent nma;
    private Animator animator;
    private void Start()
    {
        nma = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        DesactivarRagdoll();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetDestination();
        }
        if (nma.hasPath && nma.remainingDistance < 0.1f)
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Walking", false);
            estado = Estado.Idle;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && estado!=Estado.Idle)
        {
            //Activo sigilo
            animator.SetBool("WalkingSigiloso", true);
            nma.speed = 0.50f;
            estado = Estado.Stealthing;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            //Desactivo sigilio
            animator.SetBool("WalkingSigiloso", false);
            nma.speed = 2.5f;
            if (estado != Estado.Idle)
            {
                estado = Estado.Walking;
            }
        }
    }
    private void SetDestination()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        //if (Physics.Raycast(ray, out hit))
        {
            //if (hit.transform.name == "Suelo")//Si se usa layerMask, quitar esta condicion
            {
                targetPoint.transform.position = hit.point;//Dibujamos el circulito
                targetPoint.transform.Translate(Vector3.back * 0.01f);//Lo subimos un poco para que no haga overlap
                targetPoint.transform.rotation = Quaternion.FromToRotation(Vector3.back, hit.normal);
                //Debug.DrawRay(hit.point, hit.normal, Color.red, 10f);
                nma.destination = hit.point;
                animator.SetBool("Idle", false);
                animator.SetBool("Walking", true);
                estado = Estado.Walking;
            }
        }
    }
    public void Morir()
    {
        ActivarRagdoll();
    }
    private void DesactivarRagdoll()
    {
        Rigidbody[] rbs =  GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rb in rbs)
        {
            rb.isKinematic = true;
        }
    }
    private void ActivarRagdoll()
    {
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = false;
        }
        GetComponent<Animator>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        Destroy(this);
    }
}
