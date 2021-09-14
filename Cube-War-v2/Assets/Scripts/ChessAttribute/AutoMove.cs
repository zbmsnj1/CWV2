using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;
using System.Linq;



public class AutoMove : MonoBehaviour
{


    public GameObject enemy;
    public GameObject enemy1;
    public GameObject enemy2;
    //public static bool onCollision;
    public Character character;
    public bool winRound;
    public GameObject atkEffect;

    private Slider eeslider;
    //private GameObject[] enemies = new GameObject[15];
    private Dictionary<GameObject, float> enemies = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> sortedEnemies = new Dictionary<GameObject, float>();
    private int deleteHeight = -100;
    private int MoveSpeed = 50;

    private bool showTrutleEffect_;

    private bool loseHP;
    private string thisTag;
    Vector3 target;
    private float dieTimer = 3f;
    private ChessFSM chessFSM;
    private ChessStruct chessStruct;
    private float playWinfwTime = 2f;
    private float playLosefwTime = 2f;
    private bool playedwin;
    private bool playedlose;

    private GameObject blueEfff;
    private GameObject redEfff;
    private GameObject yellowEfff;
    bool addedNMA;

    /*    public float attackSpeed = 10f;*/
    Animator mAnimator;
    public NavMeshAgent agent;

    public bool isAttacking;

    //GameObject currentChess;

    // Start is called before the first frame update
    void Start()
    {
        chessStruct = GameObject.Find("AllChessInfo").GetComponent<ChessStruct>();
        character = transform.GetComponent<Character>();
        chessFSM = transform.GetComponent<ChessFSM>();
        agent = GetComponent<NavMeshAgent>();

        blueEfff = transform.Find("BlueEff").gameObject;
        redEfff = transform.Find("RedEff").gameObject;
        yellowEfff = transform.Find("YellowEff").gameObject;

        if (transform.tag == "Enemy")
        {
            thisTag = "Enemy";

            eeslider = chessStruct.playerBigSlider;
        }
        else
        {
            thisTag = "BattleChess";

            eeslider = chessStruct.enemyBigSlider;
        }

        mAnimator = gameObject.GetComponent<Animator>();
        addedNMA = false;
        isAttacking = false;
        showTrutleEffect_ = false;
    }

    // Update is called once per frame
    void Update()
    {
    
        ShowLifeStealBuff();
        showTrutleEffect();

        if (TimeDown.GAME_STATE == TimeDown.Game_State.Battle)
        {
            initEnemies();

            if(transform.position.z > ChessStruct.benchPosZZ){
                if(gameObject.tag == "DieChess" || gameObject.tag == "DeadEnemy"){

                }else{
                    AddNavMeshAgent();
                    enemy = findNthClosedEnemy(enemy, 1);
                    enemy1 = findNthClosedEnemy(enemy1, 2);
                    enemy2 = findNthClosedEnemy(enemy2, 3);
                    //findClosedEnemy();
                }
                
            
                chessFSM.updateTransitions();
                chessFSM.updateFSM();
                
            }            
        }

        
    }

    void AddNavMeshAgent(){
        
        if(!addedNMA){
            gameObject.AddComponent<NavMeshAgent>();
            transform.GetComponent<NavMeshAgent>().speed = 1f;
            transform.GetComponent<NavMeshAgent>().radius = 0.4f;
            addedNMA = true;
        }
    }

    public void DeleteNavMeshAgent(){
        if(addedNMA){
            Destroy(transform.GetComponent<NavMeshAgent>());
            addedNMA = false;
        }
    }
    

    public void ResetAll(){
        blueEfff.SetActive(false);
        yellowEfff.SetActive(false);
        redEfff.SetActive(false);
        winRound = false;
        playWinfwTime = 2f;
        playLosefwTime = 2f;
        dieTimer = 5f;
        //onCollision = false;
        character.initSelf();
        chessFSM.resetTransitions();
        chessFSM.curState = ChessFSM.FSMState.LookingForEnemy;
        chessFSM.curState_I = ChessFSM.FSMState_I.Idle;
        chessIdle();
        loseHP = false;
        chessStruct.fireworkwin.SetActive(false);
        chessStruct.fireworklose.SetActive(false);
        playedlose = false;
        playedwin = false;
        gameObject.tag = "BattleChess";
    }


   
    public void chessIdle()
    {
        isAttacking = false;
        mAnimator.speed = 1.0f;
        //transform.GetComponent<Rigidbody>().isKinematic = false;
        transform.GetComponent<BoxCollider>().enabled = true;
        //transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        transform.GetComponent<Animator>().SetBool("have_a_path_to_enemy", false);
        transform.GetComponent<Animator>().SetBool("in_attack_range", false);
        transform.GetComponent<Animator>().SetBool("win_a_round", false);
        transform.GetComponent<Animator>().SetBool("die", false);
        transform.GetComponent<Animator>().SetBool("kill_an_enemy", false);

        if (atkEffect != null)
        {
            atkEffect.SetActive(false);
        }
    }

