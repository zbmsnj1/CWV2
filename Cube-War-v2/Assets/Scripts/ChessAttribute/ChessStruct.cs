using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AI;

public class ChessStruct : MonoBehaviour
{
    public GameObject red_eff;
    public GameObject blue_eff;
    public GameObject yellow_eff;
    public GameObject line1_r;
    public GameObject line1_b;
    public GameObject line1_g;
    public GameObject line3_r;
    public GameObject line3_b;
    public GameObject line3_g;
    public GameObject line5_r;
    public GameObject line5_b;
    public GameObject line5_g;
    public GameObject fireworkwin;
    public GameObject fireworklose;
    public Slider enemyBigSlider;
    public Slider playerBigSlider;
    public GameObject upStarEffect_1;
    public GameObject upStarEffect_2;
    public GameObject winFireworks;
    public GameObject OurChess;
    public GameObject OurEnemy;
    public GameObject BattleChess;
    public GameObject chessTag;
    public GameObject selfSlider;
    public GameObject enemySlider;
    public GameObject benchCollider;
    public static bool initGlobalChesses;

    private bool addedEnemy;
    private GameObject[] myChess;
    public static int filled;
    public static int benchPosZZ = -3;
    public static bool[] bought = new bool[5];
    public GameObject[] enemyModel;

    private float upStarTimer = 1f;
    private Vector3 sameChPos;
    private bool upStar;
    [Serializable]
    public struct chess
    {
        public GameObject model;
    }
    [Serializable]
    public struct bench_area
    {
        public Vector3 pos;
    }

    
    public bench_area[] benchTiles ;
    public chess[] chesses;

    public struct addBat{
        public bool star1;
        public bool star2;
        public bool star3;
    }

    public addBat AddBat;
    
    public static chess[] globalChesses = new chess[200];
    public static bool[] hasChess = new bool[8];
    private float[] myChessPos_x = new float[10];
    private float[] myChessPos_z = new float[10];
    private int[][] chessLevel;
    //private GameObject go_Effect;
    
    // Start is called before the first frame update
    private void Awake() {
        chessLevel = new int[8][];
        for(int i=0; i<chessLevel.Length; i++)
        {
            chessLevel[i] = new int[3] { i*3 + 1, i*3 + 2, i*3 + 3 };
        }
        /*chessLevel[0] = new int[3] { 1, 2, 3 };
        chessLevel[1] = new int[3] { 4, 5, 6 };
        chessLevel[2] = new int[3] { 7, 8, 9 };*/

        //chessLevel[1, 2] = 4;

        for (int i=0; i<chesses.Length; i++)
        {
            
            globalChesses[i] = chesses[i];
            //初始化所有棋子模型和id，id为了后续根据储存的数据找到对应的名字，攻击，等等
            //globalChesses[i].model.GetComponent<Character>().initData();
        }

        AddBat.star1 = false;
        AddBat.star2 = false;
        AddBat.star3 = false;

        initGlobalChesses = true;
    }


    // Update is called once per frame


    public void addChesstoBench()
    {
        for(int i =0; i< GameControlSystem.buy.Length; i++)
        {
            filled = emptyBenchTile();
            if (filled > 7)
            {
                return;
            }
            if (GameControlSystem.buy[i] != 0 && !bought[i])
            {
                
                for (int j=0; j<chesses.Length; j++)
                {
                    int id = chesses[j].model.GetComponent<Character>().id;
                    if (GameControlSystem.buy[i] == id)
                    {
                        
                        var go = Instantiate(chesses[j].model, benchTiles[filled].pos, transform.rotation);
                        GameControlSystem.gold -= GameControlSystem.chessDataScript.allChessData[id-1].price;
                        go.transform.SetParent(OurChess.transform);
                        go.name = GameControlSystem.chessDataScript.allChessData[id-1].chessName;
                        int star = GameControlSystem.chessDataScript.allChessData[id-1].star;
                        addComponentForChess(go, id, star, false);
                        break;
                    }
                }
                
                
                //SceneManager.MoveGameObjectToScene(go, SceneManager.GetSceneByName("GameBoard"));

                bought[i] = true;
            }
        }

        
    }
    

    public int emptyBenchTile()
    {
        for(int i=0; i<8; i++)
        {
            if (!hasChess[i])
            {
                return i;
            }
        }

        return 1000;
    }

   

