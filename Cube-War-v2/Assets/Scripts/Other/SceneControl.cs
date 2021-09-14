using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
   
    void Start(){
        
       //SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
       SceneManager.LoadScene("GameBoard", LoadSceneMode.Additive);
       SceneManager.LoadScene("Models", LoadSceneMode.Additive);
    }


    

   
   
}