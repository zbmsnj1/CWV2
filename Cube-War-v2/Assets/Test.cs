using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
        private ChessStruct chessStructScript;
    // Start is called before the first frame update
    void Start()
    {
        chessStructScript = GameObject.Find("AllChessInfo").GetComponent<ChessStruct>();
    }

    // Update is called once per frame
    void Update()
    {
        if(chessStructScript!=null){
            Debug.Log("FIND!");
        }
    }
}