    public void recordChessPos()
    {
        myChess = GameObject.FindGameObjectsWithTag("BattleChess");
            for (int i = 0; i < myChess.Length; i++)
            {
                var zz = myChess[i].GetComponent<Transform>().position.z;
                if (zz > benchPosZZ)
                {
                    
                    myChessPos_x[i] = myChess[i].GetComponent<Transform>().position.x;
                    myChessPos_z[i] = myChess[i].GetComponent<Transform>().position.z;
                }
            }
    }

    public void resetChessPos()
    {
        EmptyEnemy();
        //addedEnemy = false;
        if(myChess==null){
            return;
        }
        
        for (int i = 0; i < myChess.Length; i++)
        {
            var zz = myChess[i].GetComponent<Transform>().position.z;
            if (zz > benchPosZZ)
            {
                myChess[i].transform.position = new Vector3(myChessPos_x[i], 1.0f, myChessPos_z[i]);
                myChess[i].transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                myChess[i].SetActive(true);
                myChess[i].GetComponent<Character>().selfSlider.transform.gameObject.SetActive(true);
               // myChess[i].GetComponent<NavMeshAgent>().enabled = false;          //每回合战斗开始前，先把NavMesh关闭，以确保棋子可以移动到bench区域
                //AutoMove.onCollision = false;                                            //how to reset walk and fight to false

            }

            AutoMove autoMoveScript = myChess[i].GetComponent<AutoMove>();
            autoMoveScript.ResetAll();          //重置棋子所有状态
            autoMoveScript.DeleteNavMeshAgent();    //删除
        }
    }

    public void addEnemy()
    {
        if (!addedEnemy)
        {
            GameObject[] go = new GameObject[10];
            if (TimeDown.ROUND == 1)
            {

                addEnemy(go, 0, 1, 3.5f, 5.5f);
                addEnemy(go, 1, 1, 4.5f, 5.5f);
                 addEnemy(go, 2, 1, 5.5f, 5.5f);
                  addEnemy(go, 3, 1, 6.5f, 5.5f);
                  addEnemy(go, 4, 1, 1.5f, 5.5f);

                //randomBottle(1, 2);
            }

            if (TimeDown.ROUND == 2)
            {
                addEnemy(go, 0, 1, 1.5f, 5.5f);
                addEnemy(go, 1, 1, 5.5f, 5.5f);

                //randomBottle(2, 3);
            }

           

            for (int i=0; i<go.Length; i++)
            {
                if (go[i] != null)
                {
                    go[i].transform.SetParent(OurEnemy.transform);
                }              
            }
            addedEnemy = true;
        }
           
        

       
      
    }

    void EmptyEnemy()
    {
        var go = GameObject.FindGameObjectsWithTag("Enemy");
        var dead_go = GameObject.FindGameObjectsWithTag("DeadEnemy");
        for (int i =0; i<go.Length; i++)
        {
            Destroy(go[i]);
        }
        for (int i = 0; i < dead_go.Length; i++)
        {
            Destroy(dead_go[i]);
        }

        addedEnemy = false;
    }

    public void checkSameChess()
    {
        for(int a=0; a<chessLevel.Length; a++)
            {
                upgradeChessLevel(chessLevel[a][0], chessLevel[a][1], upStarEffect_1);          //from 1 star to 2 star
                upgradeChessLevel(chessLevel[a][1], chessLevel[a][2], upStarEffect_2);          //from 2 star to 3 star

            }
 
    }



