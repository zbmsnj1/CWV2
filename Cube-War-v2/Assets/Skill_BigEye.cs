using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_BigEye : Skill
{
    // Start is called before the first frame update
    private GameObject atkEffect1;
    private GameObject atkEffect2;

    public override void Awake()
    {
        base.Awake();
        atkEffect1 = Instantiate(character.atk_Effect1, transform.position, transform.rotation);
        atkEffect1.transform.SetParent(transform);
        //atkEffect.GetComponent<Trajectory>().enabled = true;
        atkEffect1.name = "Atk_Effect1";
        atkEffect1.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        
        //go.GetComponent<AutoMove>().atkEffect = atkEffect;
        atkEffect2 = Instantiate(character.atk_Effect2, transform.position, transform.rotation);
        atkEffect2.transform.SetParent(transform);
        //atkEffect.GetComponent<Trajectory>().enabled = true;
        atkEffect2.name = "Atk_Effect2";
        atkEffect2.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

    }

    public override void BuffSkill(){
        
    }
 
    public override void AttackSkll()
    {
        if(autoMove.dead() || autoMove.kill() || autoMove.winAround()){
            atkEffect1.SetActive(false);
            atkEffect2.SetActive(false);
            return;
        }
        if(character.star == 1)
            return;

        if(character.star == 2){
            if(autoMove.enemy1!=null)
                atkEffect1.SetActive(true);
            else
                atkEffect1.SetActive(false);
        }

        if(character.star == 3){
            if(autoMove.enemy2!=null && autoMove.enemy1!=null){
                atkEffect1.SetActive(true);
                atkEffect2.SetActive(true);
            }
            if(autoMove.enemy2==null && autoMove.enemy1!=null){
                atkEffect1.SetActive(true);
                atkEffect2.SetActive(false);
            }
            
        }
        if(autoMove.enemy2==null && autoMove.enemy1==null){
                atkEffect1.SetActive(false);
                atkEffect2.SetActive(false);
            }
        

        //atkEffect2.SetActive(true);
        
        //go.GetComponent<AutoMove>().atkEffect = atkEffect;
    }

    public override void AttackEnemy(){
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            //slider.value -= 20;
            float randValue = Random.value;
            float damage;
            if(autoMove.enemy2!=null && autoMove.enemy1!=null){
                if(randValue < character.now_Baoji){                    
                    damage = character.now_AtkDmg*(40/(40+autoMove.enemy.GetComponent<Character>().now_Armor))*character.now_BaojiDmg;                   
                }else{
                    damage = character.now_AtkDmg*(40/(40+autoMove.enemy.GetComponent<Character>().now_Armor));
                }

                if(character.star == 1){
                    autoMove.enemy.GetComponent<Character>().now_HP -= damage*1.2f;
                    Hurt(damage*1.2f, autoMove.enemy, true);
                    LifeSteal(damage);
                }

                if(character.star == 2){
                    //主箭 80%
                    autoMove.enemy.GetComponent<Character>().now_HP -= damage*0.8f;
                    Hurt(damage*0.8f, autoMove.enemy, true);
                    LifeSteal(damage*0.8f);
                    //副箭各60%
                    autoMove.enemy1.GetComponent<Character>().now_HP -= damage*0.6f;
                    Hurt(damage*0.6f, autoMove.enemy1, true);
                    LifeSteal(damage*0.3f);
                }

                if(character.star == 3){
                    //主箭 80%
                    autoMove.enemy.GetComponent<Character>().now_HP -= damage*0.8f;
                    Hurt(damage*0.8f, autoMove.enemy, true);
                    LifeSteal(damage*0.8f);
                    //副箭各60%
                    autoMove.enemy1.GetComponent<Character>().now_HP -= damage*0.6f;
                    Hurt(damage*0.6f, autoMove.enemy1, true);
                    LifeSteal(damage*0.3f);
                    autoMove.enemy2.GetComponent<Character>().now_HP -= damage*0.6f;
                    Hurt(damage*0.6f, autoMove.enemy2, true);
                    LifeSteal(damage*0.3f);
                }

                
                
            }
            if(autoMove.enemy2==null && autoMove.enemy1!=null){
                if(randValue < character.GetComponent<Character>().now_Baoji){                    
                    damage = character.now_AtkDmg*(40/(40+autoMove.enemy.GetComponent<Character>().now_Armor))*character.now_BaojiDmg;                   
                }else{
                    damage = character.now_AtkDmg*(40/(40+autoMove.enemy.GetComponent<Character>().now_Armor));
                }
                
                if(character.star == 1){
                    autoMove.enemy.GetComponent<Character>().now_HP -= damage*1.2f;
                    Hurt(damage*1.2f, autoMove.enemy, true);
                    LifeSteal(damage);
                }else{
                    //主箭 80%
                    autoMove.enemy.GetComponent<Character>().now_HP -= damage*0.8f;
                    Hurt(damage*0.8f, autoMove.enemy, true);
                    LifeSteal(damage*0.8f);
                    //副箭各60%
                    autoMove.enemy1.GetComponent<Character>().now_HP -= damage*0.6f;
                    Hurt(damage*0.6f, autoMove.enemy1, true);
                    LifeSteal(damage*0.3f);
                }

                
            }
            if(autoMove.enemy2==null && autoMove.enemy1==null){
                if(randValue < character.GetComponent<Character>().now_Baoji){                    
                    damage = character.now_AtkDmg*(40/(40+autoMove.enemy.GetComponent<Character>().now_Armor))*character.now_BaojiDmg;                   
                }else{
                    damage = character.now_AtkDmg*(40/(40+autoMove.enemy.GetComponent<Character>().now_Armor));
                }
                //主箭 120%
                autoMove.enemy.GetComponent<Character>().now_HP -= damage*1.2f;
                Hurt(damage*1.2f, autoMove.enemy, true);
                LifeSteal(damage);
                
            }


            
            attackTimer = (1 / character.now_AtkSpeed) * 0.5f;


        }
    }
    
}