    public void chessCelebrate()
    {
        isAttacking = false;

        transform.GetComponent<NavMeshAgent>().updateRotation = true;
        mAnimator.speed = 1.0f;
        transform.GetComponent<Animator>().SetBool("win_a_round", true);
        //transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;

        Debug.Log("celebrate!");

        if (atkEffect != null)
        {
            atkEffect.SetActive(false);
        }
    }

    public void chessMoving()
    {
        isAttacking = false;

        transform.GetComponent<NavMeshAgent>().updateRotation = true;
        transform.GetComponent<NavMeshAgent>().isStopped = false; 

        mAnimator.speed = 1.0f;
        //transform.GetComponent<Rigidbody>().constraints =   RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
        transform.GetComponent<Animator>().SetBool("have_a_path_to_enemy", true);
        transform.GetComponent<Animator>().SetBool("in_attack_range", false);

        //transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        //lookAtEnemy(enemy);
        if(enemy != null)

            transform.GetComponent<NavMeshAgent>().SetDestination(enemy.transform.position);

        if (atkEffect != null)
        {
            atkEffect.SetActive(false);
        }
    }

    public void chessAttacking()
    {

        mAnimator.speed = character.now_AtkSpeed;
        transform.GetComponent<Animator>().SetBool("kill_an_enemy", false);
        transform.GetComponent<Animator>().SetBool("in_attack_range", true);

        transform.GetComponent<NavMeshAgent>().updateRotation = false;
        transform.GetComponent<NavMeshAgent>().isStopped = true; 


        //transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        isAttacking = true;

        if (atkEffect != null)
        {
            atkEffect.SetActive(true);
        }
        lookAtEnemy(enemy);
    }

    

    

