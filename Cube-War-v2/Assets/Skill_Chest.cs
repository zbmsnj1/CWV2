using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Chest : Skill
{
    // Start is called before the first frame update


    public override void BuffSkill()
    {
        if(character.star == 1){
            character.now_BaojiDmg = 3f;
        }
        if(character.star == 2){
            character.now_BaojiDmg = 4.5f;
        }
        if(character.star == 3){
            character.now_BaojiDmg = 6f;
        }
        
    }
}
