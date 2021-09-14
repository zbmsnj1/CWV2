using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessFSM : MonoBehaviour
{
    public enum FSMState                                  //the base states
    {
        LookingForEnemy,
        Celebrate,
        Dead,
    }

    public enum FSMState_I                                //first child states
    {
        Idle,
        Moving,
        Attacking,
    }

    public struct Transitions                           //4 transitions
    {
        public bool have_a_path_to_enemy;
        public bool in_attack_range;
        public bool kill_an_enemy;
        public bool win_a_round;
        public bool die;
    }


    public FSMState curState;
    public FSMState_I curState_I;
    public float wolfSpeed;
    private AutoMove autoMoveScript;
    private Transitions transitions;
    private Character character;
    private static float KILL_SPEED = 2;
    private static float PATROLLING_SPEED = 1;
    private static float CHASING_SPEED = 1.5f;

    //private float MIN_ATTACKING_RANGE = 1f;
    private float MIN_PATH_DISTANCE = 30f;
    

    // Start is called before the first frame update
    void Awake()
    {
        curState = FSMState.LookingForEnemy;
        curState_I = FSMState_I.Idle;
        autoMoveScript = transform.GetComponent<AutoMove>();
        character = transform.GetComponent<Character>();
    }

    void Start(){

    }

    // Update is called once per frame
    void Update()
    {
       /* Debug.Log(curState.ToString());
        Debug.Log(curState_I.ToString());
        */
            
    }

    public void updateFSM()                                            //update base states
    {

        switch (curState)
        {
            case FSMState.LookingForEnemy:
                if (transitions.win_a_round)
                {
                    curState = FSMState.Celebrate;
                    break;
                }
                if (transitions.die)
                {
                    curState = FSMState.Dead;
                    break;
                }
                
                updateLookingFor();

                break;
            case FSMState.Celebrate:
                if (!transitions.win_a_round)
                {
                    curState = FSMState.LookingForEnemy;
                    break;
                }
                autoMoveScript.chessCelebrate();
                
                break;
            case FSMState.Dead:
                if (!transitions.die)
                {
                    curState = FSMState.LookingForEnemy;
                    break;
                }
                autoMoveScript.chessDie();
                break;

        }


    }


    public void updateLookingFor()                                           //update first child states
    {
        switch (curState_I)
        {
            case FSMState_I.Idle:
                if (transitions.have_a_path_to_enemy )
                {
                    curState_I = FSMState_I.Moving;
                    break;
                }
                else if (transitions.in_attack_range)
                {
                    curState_I = FSMState_I.Attacking;
                    break;
                }
                autoMoveScript.chessIdle();


                break;
            case FSMState_I.Moving:
                if (!transitions.have_a_path_to_enemy)
                {
                    curState_I = FSMState_I.Idle;
                    break;
                }
                else if (transitions.in_attack_range)
                {
                    curState_I = FSMState_I.Attacking;

                    break;
                }
                
                autoMoveScript.chessMoving();
               // wolfScript.chasing();
                break;
            case FSMState_I.Attacking:
                if (transitions.kill_an_enemy)
                {
                    curState_I = FSMState_I.Idle;
                    break;
                }
                else if (!transitions.in_attack_range )
                {
                    curState_I = FSMState_I.Moving;
                    break;
                }
                autoMoveScript.chessAttacking();
                //setWolfSpeed(CHASING_SPEED);
                
                break;

        }
    }

    public void updateTransitions()                                        //update transitions 
    {
        if (autoMoveScript.InMoveingRange())
            transitions.have_a_path_to_enemy = true;
        else
            transitions.have_a_path_to_enemy = false;

        if (autoMoveScript.InAttackRange())
            transitions.in_attack_range = true;
        else
            transitions.in_attack_range = false;

        if (autoMoveScript.kill())
            transitions.kill_an_enemy = true;
        else
            transitions.kill_an_enemy = false;

        if (autoMoveScript.dead())
            transitions.die = true;
        else
            transitions.die = false;

        if (autoMoveScript.winAround())
            transitions.win_a_round = true;
        else
            transitions.win_a_round = false;



    }


    public void resetTransitions()
    {
        transitions.have_a_path_to_enemy = false;
        transitions.in_attack_range = false;
        transitions.kill_an_enemy = false;
        transitions.die = false;
        transitions.win_a_round = false;
    }
}
