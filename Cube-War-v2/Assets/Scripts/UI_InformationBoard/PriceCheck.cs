using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriceCheck : MonoBehaviour
{
    public Text cost;
    int price;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        price = int.Parse(cost.text);

        if (price > GameControlSystem.gold)
        {
            this.GetComponent<Button>().interactable = false;
        }
        else
            this.GetComponent<Button>().interactable = true;
    }
}
