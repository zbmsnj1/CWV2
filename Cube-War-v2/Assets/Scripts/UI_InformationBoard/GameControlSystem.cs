using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;




public class GameControlSystem : MonoBehaviour
{
    public GameObject allChessInfo_Object;
    public GameObject chessDataGameObject;
    public GameObject enemyDataGameObject;
    public TimeDown timeDown;
    public int maxChessNum = 1;
    public static int gold = 200;
    public static bool delete;
    
    public GameObject RefreshChessBoard;
    public Text goldInfo;
    public Text cnInfo;
    public GameObject buttonLock;
    public GameObject buttonUnLock;
    public GameObject buttonRefreshImg;
    public GameObject buttonCnInfoImg;
    public Text attack_text;
    public Text atkSpeed_text;
    public Text HPregen_text;
    public Text armor_text;
    public Text lifeSteal_text;
    public Text hp_text;
    public Text baoji_text;

    public GameObject star1;
    public GameObject star2;
    public GameObject star3;
    public Text chessName;
    public Text RaceClassText;

    private bool hasRefreshed;
    private bool refreshGold;
    private int basicGold;
    private int interestGold;
    private int currExp = 20;
    private static int MIN_CHESSPOOL = 5;
    private int exp;
    private bool lockChess;

    private GameObject[] battleChess;

    private ChessStruct chessStructScript;

    public Texture2D cursorDelete;
    public Texture2D cursor;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

    System.Random rand = new System.Random();

    public static List<int> chessPool = new List<int>();
    private List<int> tempPool = new List<int>();             //for lock chess function
    public static int[] buy = new int[5];

    [Serializable]
    public struct bench_area
    {
        public Vector3 pos;       
    }


    [Serializable]
    public struct refreshChess
    {
        public GameObject infomation;
        public GameObject gameObject_button;
        public Text name;
        public Text info;
        public Text price;
        public RawImage rawImage;
        public int id;
    }

    public refreshChess[] nowChesses;

    public static ChessDataScript chessDataScript;
    public static EnemyDataScript enemyDataScript;
    // Start is called before the first frame update
    void Awake(){
        //初始化所有棋子信息数据（名字，攻击，防御，等等）
        chessDataScript =  chessDataGameObject.GetComponent<ChessDataScript>();
        enemyDataScript =  enemyDataGameObject.GetComponent<EnemyDataScript>();
        chessStructScript = allChessInfo_Object.GetComponent<ChessStruct>();
        
    }


    void Start()
    {
        
        hasRefreshed = false;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (TimeDown.GAME_STATE == TimeDown.Game_State.Prepare)
        {
            if(TimeDown.NEW_ROUND){
                addToChessPool();               //每回合添加新的棋子进入棋子池
                chessStructScript.resetChessPos(); //每回合重置棋子位置
                TimeDown.NEW_ROUND = false;
            }
            
            if(!hasRefreshed){
                RandomChess();              //每回合刷新棋子
                hasRefreshed = true;
            }

            if(!refreshGold){
                addGoldbyRound();
                refreshGold = true;
            }    
            
            chessStructScript.addEnemy();           //添加敌人

            
                   
        }

        if(TimeDown.GAME_STATE == TimeDown.Game_State.Ready){
            RemoveExtraChess();
            //chessStructScript.recordChessPos();     //记录棋子位置, 现在把这个放在RemoveExtraChess()里了
            hasRefreshed = false;
            refreshGold = false;
            Debug.Log("Ready-State Done.");
        }
        
        chessStructScript.addChesstoBench();    //把棋子添加到bench上面
        
        setCursor();                    //设置删除棋子的按钮         
        checkBuy();
        exptoText();
        goldtoText();
        showGoldExpInfo();
        chessStructScript.checkSameChess();     //如果有3个相同的棋子，就自动升级
        chessStructScript.RunUpStarEffect();  //并执行1秒升级特效
        


    }


    

