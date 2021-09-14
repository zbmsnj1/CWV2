using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRunningTime : MonoBehaviour
{
    public Text runningTime;
    private float spendTime;
    private int hour;
    private int minute;
    private int second;

    // Start is called before the first frame update
    void Start()
    {
        //runningTime = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        spendTime += Time.deltaTime;

        hour = (int)spendTime / 3600;
        minute = (int)(spendTime - hour * 3600) / 60;
        second = (int)(spendTime - hour * 3600 - minute * 60);

        runningTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);

    }
}
