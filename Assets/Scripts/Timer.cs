using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class Timer : MonoBehaviour
{
    public Text minutesText;
    public Text secondsText;

    public int minutes = 4;
    public int seconds = 59;
    public GameObject canvas;
    public bool timeStop = false;

    public void BeginTimer()
    {
        GetComponent<PhotonView>().RPC("Count", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void Count()
    {
        BeginCounting();
    }

    void BeginCounting()
    {
        CancelInvoke();
        InvokeRepeating("TimeCountDown", 1, 1);
    }

    void TimeCoundDown()
    {
        if( seconds > 10)
        {
            seconds -= 1;
            secondsText.text = seconds.ToString();
        } 
        else if(seconds > 0 && seconds <11)
        {
            seconds -= 1;
            secondsText.text = "0"+ seconds.ToString();
        }
        else if (seconds ==  0 && minutes > 0)
        {
            secondsText.text = "0" + seconds.ToString();
            minutes -= 1;
            seconds = 59;
            minutesText.text = minutes.ToString();
            secondsText.text = seconds.ToString();
        }

        if (seconds == 0 && minutes <=0)
        {
            canvas.GetComponent<KillCount>().countDown = false;
            canvas.GetComponent<KillCount>().TimeOver();
            timeStop = true;
        }
    }
}
