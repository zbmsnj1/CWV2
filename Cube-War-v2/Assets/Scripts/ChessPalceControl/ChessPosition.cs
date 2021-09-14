using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class ChessPosition : MonoBehaviour
{
    private ChessPlaceControl chessPlaceControl;
    private GameControlSystem controlSystem;
    private ChessStruct chessStructScript;

    private Character characterScript;
    public GameObject deleteButton;
    private GameObject selectedChess;
    private GameObject chessInfoUI;
    private GameObject chessInfoUI_child;
    private bool setButton;
    private int MoveSpeed =15;
    private int deleteHeight = 10;
    private bool delete;
    private float enemyArea_posZ = 2.2f;

    void Awake(){
                      //get control script
        controlSystem = GameObject.Find("GameControlSystem").GetComponent<GameControlSystem>();
        chessPlaceControl = GameObject.Find("ChessPlaceControl").GetComponent<ChessPlaceControl>();
        chessStructScript = GameObject.Find("AllChessInfo").GetComponent<ChessStruct>();
        characterScript = transform.GetComponent<Character>();
    }



    // Update is called once per frame
    void Update()
    {

        CheckPos();     //不管是prepare，ready，battle，都检查位置
        if (TimeDown.GAME_STATE == TimeDown.Game_State.Prepare){

        }else{
            resetChessPlaceControl();   //不允许在其他准备回合之外移动棋子
        }
        //target = new Vector3(transform.position.x, deleteHeight, transform.position.z);
        //resetChessPlaceControl();
        
        //deleteThis(delete);

        if (chessPlaceControl.chessIsSelected == true)
        {
            selectedChess = chessPlaceControl.currentChess;
            selectedChess.GetComponent<Outline>().enabled = true;
            
        }
        else
        {
            
            //chessInfoUI.SetActive(false);
            if (selectedChess == null)
                return;
            selectedChess.GetComponent<Outline>().enabled = false;
            selectedChess = null;
            
        }

        

    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
           
            


        }

        if (Input.GetMouseButtonDown(0))                                        //on mouse down left click  
        {
            characterScript.chessInfotoText();  //鼠标点下，显示棋子信息

            if(GameControlSystem.delete == true)
            {
                if(gameObject.tag == "Chess")
                    deleteThis();

                if(gameObject.tag == "BattleChess" && TimeDown.GAME_STATE == TimeDown.Game_State.Prepare)
                    deleteThis();
            }
           
            if (TimeDown.GAME_STATE != TimeDown.Game_State.Prepare || transform.position.z > enemyArea_posZ)                   //only can select chess on prepare state
            {
                return;
            }
            if (chessPlaceControl.currentChess == null)
            {
                chessPlaceControl.currentChess = this.transform.gameObject;
                
            }
            else
            {
                chessPlaceControl.targetChess = this.transform.gameObject;
            }
        }

        if (Input.GetMouseButtonUp(0))                                           //on mouse up left click  
        {
            
            if (TimeDown.GAME_STATE != TimeDown.Game_State.Prepare)                     //only can select chess on prepare state
            {
                return;
            }
            if (chessPlaceControl.currentChess == null && chessPlaceControl.targetChess == null)
            {
                chessPlaceControl.chessIsSelected = false;
            }
            else
            {
                chessPlaceControl.chessIsSelected = true;
            }
        }
    }

 

    void resetChessPlaceControl()
    {
        
        chessPlaceControl.currentChess = null;
        chessPlaceControl.targetChess = null;
        chessPlaceControl.chessIsSelected = false;
    }


    

    void deleteThis()
    {
        switch (gameObject.GetComponent<Character>().star)
        {
            case 1:               
                GameControlSystem.chessPool.Add(gameObject.GetComponent<Character>().id);
                break;
            case 2:
                for(int i=0; i<3; i++)
                {
                    GameControlSystem.chessPool.Add(gameObject.GetComponent<Character>().id - 1);
                }
                break;
            case 3:
                for (int i = 0; i < 9; i++)
                {
                    GameControlSystem.chessPool.Add(gameObject.GetComponent<Character>().id - 2);
                }
                break;
        }
        
        GameControlSystem.gold += gameObject.GetComponent<Character>().price;
        Destroy(this.gameObject);

    }


    void CheckPos(){
        if(gameObject.tag == "Enemy" || gameObject.tag == "DieChess" || gameObject.tag == "DeadEnemy")
            return;

        if(transform.position.z>ChessStruct.benchPosZZ){
            if(gameObject.tag == "BattleChess"){
                return;
            }else{
                gameObject.tag = "BattleChess";
                gameObject.transform.SetParent(chessStructScript.BattleChess.transform);
            }            
        }
        else{
            if(gameObject.tag == "Chess"){
                return;
            }else{
                gameObject.tag = "Chess";
                gameObject.transform.SetParent(chessStructScript.OurChess.transform);
            }
        }
            
    }


  

}
