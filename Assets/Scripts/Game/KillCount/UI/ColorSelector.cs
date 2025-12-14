using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Core.Event;
using Core.Utils;
namespace Game.KillCount.UI
{

    /// <summary>
    /// class for color selector panel
    /// it containts method for choosing color
    /// </summary>
    public class ColorSelector : MonoBehaviour
    {

        /// <summary>
        /// photonview instance attached to the colorSelectorPanel
        /// </summary>
        private PhotonView photonView;

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
        }

        /// <summary>
        /// update the color panel when a color is choosed
        /// than send an event to the gameManager to set the color / player
        /// </summary>
        /// <param name="color"></param>
        [PunRPC]
        void UpdateColorSelectorPanel(string color)
        {
            //Debug.Log(color + " has been choosed by the player "+ PhotonNetwork.LocalPlayer.NickName);
            //Debug.Log("PunRPC updateColorSelectorPanel " + color +" will be deleted");
            GameObject buttonToDisable = GameObject.Find(color);
            buttonToDisable.GetComponent<Button>().interactable = false;
            //Debug.Log("UpdateColorSelectorPanel " + color +" resolving "+ ColorUtils.ResolveColorFromString(color));
 
            CreatePlayerEvent.onColorChoosed?.Invoke(PhotonNetwork.LocalPlayer.NickName, ColorUtils.ResolveColorFromString(color));
        }
        /// <summary>
        /// red color button is clicked
        /// </summary>
        public void RedColorChoosed()
        {
            //Debug.Log("red color is choosed");
            photonView.RPC("UpdateColorSelectorPanel", RpcTarget.AllBuffered, "RedButton");
        }

        /// <summary>
        /// green color button is clicked
        /// </summary>
        public void GreenColorChoosed()
        {
            photonView.RPC("UpdateColorSelectorPanel", RpcTarget.AllBuffered, "GreenButton");
        }

        /// <summary>
        /// orange color button is clicked
        /// </summary>
        public void OrangeColorChoosed()
        {
            photonView.RPC("UpdateColorSelectorPanel", RpcTarget.AllBuffered, "OrangeButton");
        }

        /// <summary>
        /// purple color button is clicked
        /// </summary>
        public void PurpleColorChoosed()
        {
            photonView.RPC("UpdateColorSelectorPanel", RpcTarget.AllBuffered, "PurpleButton");
        }

        /// <summary>
        /// blue color is clicked
        /// </summary>
        public void BlueColorChoosed()
        {
            photonView.RPC("UpdateColorSelectorPanel", RpcTarget.AllBuffered, "BlueButton");
        }

        /// <summary>
        /// grey color is clicked
        /// </summary>
        public void GreyColorChoosed()
        {
            photonView.RPC("UpdateColorSelectorPanel", RpcTarget.AllBuffered, "GreyButton");
        }
    }

}