    void showGoldExpInfo(){
        if (gold < 2 || lockChess)
            buttonRefreshImg.SetActive(true);
        else
            buttonRefreshImg.SetActive(false);

        if (gold < 5)
            buttonCnInfoImg.SetActive(true);
        else
            buttonCnInfoImg.SetActive(false);
    }

    public void buttonRefresh()
    {
        if (gold < 2  || lockChess || chessPool.Count + tempPool.Count < MIN_CHESSPOOL)
            return;
        gold -= 2;                                              //each refresh cost 2 gold
        for (int i = 0; i < tempPool.Count; i++)                //if press refeshButton, reAdd the chess from temp pool to chess pool
        {
            chessPool.Add(tempPool[i]);
        }
        tempPool.Clear();
        resetInfo();                                            //recall information of each refreshed chess
        resetBuy();
        RandomChess();
        
    }

    public void buttonLockDown()
    {
        buttonUnLock.SetActive(true);
        buttonLock.SetActive(false);
        //buttonRefreshImg.SetActive(true);
        lockChess = true;
      
    }

    public void buttonUnLockDown()
    {
        buttonUnLock.SetActive(false);
        buttonLock.SetActive(true);
        //buttonRefreshImg.SetActive(false);
        lockChess = false;
    }


    void RandomChess()
    {
        if (!ChessStruct.initGlobalChesses)                         //make sure ChessStruct.globalChesses has copyed the data to itself
        {
            return;
        }

        if(lockChess){
            return;
        }


        for(int i=0; i<5; i++)
        {
            int ran = rand.Next(chessPool.Count);
            idToText(i, chessPool[ran]);
            tempPool.Add(chessPool[ran]);                       //temporary store chess in temp pool
            chessPool.Remove(chessPool[ran]);
            //Debug.Log(chessPool[i]);
           
        }
       

    }

