using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public int id;
    public string chessName;  

    [HideInInspector]
    public int star;
    [HideInInspector]
    public int price;
    [HideInInspector]
    public string _race_class;
    [HideInInspector]
    public float now_Armor;
    [HideInInspector]
    public float now_Baoji;
    [HideInInspector]
    public float now_LifeSteal;
    [HideInInspector]
    public float now_HpRegen;
    [HideInInspector]
    public float now_HP;
    [HideInInspector]
    public float now_Max_HP;
    [HideInInspector]
    public float now_AtkDmg;
    [HideInInspector]
    public float now_AtkSpeed;
    [HideInInspector]
    public float atk_Range;

    public float now_BaojiDmg;

    public GameObject atk_Effect;
    public GameObject atk_Effect1;
    public GameObject atk_Effect2;
 

    public Texture texture;
    public Slider selfSlider;

    public int base_HP;
    public float base_AtkDmg;
    public float base_AtkSpeed;
    public float base_LifeSteal;
    public float base_HpRegen;
    public float base_Baoji;
    public float base_Armor;
    public float base_BaojiDmg;
  

 
    GameControlSystem controlSystem;




    // Start is called before the first frame update

    void Awake()
    {
        controlSystem = GameObject.Find("GameControlSystem").GetComponent<GameControlSystem>();
        
        initData();

        //initCharacter = true;
    }

    void Start(){
        initSelf();     //放在start里保证可以初始化成功，稍微晚于添加slider组件（放在awake里初始化失败）
    }



    // Update is called once per frame
    void Update()
    {
        //initNameRaceClass();
        if(TimeDown.GAME_STATE == TimeDown.Game_State.Battle){
            UpdateHP();
        }
        
        if(TimeDown.GAME_STATE == TimeDown.Game_State.Prepare)
        {
            
            UpdateHP();
        }
       
    }

    public void initData(){
        if(gameObject.tag == "Chess"){       
            //初始化固定不变的   
            chessName = GameControlSystem.chessDataScript.allChessData[id-1].chessName;
            star = GameControlSystem.chessDataScript.allChessData[id-1].star;
            price = GameControlSystem.chessDataScript.allChessData[id-1].price;
            _race_class = GameControlSystem.chessDataScript.allChessData[id-1]._race_class;
            atk_Range = GameControlSystem.chessDataScript.allChessData[id-1].atk_Range;

            //棋子初始属性，后面可能会动态变化
            base_HP = GameControlSystem.chessDataScript.allChessData[id-1].max_HP;
            base_AtkDmg = GameControlSystem.chessDataScript.allChessData[id-1].baseAttack_dmg;
            base_AtkSpeed = GameControlSystem.chessDataScript.allChessData[id-1].atk_speed;
            base_Armor = GameControlSystem.chessDataScript.allChessData[id-1].armor;
            base_Baoji = GameControlSystem.chessDataScript.allChessData[id-1].baoji;
            base_LifeSteal = GameControlSystem.chessDataScript.allChessData[id-1].lifeSteal;
            base_HpRegen = GameControlSystem.chessDataScript.allChessData[id-1].HP_Regen;

            Debug.Log("初始化成功"+transform.name);
        }else if(gameObject.tag == "Enemy"){
            chessName = GameControlSystem.enemyDataScript.allChessData[id-1].chessName;
            star = GameControlSystem.enemyDataScript.allChessData[id-1].star;
            price = GameControlSystem.enemyDataScript.allChessData[id-1].price;
            _race_class = GameControlSystem.enemyDataScript.allChessData[id-1]._race_class;
            atk_Range = GameControlSystem.enemyDataScript.allChessData[id-1].atk_Range;

            base_HP = GameControlSystem.enemyDataScript.allChessData[id-1].max_HP;
            base_AtkDmg = GameControlSystem.enemyDataScript.allChessData[id-1].baseAttack_dmg;
            base_AtkSpeed = GameControlSystem.enemyDataScript.allChessData[id-1].atk_speed;
            base_Armor = GameControlSystem.enemyDataScript.allChessData[id-1].armor;
            base_Baoji = GameControlSystem.enemyDataScript.allChessData[id-1].baoji;
            base_LifeSteal = GameControlSystem.enemyDataScript.allChessData[id-1].lifeSteal;
            base_HpRegen = GameControlSystem.enemyDataScript.allChessData[id-1].HP_Regen;
            
            Debug.Log("初始化成功"+transform.name);
        }
        
        base_BaojiDmg = 1.2f;                   //初始每人都有120%的暴击伤害。
        
        //Debug.Log("初始化成功"+transform.name);

    }

    
    


    //最初创建被call一次，然后新回合还原棋子被call一次
    public void initSelf()
    {
        selfSlider = transform.GetComponentInChildren<Slider>();

        if(selfSlider == null){                 //当血条slider还没被添加，返回
            return;
        }
        now_Max_HP = base_HP;
        now_HP = now_Max_HP;
        now_Armor = base_Armor;
        now_AtkDmg = base_AtkDmg;
        now_AtkSpeed = base_AtkSpeed;
        //now_HpRegen = base_HpRegen;
        now_LifeSteal = base_LifeSteal;
        now_Baoji = base_Baoji;
        now_BaojiDmg = base_BaojiDmg;

        selfSlider.maxValue = base_HP;      

    }


    void UpdateHP()
    {
        if(selfSlider == null){                 //当血条slider还没被添加，返回
            return;
        }

        selfSlider.value = now_HP;

    }

    public void chessInfotoText()
    {
        

        controlSystem.attack_text.text = now_AtkDmg.ToString();
        controlSystem.armor_text.text = now_Armor.ToString();      
        controlSystem.atkSpeed_text.text = now_AtkSpeed.ToString();
        //controlSystem.HPregen_text.text = now_HpRegen.ToString();
        controlSystem.baoji_text.text = now_Baoji.ToString();
        controlSystem.lifeSteal_text.text = now_LifeSteal.ToString();
        controlSystem.hp_text.text = now_HP.ToString();

        controlSystem.chessName.text = chessName.ToString();
        controlSystem.RaceClassText.text = _race_class.ToString();

        switch (star)
        {
            case 1:
                controlSystem.star1.SetActive(true);
                controlSystem.star2.SetActive(false);
                controlSystem.star3.SetActive(false);
                break;
            case 2:
                controlSystem.star1.SetActive(false);
                controlSystem.star2.SetActive(true);
                controlSystem.star3.SetActive(false);
                break;
            case 3:
                controlSystem.star1.SetActive(false);
                controlSystem.star2.SetActive(false);
                controlSystem.star3.SetActive(true);
                break;
        }
    }





    /*Character and enemy 
     * 
     * C:
     * zombie: hp 200 attack 35
     * maw-j: hp 330 attack 40
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * E:
     * round 1: gold zombie:  hp 150 attack 30
     * round 2: gold maw-j: hp 300 attack 45
     * round 3: cold zombi: hp 1250 attack 160
     * round 5: cold maw-j: hp 850 attack 180
     */



    /* 
     Orc: 兽王  斧王  裂魂人   斯拉达
     Undead: 亚巴顿   维萨吉  巫妖    韵母蜘蛛

 */
}
