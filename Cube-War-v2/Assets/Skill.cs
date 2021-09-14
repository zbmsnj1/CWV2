using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float attackTimer = 0.5f;
    public AutoMove autoMove;
    public ChessStruct chessStruct;
    private bool getAM;

    public Character character;
    public virtual void Awake() {
        character = transform.GetComponent<Character>();
        chessStruct = GameObject.Find("AllChessInfo").GetComponent<ChessStruct>();
        getAM = false;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        BuffSkill();


        GetAutoMove();
        
        if(autoMove!=null && autoMove.isAttacking){
            
            AttackEnemy();
            AttackSkll();
        }
    }



    public virtual void AttackSkll(){
        
    }

    public virtual void BuffSkill(){
        var battChess = GameObject.FindGameObjectsWithTag("BattleChess");
        if(gameObject.tag == "BattleChess"){

            if(character.star == 1 && chessStruct.AddBat.star1  ){
                foreach(var ch in battChess){
                    ch.GetComponent<Character>().now_LifeSteal = ch.GetComponent<Character>().base_LifeSteal;
                    ch.GetComponent<Character>().now_LifeSteal += 0.1f;
                }
            }

            if(character.star == 2 && chessStruct.AddBat.star2 ){
                foreach(var ch in battChess){
                    ch.GetComponent<Character>().now_LifeSteal = ch.GetComponent<Character>().base_LifeSteal;
                    ch.GetComponent<Character>().now_LifeSteal += 0.2f;
                }
            }

            if(character.star == 3 && chessStruct.AddBat.star3){
                foreach(var ch in battChess){
                    ch.GetComponent<Character>().now_LifeSteal = ch.GetComponent<Character>().base_LifeSteal;
                    ch.GetComponent<Character>().now_LifeSteal += 0.3f;
                }
            }

            
        }

    


        

    }

    public virtual void AttackEnemy(){

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            //slider.value -= 20;
            float randValue = Random.value;
            float damage;
            if(randValue < character.now_Baoji){
                
                damage = character.now_AtkDmg*(40/(40+autoMove.enemy.GetComponent<Character>().now_Armor))*character.now_BaojiDmg;
                autoMove.enemy.GetComponent<Character>().now_HP -= damage;
                Hurt(damage, autoMove.enemy, true);

                
            
            }else{
                damage = character.now_AtkDmg*(40/(40+autoMove.enemy.GetComponent<Character>().now_Armor));
                autoMove.enemy.GetComponent<Character>().now_HP -= damage;
                Hurt(damage, autoMove.enemy, false);
            }

            
            if(autoMove.enemy.GetComponent<Character>().name == "Turtle"){
                    if(autoMove.enemy.GetComponent<Character>().star == 1){
                        character.now_HP -= damage*0.15f;
                        Hurt(damage*0.15f, gameObject, false);
                    }
                    if(autoMove.enemy.GetComponent<Character>().star == 2){
                        character.now_HP -= damage*0.25f;
                        Hurt(damage*0.25f, gameObject, false);
                    }
                    if(autoMove.enemy.GetComponent<Character>().star == 3){
                        character.now_HP -= damage*0.35f;
                        Hurt(damage*0.35f, gameObject, false);
                    }
            }

            LifeSteal(damage);
            attackTimer = (1 / character.now_AtkSpeed) * 0.5f;
            /*            attackTimer = 0.5f;*/
        }
    }

    public void LifeSteal(float damage){
        if(character.now_LifeSteal == 0)
            return;

        
        if(character.now_HP + character.now_LifeSteal*damage >=character.now_Max_HP){
            character.now_HP = character.now_Max_HP;
            return;
        }

        character.now_HP += character.now_LifeSteal*damage;
        
    }

    public void Hurt(float damage, GameObject enemy, bool isBaoji)
    {
        GameObject o = new GameObject("-x");
        o.transform.position = enemy.transform.position + Vector3.up * Mathf.Lerp(4f, 2f, enemy.GetComponent<Character>().now_HP / enemy.GetComponent<Character>().now_Max_HP);
        o.transform.rotation = Camera.main.transform.rotation;
        TextMesh tm = o.AddComponent<TextMesh>();
        

        tm.text = damage.ToString("0");
        if(!isBaoji){
            if(enemy.tag=="BattleChess")
                tm.characterSize = 0.3f;
            else
                tm.characterSize = 0.2f;
        }else{
            tm.characterSize = 0.4f; 
            tm.fontStyle = FontStyle.Bold;
        }
        
        if(enemy.tag=="BattleChess"){
            tm.color = new Color(0.9f, 0.9f, 0.9f, 1f);
        }else{
            if(enemy.name == gameObject.name){                          //打乌龟，自身反弹伤害
                tm.color = new Color(251/255f, 245/255f, 115/255f, 1f);
            }else
                tm.color = new Color(0.9f, 0.3f, 0.3f, 1f);
        }
        
        Destroy(o, 1f);
    }


    void GetAutoMove(){
        if(transform.GetComponent<AutoMove>() !=null && !getAM){
            getAM = true;
            autoMove = transform.GetComponent<AutoMove>();
            
        }
    }


}
