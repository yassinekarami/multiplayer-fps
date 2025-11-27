using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class NicknameScript : MonoBehaviourPunCallbacks
{
    public Text[] names;
    public Image[] healthBars;
    private GameObject waitObject;
    private GameObject displayPanel;
    public Text messageText;
    public int[] kills;
    private void Start()
    {
        displayPanel.SetActive(false);
        for (int i = 0; i < names.Length; i++)
        {
            names[i].gameObject.SetActive(false);
            healthBars[i].gameObject.SetActive(false);
        }
        waitObject = GameObject.Find("WaitingBG");
    }

    public void Leaving()
    {
        StartCoroutine("BackToLobby");      
    }
    IEnumerator BackToLobby()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.LoadLevel("lobby");
    }

    public void ReturnToLobby()
    {
        waitObject.SetActive(false);
        RoomExit();
    }

    void RoomExit()
    {
        StartCoroutine(ToLobby());
    }

    public void RunMessage(string win, string lose)
    {
        this.GetComponent<PhotonView>().RPC("DisplayMessage",  RpcTarget.All, win, lose);
        UpdateKills(win);
    }

    void UpdateKills(string win)
    {
        for (int i = 0;i < names.Length;i++)
        {
            if (win == names[i].text)
            {
                kills[i]++;
            }
        }
    }

    [PunRPC]
    void DisplayMessage(string win, string lose)
    {
        displayPanel.SetActive(true);
        messageText.text = win + " killed "+lose;
        StartCoroutine(SwitchOffMessage());
    }

    [PunRPC]
    void MessageOff()
    {
        displayPanel.SetActive(false);
        messageText.text = "";
    }
    IEnumerator ToLobby()
    {
        yield return new WaitForSeconds(0.1f);
        Cursor.visible = true;
        PhotonNetwork.LeaveRoom();
    }

    IEnumerator SwitchOffMessage()
    {
        yield return new WaitForSeconds(3);
        this.GetComponent<PhotonView>().RPC("MessageOff", RpcTarget.All);

    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }

}