    void upgradeChessLevel( int firstLevel, int secondLevel, GameObject upStarEffect)
    {
        var sameChesses_ByTag = GameObject.FindGameObjectsWithTag(firstLevel.ToString());

        foreach(var ches in sameChesses_ByTag){                                                 //检查棋子位置，如果在棋盘上才进行升级，在benchtile上不升级
            if (ches.transform.position.z<=benchPosZZ){
                sameChesses_ByTag = sameChesses_ByTag.Where(val => val != ches).ToArray();          //从sameChess_ByTag中移除在benchtile上的棋子
            }
        }
        
        if (sameChesses_ByTag.Length < 3)
        {
            return;
        }             
        else                                               //if >3, destroy all 3 objects
        {
            
            sameChPos = sameChesses_ByTag[0].transform.parent.position;
            for (int i = 0; i < sameChesses_ByTag.Length; i++)
            {
                var go = sameChesses_ByTag[i].transform.parent.gameObject;
                Destroy(go);
            }
            

            for (int j = 0; j < chesses.Length; j++)                                    //create a new stronger objects
            {
                int go_Up_ID = secondLevel;
                if (go_Up_ID == chesses[j].model.GetComponent<Character>().id)
                {
                    var go_Effects = GameObject.FindGameObjectsWithTag("Effect");
                    foreach (var go in go_Effects)
                    {
                        Destroy(go);
                    }
                    var go_Effect = Instantiate(upStarEffect, new Vector3(sameChPos.x, sameChPos.y + 1f, sameChPos.z), transform.rotation);
                    go_Effect.tag = "Effect";
                    var go_Up = Instantiate(chesses[j].model, sameChPos, transform.rotation);
                    //GameControlSystem.gold -= chesses[j].model.GetComponent<Character>().price;
                    go_Up.transform.SetParent(OurChess.transform);
                    go_Up.name = GameControlSystem.chessDataScript.allChessData[go_Up_ID-1].chessName;
                    int star = GameControlSystem.chessDataScript.allChessData[go_Up_ID-1].star;
                    addComponentForChess(go_Up, go_Up_ID, star, false);
                    upStar = true;
                    break;
                }
            }
        }      
    }

    public void RunUpStarEffect()
    {
        if(upStar)
        {
            upStarTimer -= Time.deltaTime;
            if (upStarTimer <= 0)
            {
                var go_Effects = GameObject.FindGameObjectsWithTag("Effect");
                foreach (var go_Effect in go_Effects){
                    Destroy(go_Effect);
                }
                upStar = false;          
                upStarTimer = 1f;
            }
        }

        
    }

    void addComponentForChess(GameObject go, int id, int star, bool enemy=false)
    {
        if (!enemy)
        {
            go.tag = "Chess";
            go.layer = 10;    //chessMove layer
        }
        else
        {
            go.tag = "Enemy";
            go.layer = 11;    //chessMove layer
            
        }
        

        go.AddComponent<AutoMove>();
        go.AddComponent<ChessFSM>();
        go.AddComponent<ChessPosition>();
        go.AddComponent<Outline>();
        go.GetComponent<Outline>().enabled = false;
        
        go.AddComponent<BoxCollider>();
        go.GetComponent<BoxCollider>().isTrigger = true;



        if(go.GetComponent<Character>().atk_Effect!=null){
            var atkEffect = Instantiate(go.GetComponent<Character>().atk_Effect, go.transform.position, transform.rotation);
            atkEffect.transform.SetParent(go.transform);
            //atkEffect.GetComponent<Trajectory>().enabled = true;
            atkEffect.name = "Atk_Effect";
            
            go.GetComponent<AutoMove>().atkEffect = atkEffect;

        }
        //go.AddComponent<NavMeshAgent>();
       //go.GetComponent<NavMeshAgent>().enabled = false;
        //go.AddComponent<Rigidbody>();
        //go.GetComponent<Rigidbody>().useGravity = false; 
        //go.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        //go.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;
        //go.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ;

        Vector3 star1_position = new Vector3(go.transform.position.x, go.transform.position.y-0.9f, go.transform.position.z);
        Vector3 star2_position = new Vector3(go.transform.position.x, go.transform.position.y-0.8f, go.transform.position.z);
        Vector3 star3_position = new Vector3(go.transform.position.x, go.transform.position.y-0.7f, go.transform.position.z);
        Vector3 enemy_slider_position = new Vector3(go.transform.position.x, go.transform.position.y+0.7f, go.transform.position.z);
        Vector3 star1_size = new Vector3(1.0f, 1.0f, 1.0f);
        Vector3 star2_size = new Vector3(1.0f, 1.0f, 1.0f);
        Vector3 star3_size = new Vector3(1.0f, 1.0f, 1.0f);

        var cTag = Instantiate(chessTag, go.transform.position, transform.rotation);
        cTag.transform.SetParent(go.transform);
        if(!enemy){
            cTag.tag = id.ToString();    //for upstar chess
        }else{
            id = -1;
            cTag.tag = id.ToString();    //for enemy
        }
        

        var blue = Instantiate(blue_eff, go.transform.position, transform.rotation);
        blue.name = "BlueEff";
        blue.transform.SetParent(go.transform);
        blue.SetActive(false);

        var yellow = Instantiate(yellow_eff, go.transform.position, transform.rotation);
        yellow.name = "YellowEff";
        yellow.transform.SetParent(go.transform);
        yellow.SetActive(false);

        var red = Instantiate(red_eff, go.transform.position, transform.rotation);
        red.name = "RedEff";
        red.transform.SetParent(go.transform);
        red.SetActive(false);

        if (!enemy)
        {
            GameObject cSlider;
            switch (star)
            {
                case 1:
                    cSlider = Instantiate(selfSlider, star1_position, transform.rotation);
                    cSlider.transform.SetParent(go.transform);
                    go.GetComponent<BoxCollider>().size = star1_size;
                    if(go.GetComponent<AutoMove>().atkEffect!=null)
                        go.GetComponent<AutoMove>().atkEffect.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    break;
                case 2:
                    cSlider = Instantiate(selfSlider, star2_position, transform.rotation);
                    cSlider.transform.SetParent(go.transform);
                    go.GetComponent<BoxCollider>().size = star2_size;
                     if(go.GetComponent<AutoMove>().atkEffect!=null)
                        go.GetComponent<AutoMove>().atkEffect.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    break;
                case 3:
                    cSlider = Instantiate(selfSlider, star3_position, transform.rotation);
                    cSlider.transform.SetParent(go.transform);
                    go.GetComponent<BoxCollider>().size = star3_size;
                     if(go.GetComponent<AutoMove>().atkEffect!=null)
                        go.GetComponent<AutoMove>().atkEffect.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    break;

            }
            
        }
        else
        {
            var eSlider = Instantiate(enemySlider, enemy_slider_position, transform.rotation);
            eSlider.transform.SetParent(go.transform);
            go.GetComponent<BoxCollider>().size = star3_size;
            go.AddComponent<NavMeshAgent>();    //仅仅在初始化敌人时，添加NavMesh，初始化己方棋子时不添加，在ready阶段添加
        }



        var cBench = Instantiate(benchCollider, go.transform.position, transform.rotation);
        cBench.transform.SetParent(go.transform);
    }


