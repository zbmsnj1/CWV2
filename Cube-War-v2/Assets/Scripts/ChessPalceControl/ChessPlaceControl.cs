using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChessPlaceControl : MonoBehaviour
{
    

    public GameObject currentChess;
    public GameObject targetChess;
    public Material materialSelected;
    public bool chessIsSelected;
    private GameObject targetTile;

    


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {


        //currentChess = ccurrentChess.pickedcurrentChess;

        
            if (Input.GetMouseButton(0))
            {
                

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  //camare2D.ScreenPointToRay (Input.mousePosition);
                RaycastHit hit;

                if (GameControlSystem.delete || chessIsSelected)
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        targetTile = hit.collider.transform.gameObject;
                  

                        if (currentChess != null )
                        {

                            if (targetChess != null)
                            {
                                Debug.Log("Swap Chess Position");
                                SwapPosition(currentChess, targetTile);

                            }else if(CheckArea(targetTile) == true){
                                MoveToPosition(currentChess, targetTile);
                            }

                            

                        }

                    }
                    currentChess = null;
                    targetChess = null;
                    chessIsSelected = false;
                    GameControlSystem.delete = false;
                }

                

        }
        if (Input.GetMouseButtonDown(1))                      //cancel the selection of a chess when right click on board
        {
            currentChess = null;
            targetChess = null;
            chessIsSelected = false;
            GameControlSystem.delete = false;
        }

    }


    void MoveToPosition(GameObject g1, GameObject g2)
    {
        
        float xx, zz, yy;
        //float yy = 1.0F;

        xx = g2.GetComponent<Transform>().position.x;
        zz = g2.GetComponent<Transform>().position.z;
        yy = g2.GetComponent<Transform>().position.y;
        if(g2.name == "Cube"){
            yy += 0.5f;
        }
        if(g2.name == "Cylinder"){
            yy += 1f;
        }


        //GameObject.Find(g1.name).GetComponent<Transform>().position = new Vector3(xx, yy, zz);
        g1.transform.position = new Vector3(xx, yy, zz);
       /* g1.GetComponent<Rigidbody>().AddForce(Vector3.up * 50);
        g1.transform.position = Vector3.MoveTowards(g1.transform.position, new Vector3(xx, yy, zz), 100 * Time.deltaTime);*/
        
    }

    bool CheckArea(GameObject g1)
    {
        try{
            string areaName = g1.transform.parent.parent.parent.name;
            if(areaName != "PlayerArea" && areaName != "BenchArea"){
                return false;
            }else{
                return true;
            }
        }
        catch{
            Debug.Log("Catch Exception.");
            return false;
        }


    }

    void SwapPosition(GameObject g1, GameObject g2)
    {
        
        if(g1.tag == "Tile" || g2.tag == "Tile"  )                               //avoid bug to swap with tile 
        {
            return;
        }
        float xx11, zz11, xx22, zz22, yy11, yy22;



        xx11 = g1.GetComponent<Transform>().position.x;
        //yy = GameObject.Find("Shooter-W").GetComponent<Transform>().position.y;      //the position.y will never change
        zz11 = g1.GetComponent<Transform>().position.z;
        yy11 = g1.GetComponent<Transform>().position.y;

        xx22 = g2.GetComponent<Transform>().position.x;
        zz22 = g2.GetComponent<Transform>().position.z;
        yy22 = g2.GetComponent<Transform>().position.y;

        g1.transform.position = new Vector3(xx22, yy22, zz22);
        g2.transform.position = new Vector3(xx11, yy11, zz11);

    }
}