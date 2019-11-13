using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Infiltrado : MonoBehaviour
{
    public GameObject prefab;
    public LayerMask layerMask;
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
        }
    }

    private void SetDestination()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.name == "Suelo")//Si se usa layerMask, quitar esta condicion
            {
                //Instantiate(prefab, hit.point, Quaternion.identity);
                nma.destination = hit.point;
                animator.SetBool("Idle", false);
                animator.SetBool("Walking", true);
                print("AQUI");
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
