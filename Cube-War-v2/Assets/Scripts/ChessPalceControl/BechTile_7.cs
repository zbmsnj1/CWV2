using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BechTile_7 : MonoBehaviour
{
    private bool triggered = false;
    private Collider other;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (triggered && !other)
        {
            ChessStruct.hasChess[7] = false;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        triggered = true;
        this.other = collider;
        ChessStruct.hasChess[7] = true;
    }

    void OnTriggerExit(Collider collider)
    {
        ChessStruct.hasChess[7] = false;
    }
}
