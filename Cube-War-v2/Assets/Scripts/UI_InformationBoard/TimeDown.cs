using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class TimeDown : MonoBehaviour
{
    public GameControlSystem gameControlSystem;
    public Text gameStateText;
    public Text roundStateText;
    public Text roundText;
    public Text timeDown;
    public Text goldText;
    public Text chessNumText;
    public int downTime = 30;
    public int prepareTime;
    public int readyTime;
    public int battleTime;
    public int autoState = 0;
    public static int ROUND = 1;
    public static bool NEW_ROUND;

    float chessNum;

    public enum Game_State{
        Prepare,   //准备时间，可以购买棋子，摆放位置
        Ready, //记录场上棋子摆放位置
        Battle//战斗回合
    }

    public static Game_State GAME_STATE;
    
    // Start is called before the first frame update
    void Start()
    {   
        GAME_STATE = Game_State.Prepare;
        StartCoroutine("LoseTime");
        Time.timeScale = 1;
        NEW_ROUND = true;                    //初始设定为true，当GameControlSystem 执行完addToChessPool后，设置false
        gameStateText.text = "准备回合";
        roundText.text = ("Round " + ROUND);
    }

    // Update is called once per frame
    void Update()
    {
        chessNum = GameObject.FindGameObjectsWithTag("BattleChess").Length;
        timeDown.text = ("" + downTime);
        goldText.text = ("" + GameControlSystem.gold);
        chessNumText.text = (chessNum + "/" + gameControlSystem.maxChessNum);

        if(downTime == 0){
            switch(GAME_STATE){
                case Game_State.Prepare:
                    GAME_STATE = Game_State.Ready;
                    //newRound = false;
                    gameStateText.text = "战斗即将打响！！！";
                    roundStateText.text = "xxx";
                    downTime = readyTime;
                    break;
                case Game_State.Ready:
                    GAME_STATE = Game_State.Battle;
                    gameStateText.text = "战斗回合";
                    roundStateText.text = "xxx";
                    downTime = battleTime;
                    break;
                case Game_State.Battle:
                    GAME_STATE = Game_State.Prepare;
                    ROUND++;
                    NEW_ROUND = true;                    //战斗回合结束后，设置为True
                    gameStateText.text = "准备回合";
                    roundStateText.text = "xxx";
                    roundText.text = ("Round " + ROUND);
                    downTime = prepareTime;
                    break;
                default:
                    break;
            }
        }

       
    }

    IEnumerator LoseTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            downTime--;
        }
    }

    void ResetDownTime(int time)
    {
        downTime = time;      
    }

    public void ButtonStartGame(){
        if(GAME_STATE == Game_State.Prepare){
            downTime =0;
        }
        
    }
}
