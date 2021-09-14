using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    float speed = 5f;
    public float Angle= 0; 
    public Rigidbody Bullet;
    public Transform FirePoint; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.J))
        {
            Rigidbody clone;
            clone = (Rigidbody)Instantiate(Bullet, FirePoint.position, FirePoint.rotation);
            clone.velocity = transform.TransformDirection(Vector3.forward * Angle*speed);

        }
    }
}