    void addToChessPool()
    {
        if (!ChessStruct.initGlobalChesses)               //make sure ChessStruct.globalChesses has copyed the data to itself
        {
            return;
        }

         if (!lockChess)
            {
                resetBuy();
                if(chessPool.Count + tempPool.Count >= MIN_CHESSPOOL)
                    resetInfo();
                for (int i = 0; i < tempPool.Count; i++)                //if new round, reAdd the chess from temp pool to chess pool
                {
                    chessPool.Add(tempPool[i]);
                }
                tempPool.Clear();
            }
        currExp += 1;                                               //add 1 exp each round
        RefreshChessBoard.SetActive(true);
        
        


        if(TimeDown.ROUND == 1)
        {
            for (int i = 0; i < 50; i++)
            {
                chessPool.Add(1);
                chessPool.Add(16);
                chessPool.Add(4);
                chessPool.Add(7);
                chessPool.Add(10);
                chessPool.Add(19);
                chessPool.Add(22);
                chessPool.Add(13);
                
            }
            
        }

        if (TimeDown.ROUND == 2)
        {
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(1);
                chessPool.Add(4);
            }
            for (int i = 0; i < 3; i++)
            {                    
                chessPool.Add(4);        //1        
                chessPool.Add(1);
                chessPool.Add(1);
            }
        }
        if (TimeDown.ROUND == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(1);
                chessPool.Add(7);
            }
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(4);  //2
                chessPool.Add(13);
                chessPool.Add(22);
            }
        }
        if (TimeDown.ROUND == 4)
        {
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(1);
                chessPool.Add(7);
            }
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(4);   //3
                chessPool.Add(13);
                chessPool.Add(22);
            }
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(10);        //11
                chessPool.Add(16);
            }
        }
        if (TimeDown.ROUND == 5)
        {
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(1);
                chessPool.Add(7);
            }
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(4);  //4
                chessPool.Add(13);
                chessPool.Add(22);
            }
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(10);        //22
                chessPool.Add(16);
            }
        }
        if (TimeDown.ROUND == 6)
        {
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(1);
                chessPool.Add(7);
            }
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(4);  //5
                chessPool.Add(13);
                chessPool.Add(22);
            }
            for (int i = 0; i < 12; i++)
            {
                chessPool.Add(19);             //1111
            }

        }
        if (TimeDown.ROUND == 7)
        {
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(1);
                chessPool.Add(7);
            }
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(4);  //6
                chessPool.Add(13);
                chessPool.Add(22);
            }
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(10);   //33
                chessPool.Add(16);
            }
        }
        if (TimeDown.ROUND == 8)
        {
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(1);
                chessPool.Add(7);
            }
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(4); //7
                chessPool.Add(13);
                chessPool.Add(22);
            }
            for (int i = 0; i < 12; i++)
            {
                chessPool.Add(19);             //2222
            }
        }
        if (TimeDown.ROUND == 9)
        {
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(1);
                chessPool.Add(7);
            }
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(10);  //44
                chessPool.Add(16);
            }
        }
        if (TimeDown.ROUND == 10)
        {
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(1);
                chessPool.Add(7);
            }
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(4);     //8
                chessPool.Add(13);
                chessPool.Add(22);
            }
            for (int i = 0; i < 12; i++)
            {
                chessPool.Add(19);             //3333
            }
        }
        if (TimeDown.ROUND == 11)
        {
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(1);
                chessPool.Add(7);
            }
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(10);      //55
                chessPool.Add(16);
            }
        }
        if (TimeDown.ROUND == 12)
        {

            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(4);             //9
                chessPool.Add(13);
                chessPool.Add(22);
            }
            for (int i = 0; i < 3; i++)
            {
                chessPool.Add(10);             //66
                chessPool.Add(16);
            }
            for (int i = 0; i < 12; i++)                
            { 
                chessPool.Add(19);             //4444
            }
        }

        

    }

    void idToText(int index, int id)
    {
        nowChesses[index].name.text = chessDataScript.allChessData[id-1].chessName;
        if(chessDataScript.allChessData[id-1].price==1){
            nowChesses[index].name.color=new Color(255/255f,255/255f,255/255f,255/255f);
        }
        if(chessDataScript.allChessData[id-1].price==2){
            nowChesses[index].name.color=new Color(59/255f,117/255f,255/255f,255/255f);
        }
        if(chessDataScript.allChessData[id-1].price>2){
            nowChesses[index].name.color=new Color(231/255f,172/255f,60/255f,255/255f);
        }
        
        nowChesses[index].info.text = chessDataScript.allChessData[id-1]._race_class;
        nowChesses[index].price.text = (""+chessDataScript.allChessData[id-1].price);
        nowChesses[index].id = chessDataScript.allChessData[id-1].id;
        nowChesses[index].rawImage.texture =  ChessStruct.globalChesses[id-1].model.GetComponent<Character>().texture;

    }

    void checkBuy()
    {
        if (ChessStruct.filled > 7)
        {
            setButton(false);
            return;
        }
        setButton(true);
            
        for (int i = 0; i < 5; i++)
        {
            if (buy[i] == 0)
            {
                buy[i] = buyChess(i);

                if (buy[i] != 0)
                {
                    tempPool.Remove(buy[i]);
                    
                }
            }
            
        }
    }

    int buyChess(int i)
    {
        if (!nowChesses[i].infomation.activeInHierarchy && RefreshChessBoard.activeInHierarchy)
        {
            return nowChesses[i].id;
        }
        return 0;
    }

    void resetBuy()
    {
        for (int i = 0; i < 5; i++)
        {
            buy[i] = 0;
            ChessStruct.bought[i] = false;
        }
    }

    void setButton(bool state)
    {
        nowChesses[0].gameObject_button.SetActive(state);
        nowChesses[1].gameObject_button.SetActive(state);
        nowChesses[2].gameObject_button.SetActive(state);
        nowChesses[3].gameObject_button.SetActive(state);
        nowChesses[4].gameObject_button.SetActive(state);
    }

    void addGoldbyRound()               //gold system
    {
        for(int i=1; i<100; i++)                      //for TimeDown.ROUND 1-4, add gold from 1 to 4 each TimeDown.ROUND
        {
            if (TimeDown.ROUND == i )
            {              
                if (TimeDown.ROUND < 5)
                {
                    gold += i;             
                }
                else
                {
                    gold += 5;                   
                }
                gold += interestGold;                //add interest
                break;
            }
            
        }
       
    }

    void goldtoText()
    {
        if (TimeDown.ROUND < 5)
            basicGold = TimeDown.ROUND + 1;
        else
            basicGold = 5;

        if (interestGold < 5)
            interestGold = gold / 10;
        else
            interestGold = 5;

        int next = gold + basicGold + interestGold;
        goldInfo.text = "\n   Next: " + next +
                        "\n\n   Current: " + gold +
                        "\n   Basic: " + basicGold +
                        "\n   Interest: " + interestGold +
                        "\n";
    }

    public void addExpbyGold()
    {
        if (gold < 5)
            return;
        currExp += 4;
        gold -= 5;
    }

    void exptoText()                              //control the maxChess number
    {
        exp = 2 - currExp;
        if (currExp > 1)
        {
            exp = 3 - currExp;
            maxChessNum = 2;
            if(currExp > 2)
            {
                exp = 5 - currExp;
                maxChessNum = 3;
                if (currExp > 4)
                {
                    exp = 9 - currExp;
                    maxChessNum = 4;
                    if (currExp > 8)
                    {
                        exp = 17 - currExp;
                        maxChessNum = 5;
                        if (currExp > 16)
                        {
                            exp = 33 - currExp;
                            maxChessNum = 6;
                            if (currExp > 32)
                            {
                                exp = 57 - currExp;
                                maxChessNum = 7;
                                if (currExp > 56)
                                {
                                    exp = 89 - currExp;
                                    maxChessNum = 8;
                                    if (currExp > 88)
                                    {
                                        exp = 129 - currExp;
                                        maxChessNum = 9;
                                        if (currExp > 128)
                                        {
                                            exp = 0;
                                            maxChessNum = 10;

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        cnInfo.text =   "\n Need: " + exp + " exp"+
                        "\n\n Current: " + currExp + " exp" +
                        "\n\n 5 gold = 4 exp\n";

    }

    void resetInfo()
    {
        nowChesses[0].infomation.SetActive(true);
        nowChesses[1].infomation.SetActive(true);
        nowChesses[2].infomation.SetActive(true);
        nowChesses[3].infomation.SetActive(true);
        nowChesses[4].infomation.SetActive(true);
    }




    void RemoveExtraChess(){   //这是把棋盘上多出人口的棋子，放回chessbench,或者卖掉
        battleChess = GameObject.FindGameObjectsWithTag("BattleChess");
        if(battleChess.Length > maxChessNum){
            ChessStruct.filled = chessStructScript.emptyBenchTile();        //即时更新，filled数字，避免放回棋子，重叠到一个bench位置上。
            if (ChessStruct.filled < 8)
            {
                battleChess[0].transform.position = chessStructScript.benchTiles[ChessStruct.filled].pos;               //reset bad chess position
            }
            else
            {
                int price = 0;
                int id = 0;
                foreach (var chess in ChessStruct.globalChesses)
                {
                    if (chess.model.transform == battleChess[0].transform)
                    {
                        
                        id = chess.model.GetComponent<Character>().id;
                        price = chessDataScript.allChessData[id-1].price;
                        break;
                    }
                }
                if(price != 0)
                {
                    gold += price;                    //delete a chess will refound gold of the chess's price
                    chessPool.Add(id);                //delete a chess will reAdd the chess to chessPool
                    Destroy(battleChess[0]);
                }               
            }
        }else{
            chessStructScript.recordChessPos();     //如果在场chess数量不大于人口，则记录位置
        }
    }


    public void deleteDown()
    {
        if (delete)
            delete = false;
        else
            delete = true;
        
    }


    void setCursor()
    {
        if (delete)
        {
            Cursor.SetCursor(cursorDelete, hotSpot, cursorMode);
        }
        else
        {
            Cursor.SetCursor(cursor, hotSpot, cursorMode);
        }
    }
}


