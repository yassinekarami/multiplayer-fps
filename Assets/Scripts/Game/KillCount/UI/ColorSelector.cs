using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Core.Event;
using Core.Utils;
using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Game.KillCount.UI
{

    /// <summary>
    /// class for color selector panel
    /// it containts method for choosing color
    /// </summary>
    public class ColorSelector : MonoBehaviour, IOnEventCallback
    {

        /// <summary>
        /// photonview instance attached to the colorSelectorPanel
        /// </summary>
        private PhotonView photonView;

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
        }

        /// <summary>
        /// red color button is clicked
        /// </summary>
        public void RedColorChoosed()
        {
            object[] content = new object[] { "RedButton" };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(Constant.PunEventCode.colorHasBeenChooseEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }

        /// <summary>
        /// green color button is clicked
        /// </summary>
        public void GreenColorChoosed()
        {
            object[] content = new object[] { "GreenButton" };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(Constant.PunEventCode.colorHasBeenChooseEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }

        /// <summary>
        /// orange color button is clicked
        /// </summary>
        public void OrangeColorChoosed()
        {
            object[] content = new object[] { "OrangeButton",  };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(Constant.PunEventCode.colorHasBeenChooseEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }

        /// <summary>
        /// purple color button is clicked
        /// </summary>
        public void PurpleColorChoosed()
        {
            object[] content = new object[] { "PurpleButton" };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(Constant.PunEventCode.colorHasBeenChooseEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }

        /// <summary>
        /// blue color is clicked
        /// </summary>
        public void BlueColorChoosed()
        {
            object[] content = new object[] { "BlueButton" };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(Constant.PunEventCode.colorHasBeenChooseEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }

        /// <summary>
        /// grey color is clicked
        /// </summary>
        public void GreyColorChoosed()
        {
            object[] content = new object[] { "GreyButton" };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(Constant.PunEventCode.colorHasBeenChooseEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }

        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;
         
            if (eventCode == Constant.PunEventCode.colorHasBeenChooseEventCode)
            {
                object[] data = (object[])photonEvent.CustomData;
                string button = (string)data[0];

                GameObject buttonGameObject = GameObject.Find(button);
                if (buttonGameObject != null && buttonGameObject.GetComponent<Button>())
                {
                    GameObject.Find(button).GetComponent<Button>().interactable = false;
                    CreatePlayerEvent.onColorChoosed?.Invoke(PhotonNetwork.LocalPlayer.NickName, ColorUtils.ResolveColorFromString(button));
                }
            }
        }
    }

}
