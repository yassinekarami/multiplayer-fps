using Core.Utils;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Shared.Gameplay
{
    public class WeaponPickup : MonoBehaviour, IOnEventCallback
    {
        // Start is called before the first frame update
        void Start()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player picked up weapon!");
                // Add weapon to player's inventory logic here
                if(other.gameObject.GetComponent<WeaponHandler>() != null)
                {
                    other.gameObject.GetComponent<WeaponHandler>().increaseAmmo(gameObject.name);
                }
                // Destroy the pickup object
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                object[] content = new object[] { gameObject.name };
                PhotonNetwork.RaiseEvent(Constant.PunEventCode.weaponHasBeenPickupEventCode, content, raiseEventOptions, SendOptions.SendReliable);

            }
        }

        /// <summary>
        /// photon event handler
        /// </summary>
        /// <param name="photonEvent"></param>
        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;
            if (eventCode == Constant.PunEventCode.weaponHasBeenPickupEventCode)
            {
                StartCoroutine(destroyWeapon());

            }

        }

        // destroy the weapon object after a short delay
        IEnumerator destroyWeapon()
        {
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }
}
