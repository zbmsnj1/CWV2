using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class RaceClassCheck : MonoBehaviour
{
    public Image fireIMG_1;
    public Image fireIMG_2;
    public Image waterIMG_1;
    public Image waterIMG_2;
    public Image earthIMG_1;
    public Image earthIMG_2;
    public Image woodIMG_1;
    public Image woodIMG_2;


    private bool addAtk;
    private bool addAtkSpeed;
    private bool addMaxHP;
    private bool addArmor;


    GameObject[] battleChesses;

    void Awake(){
        addArmor = false;
        addAtk = false;
        addAtkSpeed = false;
        addMaxHP = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(TimeDown.GAME_STATE == TimeDown.Game_State.Prepare){
            CheckRC();
        }

        

    }

    //元素共鸣只在prepare阶段检测添加，并在ready阶段锁死
    void CheckRC(){
        battleChesses = GameObject.FindGameObjectsWithTag("BattleChess");

        var uniqueBattleChesses = battleChesses.GroupBy(x => x.GetComponent<Character>().chessName).Select(x => x.First()).ToList();
    
        var query = uniqueBattleChesses.SelectMany(x => x.GetComponent<Character>()._race_class)
                .GroupBy(s => s)
                .Select(g => new { Name = g.Key, Count = g.Count() });

        int rc_num =0;

        
        resetAttri();

        foreach(var result in query) {          

            if(result.Name.ToString() == "土"){
                if(result.Count >= 1){
                    earthIMG_1.GetComponent<Image>().enabled = true;
                    if(result.Count >=2){
                        earthIMG_2.GetComponent<Image>().enabled = true;
                        AddArmor();
                    }
                }
                rc_num +=1;               
            }

            if(result.Name.ToString() == "火"){
                if(result.Count >= 1){
                    fireIMG_1.GetComponent<Image>().enabled = true;
                    if(result.Count >=2){
                        fireIMG_2.GetComponent<Image>().enabled = true;
                        AddAtk();
                    }
                }
                rc_num +=1;               
            }

            if(result.Name.ToString() == "木"){
                if(result.Count >= 1){
                    woodIMG_1.GetComponent<Image>().enabled = true;
                    if(result.Count >=2){
                        woodIMG_2.GetComponent<Image>().enabled = true;
                        AddAtkSpeed();
                    }
                }  
                rc_num +=1;               
            }

            if(result.Name.ToString() == "水"){
                if(result.Count >= 1){
                    waterIMG_1.GetComponent<Image>().enabled = true;
                    if(result.Count >=2){
                        waterIMG_2.GetComponent<Image>().enabled = true;
                        AddMaxHp();
                    }
                } 
                rc_num +=1;                
            }

        }

        //当每种元素都在场
        if(rc_num == 4){
            Debug.Log("金木水火土");
        }

    }

    



    void AddMaxHp()
    {
        if(!addMaxHP){
            foreach(var chess in battleChesses){
            chess.GetComponent<Character>().now_Max_HP = (float) Math.Round(chess.GetComponent<Character>().now_Max_HP +200f);
            chess.GetComponent<Character>().now_HP = chess.GetComponent<Character>().now_Max_HP;
            }
            addMaxHP = true;
        }
        
    }


    void AddArmor()
    {
        if(!addArmor){
            foreach(var chess in battleChesses){
            chess.GetComponent<Character>().now_Armor = (float) Math.Round(chess.GetComponent<Character>().now_Armor*1.2f);
            }
            addArmor = true;
        }

    }

    void AddAtkSpeed()
    {
        if(!addAtkSpeed){
            foreach(var chess in battleChesses){
            chess.GetComponent<Character>().now_AtkSpeed = (float) Math.Round(chess.GetComponent<Character>().now_AtkSpeed*1.2f);
            }
            addAtkSpeed = true;
        }
        
    }


    void AddAtk(){
        if(!addAtk){
            foreach(var chess in battleChesses){
            chess.GetComponent<Character>().now_AtkDmg = (float) Math.Round(chess.GetComponent<Character>().now_AtkDmg*1.2f);
            }
            addAtk = true;
        }
        
    }

    void resetAttri(){
        fireIMG_1.GetComponent<Image>().enabled = false;
        fireIMG_2.GetComponent<Image>().enabled = false;
        earthIMG_1.GetComponent<Image>().enabled = false;
        earthIMG_2.GetComponent<Image>().enabled = false;
        woodIMG_1.GetComponent<Image>().enabled = false;
        woodIMG_2.GetComponent<Image>().enabled = false;
        waterIMG_1.GetComponent<Image>().enabled = false;
        waterIMG_2.GetComponent<Image>().enabled = false;

        GameObject[] benchChesses = GameObject.FindGameObjectsWithTag("Chess");

        GameObject[] all_Chesses = battleChesses.Concat(benchChesses).ToArray();
        
        foreach(var chess in all_Chesses){
            chess.GetComponent<Character>().now_AtkDmg = chess.GetComponent<Character>().base_AtkDmg;
            chess.GetComponent<Character>().now_AtkSpeed = chess.GetComponent<Character>().base_AtkSpeed;
            chess.GetComponent<Character>().now_Armor = chess.GetComponent<Character>().base_Armor;
            chess.GetComponent<Character>().now_Max_HP = chess.GetComponent<Character>().base_HP;
            chess.GetComponent<Character>().now_HP = chess.GetComponent<Character>().now_Max_HP;

        }

        addAtk = false;
        addAtkSpeed = false;
        addArmor = false;
        addMaxHP = false;
    }

    
}
