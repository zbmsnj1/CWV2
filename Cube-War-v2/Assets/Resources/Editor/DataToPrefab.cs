using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class DataToPrefab : Editor {

    [MenuItem("DataBuilder/Save Data To Prefab", false, 1)]
    static void Init(){
        SaveData();
    }

    static void SaveData(){
        GameObject dataObj = Resources.Load("_Data/ChessData", typeof(GameObject)) as GameObject;
        dataObj.GetComponent<ChessDataScript>().GetString();

        PropertyModification[] temp = new PropertyModification[1];
        temp[0] = new PropertyModification();
        temp[0].target = dataObj;
        PrefabUtility.SetPropertyModifications(((GameObject)dataObj), temp);        
    }
}

