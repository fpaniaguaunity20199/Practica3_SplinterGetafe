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
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
                    animator.SetBool("Walking", true);
                }
            }
        }
    }


}
