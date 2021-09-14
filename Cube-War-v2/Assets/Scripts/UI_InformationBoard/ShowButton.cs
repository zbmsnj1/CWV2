using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowButton : MonoBehaviour
{
    public GameObject RefreshChessBoard;
    public GameObject buttonHide;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (RefreshChessBoard.activeInHierarchy)
        {
            buttonHide.SetActive(true);
        }else
            buttonHide.SetActive(false);
    }
}
