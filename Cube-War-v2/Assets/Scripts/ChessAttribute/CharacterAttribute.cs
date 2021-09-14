using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAttribute : MonoBehaviour
{
    public struct Hero
    {
        public string axe;
        public string bm;
        public string sb;
        public string slardar;
        public string abaddon;
        public string broodmother;
        public string lich;
        public string wk;
    }

    public struct Race
    {
        public string Orc;
        public string Undead;
        public string Troll;
        public string Goblin;
    }

    public struct Class_
    {
        public string Warrior;
        public string Hunter;
        public string Assassin;
    }

    public Hero heros = new Hero();
    public Race race = new Race();
    public Class_ class_ = new Class_();

    // Start is called before the first frame update
    void Start()
    {
        race.Orc = "Orc";
        race.Undead = "Undead";
        race.Troll = "Troll";
        race.Goblin = "Goblin";

        class_.Warrior = "Warrior";
        class_.Hunter = "Hunter";
        class_.Assassin = "Assassin";

        heros.axe = "Axe";
        heros.bm = "Beast Maste";
        heros.sb = "Spirit Breaker";
        heros.slardar = "Slardar";
        heros.abaddon = "Abaddon";
        heros.broodmother = "Broodmother";
        heros.lich = "Lich";
        heros.wk = "Wraith King";


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
