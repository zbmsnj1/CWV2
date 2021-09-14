using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Move : MonoBehaviour
{

    public float speed = 2; //[1] 物体移动速度
    public Transform []target;  // [2] 目标
    public float delta = 0.2f; // 误差值
    public float distance;
    private float otherDistance;
    private static int i = 0;
    private const int walk = 0;
    private const int turn = 1;
    private const int stay = 2;
    private int state = 0;

    float rotateSpeed = 2f;
    Quaternion targetAngels;
    Quaternion initAngels;
    float angelY = 45f;

   
    void Start()
    {
        initAngels = Quaternion.Euler(0, 0f, 0);
        targetAngels = Quaternion.Euler(0, angelY, 0);
    }
 
  
    void Update()
    {
        MoveTo();
        distance = (target[i].transform.position - transform.position).magnitude;
        Debug.Log(gameObject.name + "state: " + state);
    }

    void MoveTo(){

        switch(state){
            case walk:
                MoveForward();
                break;
            case turn:
                TurnAround();
                break;
            case stay:
                Stay();
                break;

                
        }

        





    }

    void OnCollisionStay(Collision collision){
        //speed = 0;
        
        if(collision.gameObject.CompareTag("Chess")){

            try{
                Move otherMoveScript = collision.gameObject.GetComponent<Move>();
                otherDistance = otherMoveScript.distance;

                if(otherDistance < distance){
                    state = turn;
                }
            }catch{
                state = walk;
            }

            

            
        }

        if(collision.gameObject.CompareTag("Enemy")){

            state = stay;
        }



        
    }

    void OnCollisionExit(Collision collision) {
        if(collision.gameObject.CompareTag("Chess")){
            state = walk;
        }

    }

    void MoveForward(){
        // [4] 让物体朝向目标点 
            transform.LookAt (target [i]);

            // [5] 物体向前移动
            transform.Translate (Vector3.forward * Time.deltaTime * speed);

    }

    void TurnAround(){

        transform.rotation = Quaternion.Slerp(transform.rotation, targetAngels, rotateSpeed * Time.deltaTime);
        //transform.LookAt (target [i]);
        transform.Translate (Vector3.forward * Time.deltaTime * speed);
        
        if (Quaternion.Angle(targetAngels, transform.rotation) < 1)
        {
            state = walk; 
        }
  
        

    }

    void Stay(){
        transform.LookAt (target [i]);
        transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }

}
