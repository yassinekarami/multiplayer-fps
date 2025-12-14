using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Core.Utils;

public class WaitingPanel : MonoBehaviour, IOnEventCallback
{

    [SerializeField]  Text waitingForPlayerText;
    string textTemplate = "Waiting for players to join : $ / %";


    [SerializeField]  GameObject waitingBgButtons;
   
    /// <summary>
    /// awake method
    /// </summary>
    void Awake()
    {
        waitingForPlayerText = GameObject.Find("Joined").GetComponent<Text>();
        waitingBgButtons = GameObject.Find("WaitingBgButtons");
        Debug.Log("WaitingBgButtons");
    }

    private void Start()
    {
        waitingBgButtons.SetActive(false);
    }

    /// <summary>
    /// update the text with the current player in the room each time a player join
    /// </summary>
    /// <param name="currentPlayerInRoom"></param>
    /// <param name="maxPlayerInRoom"></param>
    public void UpdateText(string currentPlayerInRoom, string maxPlayerInRoom)
    {
       // waitingForPlayerText.text = textTemplate.Replace("$", currentPlayerInRoom).Replace("%", maxPlayerInRoom);
        string newText = textTemplate.Replace("$", currentPlayerInRoom).Replace("%", maxPlayerInRoom);
        object[] content = new object[] { newText };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers  = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(Constant.PunEventCode.updateTextEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
    /// <summary>
    /// callBack to register event
    /// </summary>
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    /// <summary>
    /// callBack to remove event
    /// </summary>
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    /// <summary>
    /// photon event handler
    /// </summary>
    /// <param name="photonEvent"></param>
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == Constant.PunEventCode.updateTextEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            string newText = (string)data[0];
            waitingForPlayerText.text = newText;
        }
        else if (eventCode == Constant.PunEventCode.theGameIsReadyEventCode)
        {
            waitingBgButtons.SetActive(true);
        }
    }
}
