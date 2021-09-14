using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Move1 : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    int deleteHeight = -10;
    // Start is called before the first frame update
    void Start()
    {
         agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //agent.SetDestination(target.position);

        Destroy(transform.GetComponent<NavMeshAgent>());
        
        var targetPos = new Vector3(transform.position.x, deleteHeight, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 1 * Time.fixedDeltaTime);
    }
}