    public bool NoBatNow(){
        var battleChessss = GameObject.FindGameObjectsWithTag("BattleChess");
        int batnum =0;
        foreach(var b in battleChessss){
            if(b.name=="Bat"){
                batnum +=1;
                if(b.GetComponent<Character>().star == 1)
                    AddBat.star1 = true;
                if(b.GetComponent<Character>().star == 2)
                    AddBat.star2 = true;
                if(b.GetComponent<Character>().star == 3)
                    AddBat.star3 = true;
            }
                
                
        }

        if(batnum == 0){
            AddBat.star1 = false;
            AddBat.star2 = false;
            AddBat.star3 = false;
            return true;
        }else{          
            return false;
        }
        
    }


    void addEnemy(GameObject[] go, int i, int id, float x, float z)
    {

        go[i] = Instantiate(enemyModel[id-1], new Vector3(x, 1.00f, z), transform.rotation);
        go[i].transform.localEulerAngles = new Vector3(0f, 180f, 0f);                             //change the rotation
        go[i].name = GameControlSystem.enemyDataScript.allChessData[id-1].chessName;
        int star = GameControlSystem.enemyDataScript.allChessData[id-1].star;
        addComponentForChess(go[i], id, star, true);
    }
   
    void randomBottle(int line1, int line2)
    {
        int line = UnityEngine.Random.Range(line1,line2);
        switch (line)
        {
            case 0:
                int b = UnityEngine.Random.Range(0, 3);
                switch (b)
                {
                    case 0:
                        line1_r.SetActive(true);
                        break;
                    case 1:
                        line1_g.SetActive(true);
                        break;
                    case 2:
                        line1_b.SetActive(true);
                        break;
                }
                break;
            case 1:
                int bb = UnityEngine.Random.Range(0, 3);
                switch (bb)
                {
                    case 0:
                        line3_r.SetActive(true);
                        break;
                    case 1:
                        line3_g.SetActive(true);
                        break;
                    case 2:
                        line3_b.SetActive(true);
                        break;
                }
                break;
            case 2:
                int bbb = UnityEngine.Random.Range(0, 3);
                switch (bbb)
                {
                    case 0:
                        line5_r.SetActive(true);
                        break;
                    case 1:
                        line5_g.SetActive(true);
                        break;
                    case 2:
                        line5_b.SetActive(true);
                        break;
                }
                break;
        }
    }
}
