using Core.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Core.Utils;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Game.Shared.Gameplay;
using Core.Utils;
using Mono.Cecil.Cil;

namespace Game.Shared.UI
{
    public class PlayersInfoPanel : MonoBehaviour
    {
        public GameObject playerInfo;
        GameObject[] players;
        List<Character> playerCharater = new List<Character>();
        // Start is called before the first frame update
        void Start()
        {
            //characters = new List<Character>();
            //characters.Add(new Character("toto", Color.red, 100));
            //characters.Add(new Character("titi", Color.blue, 100));
            //InstiateInfoForeachPlayer(characters);


        }
        private void OnEnable()
        {
            players = GameObject.FindGameObjectsWithTag(Constant.Tag.PLAYER);
            List<object> content = new List<object>();
            foreach (GameObject player in players)
            {
                if (player != null && player.GetComponent<PlayerStates>())
                {
                    Debug.Log("OnEnable "+ player.GetComponent<PlayerStates>().character);
                    content.Add(player.GetComponent<PlayerStates>().character);
                }
            }

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(Constant.PunEventCode.setUpPlayerInfoPanelEventCode, content.ToArray(), raiseEventOptions, SendOptions.SendReliable);
        }
        // Update is called once per frame
        void Update()
        {

        }


        /// <summary>
        /// photon event handler
        /// </summary>
        /// <param name="photonEvent"></param>
        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;
            if (eventCode == Constant.PunEventCode.setUpPlayerInfoPanelEventCode)
            {
                object[] data = (object[])photonEvent.CustomData;
                Character[] c = (Character[])data;
                this.InstiateInfoForeachPlayer(c);
            }
            
        }


        /// <summary>
        /// instanatiate nickname and healthbar for each character present in the room
        /// </summary>
        /// <param name="characters"></param>
        public void InstiateInfoForeachPlayer(Character[] characters)
        {
            float yOffset = -30; // distance between each item
            int index = 0;

            foreach (Character character in characters)
            {
                // playerInfo.GetComponentInChildren<Text>().text = 
                GameObject toInstantiate = Instantiate(playerInfo, gameObject.transform); ;
                toInstantiate.GetComponentInChildren<Text>().text = character.nickname;
                toInstantiate.GetComponentInChildren<Image>().color = ColorUtils.ResolveColorFromString(character.color) ;


                // Apply vertical offset
                toInstantiate.GetComponent<RectTransform>().anchoredPosition +=
                    new Vector2(0, index * yOffset);

                index++;
            }
        }
    }
}