    void ShowLifeStealBuff(){
        if(gameObject.tag == "Chess"){
            character.now_LifeSteal = character.base_LifeSteal;
        }
        if(gameObject.tag == "BattleChess" && chessStruct.NoBatNow()){
            character.now_LifeSteal = character.base_LifeSteal;
        }

        if(character.now_LifeSteal != 0){
            redEfff.SetActive(true);
            
            if(character.star == 1){
                redEfff.transform.position = new Vector3 (transform.position.x, transform.position.y+0.3f, transform.position.z);
                redEfff.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }

            if(character.star == 2){
                redEfff.transform.position = new Vector3 (transform.position.x, transform.position.y+0.4f, transform.position.z);
                redEfff.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
                
                

            if(character.star == 3){
                redEfff.transform.position = new Vector3 (transform.position.x, transform.position.y+0.5f, transform.position.z);
                redEfff.transform.localScale = new Vector3(2f, 2f, 2f);
            }
                
        }  
        else
            redEfff.SetActive(false);
    }

    void showTrutleEffect(){
        if(!showTrutleEffect_){
            if(character.id == 4){
                yellowEfff.transform.position = new Vector3 (transform.position.x, transform.position.y+0.3f, transform.position.z);
                yellowEfff.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                yellowEfff.SetActive(true);
            }

            if(character.id == 5){
                yellowEfff.transform.position = new Vector3 (transform.position.x, transform.position.y+0.4f, transform.position.z);
                yellowEfff.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                yellowEfff.SetActive(true);
            }
                    

            if(character.id == 6){
                yellowEfff.transform.position = new Vector3 (transform.position.x, transform.position.y+0.5f, transform.position.z);
                yellowEfff.transform.localScale = new Vector3(2f, 2f, 2f);
                yellowEfff.SetActive(true);
            }
            showTrutleEffect_ = true;
        }

        

        
    }

    
    public void chessDie()
    {
        isAttacking = false;
        DeleteNavMeshAgent();

        mAnimator.speed = 1.0f;
        transform.GetComponent<Animator>().SetBool("die", true);
        //transform.GetComponent<Rigidbody>().isKinematic = true;
        transform.GetComponent<BoxCollider>().enabled = false;
        character.selfSlider.transform.gameObject.SetActive(false); //死了先把血条隐藏


        /*  if (gameObject.tag != thisTag)                  //very important!!!!!!!!!!!!!!
        {
            return;
        }*/

        if (gameObject.tag == "BattleChess")
        {           
            gameObject.tag = "DieChess";       
        }

        if (gameObject.tag == "Enemy")
        {
            gameObject.tag = "DeadEnemy";
        }

        if (atkEffect != null)
        {
            atkEffect.SetActive(false);
        }



        dieTimer -= Time.deltaTime;
        if (dieTimer <= 0)
        {
            gameObject.SetActive(false);
            //target = new Vector3(transform.position.x, deleteHeight, transform.position.z);
            //transform.position = Vector3.MoveTowards(transform.position, target, MoveSpeed * Time.fixedDeltaTime);
            //Destroy(gameObject);
        }

        
    }

    public bool dead()
    {
        if (character.now_HP <= 0)
            return true;
        else
            return false;
    }

    public bool kill()
    {
        if (enemy!=null && enemy.GetComponent<Character>().now_HP <= 0)
            return true;
        else
            return false;
    }

    public bool winAround()
    {
        if (enemy == null)
        {
            
            Debug.Log("kill");

           
            return true;
        }
        else
        {
            return false;
        }

    }

    void lookAtEnemy(GameObject g)
    {


        transform.LookAt(new Vector3(g.transform.position.x, transform.position.y, g.transform.position.z));
    }

    void initEnemies()
    {

        enemies = new Dictionary<GameObject, float>();
        if (gameObject.tag == "BattleChess")
        {
            var allenemy = GameObject.FindGameObjectsWithTag("Enemy");

            foreach(var obj in allenemy){
                float distance = (obj.transform.position - transform.position).magnitude;
                enemies.Add(obj, distance);
            }

           
 
        }
        if (gameObject.tag == "Enemy")
        {
            var allenemy = GameObject.FindGameObjectsWithTag("BattleChess");

            foreach(var obj in allenemy){
                float distance = (obj.transform.position - transform.position).magnitude;
                enemies.Add(obj, distance);
            }
            //Debug.Log(enemy.name);          
 
        }
    }

    void findClosedEnemy()
    {
        if(enemies.Count() == 0)
            return;

        if(enemy!= null && enemy.GetComponent<Character>().now_HP > 0)
            return;
        enemy = null;

        

        sortedEnemies =   enemies.OrderBy(pair => pair.Value).ToDictionary(p => p.Key, p => p.Value);

        enemy = sortedEnemies.First().Key;

    }


    GameObject findNthClosedEnemy(GameObject e, int i){
        if(enemies.Count() < i)
            return null;

        if(e!= null && e.GetComponent<Character>().now_HP > 0)
            return e;
        e = null;

        

        sortedEnemies =   enemies.OrderBy(pair => pair.Value).ToDictionary(p => p.Key, p => p.Value);

        e = sortedEnemies.Keys.ElementAt(i-1);
        return e;
    }



    public bool InAttackRange(){
        

        if (enemy != null && character.now_HP>=0 && (enemy.transform.position - transform.position).magnitude <= character.atk_Range)
        {
            //DeleteNavMeshAgent();
            
            return true;
        }

        return false;

    }

    public bool InMoveingRange(){
        if(enemy!= null && character.now_HP>=0)
        {
            return true;
        }

        return false;

    }


    void resetAnimator()
    {
        transform.GetComponent<Animator>().SetBool("have_a_path_to_enemy", false);
        transform.GetComponent<Animator>().SetBool("in_attack_range", false);
        transform.GetComponent<Animator>().SetBool("win_a_round", false);
        transform.GetComponent<Animator>().SetBool("die", false);
        transform.GetComponent<Animator>().SetBool("kill_an_enemy", false);
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemyWall") && gameObject.tag == "BattleChess")
        {
            playedwin = false;
            winRound = true;
            loseHP = false;
        }
        else if (collision.gameObject.CompareTag("playerWall") && gameObject.tag == "Enemy")
        {
            playedlose = false;
            winRound = true;
            loseHP = false;
        }

    }

    public void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("enemyWall") && gameObject.tag == "BattleChess")
        { 
            
            if (!playedwin)
            {
                playWinfwTime -= Time.fixedDeltaTime;
                if (playWinfwTime >= 0)
                {
                    chessStruct.fireworkwin.SetActive(true);

                }
                else
                {
                    playWinfwTime = 2;
                    playedwin = true;
                    chessStruct.fireworkwin.SetActive(false);

                }

            }         

            if (!loseHP)
            {
                eeslider.value -= 5;
                eeslider.GetComponentInChildren<TMP_Text>().text = eeslider.value + "%";
                loseHP = true;
            }
        }
        else if (collision.gameObject.CompareTag("playerWall") && gameObject.tag == "Enemy")
        {
            winRound = true;

            if (!playedlose)
            {
                playLosefwTime -= Time.deltaTime;
                if (playLosefwTime >= 0)
                {
                    chessStruct.fireworklose.SetActive(true);
                    
                }
                else
                {
                    playLosefwTime = 2;
                    playedlose = true;
                    chessStruct.fireworklose.SetActive(false);
                }

            }

            if (!loseHP)
            {
                eeslider.value -= 5;
                eeslider.GetComponentInChildren<TMP_Text>().text = eeslider.value + "%";
                loseHP = true;
               
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {   if(TimeDown.GAME_STATE == TimeDown.Game_State.Battle)
        {
            if (other.gameObject.CompareTag("BottleRed"))
            {
                character.now_AtkDmg += (character.now_AtkDmg*0.2f);
                other.gameObject.SetActive(false);
                redEfff.SetActive(true);
            }
            if (other.gameObject.CompareTag("BottleGreen"))
            {
                character.now_HP -= (int) (character.now_HP*0.2f);
                other.gameObject.SetActive(false);
                yellowEfff.SetActive(true);
                
            }
            if (other.gameObject.CompareTag("BottleBlue"))
            {
                character.now_AtkSpeed += (character.now_AtkSpeed * 0.2f);
                other.gameObject.SetActive(false);
                blueEfff.SetActive(true);

            }
        }
        


    }
}
