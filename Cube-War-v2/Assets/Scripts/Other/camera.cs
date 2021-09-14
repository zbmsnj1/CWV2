using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    


    void Update()
    {
        
        CameraFOV();
     }

    public void CameraFOV()
    {
        //mouse scrollwheel to control camera
        float wheel = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 100;

        //change camera position
        this.transform.Translate(Vector3.forward * wheel);
    }


   

}
