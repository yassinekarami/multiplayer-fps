using UnityEngine;
using Photon.Pun;
using Core.Model;
using Photon.Realtime;
using System.Collections.Generic;
using Core.Event;
using Core.Utils;
using ExitGames.Client.Photon;
using Core.Interface.PlayerInfoUI;
using Core.Interface.WeaponUI;


namespace Manager
{
    public class GameManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
    {
        public static List<IPlayerInfoObserver> playerInfoObservers = new List<IPlayerInfoObserver>();
        public static List<IPlayerInfoSubject> playerInfoSubjects = new List<IPlayerInfoSubject>();

        public static List<IWeaponUIObserver> weaponUIObservers = new List<IWeaponUIObserver>();
        public static List<IWeaponUISubject> weaponUISubject = new List<IWeaponUISubject>();

        [SerializeField] private int maxPlayersInRoom = 20;
        public List<Character> characters = new List<Character>();


        [SerializeField][Tooltip("out game panel")] GameObject colorSelectorPanel = null;
        [SerializeField][Tooltip("out game panel")] GameObject waitingPanel = null;


        [SerializeField][Tooltip("in game panel")] GameObject inGamePanel = null;
        [SerializeField] GameObject level = null;

        List<GameObject> spawnPoints = new List<GameObject>();
        [SerializeField] List<GameObject> weaponSpawnPoints = new List<GameObject>();

        List<string> availableWeapons = new List<string>();

        private ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
        private float timer = 20;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.AddCallbackTarget(this);
            colorSelectorPanel = GameObject.Find("ColorSelectorPanel");
            waitingPanel = GameObject.Find("WaitingBG");
            inGamePanel = GameObject.Find("InGamePanel");
            level = GameObject.Find("Level");

            spawnPoints.AddRange(GameObject.FindGameObjectsWithTag(Constant.Tag.SPAWN_POINT));
            weaponSpawnPoints.AddRange(GameObject.FindGameObjectsWithTag(Constant.Tag.WEAPON_SPAWN_POINT));
            availableWeapons.AddRange(new[] { "spawn/RL0N-25_low", "spawn/Sci-Fi Gun", "spawn/Bio Integrity Gun" });
        }

        

        private void Start()
        {

            customProperties["readyPlayers"] = 0;
          //level.SetActive(false);
            inGamePanel.SetActive(false);
            colorSelectorPanel.SetActive(true);
            waitingPanel.SetActive(false);

            PhotonNetwork.JoinRandomOrCreateRoom(null,roomOptions: setUpRoomOptions());
        }

        private void Update()
        {
            if(PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.CustomProperties != null &&
                (int)PhotonNetwork.CurrentRoom.CustomProperties["readyPlayers"] == maxPlayersInRoom)
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    Queue<GameObject> queue = new Queue<GameObject>(weaponSpawnPoints);
                    foreach (string weapon in availableWeapons)
                    {
                        GameObject pointToBeSpawnIn = queue.Dequeue();
                        if (pointToBeSpawnIn.transform.childCount == 0)
                        {
                            GameObject obj = PhotonNetwork.Instantiate(weapon, pointToBeSpawnIn.transform.position, Quaternion.identity, 0);
                            obj.transform.parent = pointToBeSpawnIn.transform;
                        }
                    }
                    timer = 20f;
                }
            }
            
        }

        /// <summary>
        /// when a player join the room he create his character
        /// </summary>
        public override void OnJoinedRoom()
        {
            Debug.Log("Properties = " + PhotonNetwork.CurrentRoom.MaxPlayers);
            // joined a room successfully
            Debug.Log("joined room" + PhotonNetwork.CurrentRoom + " current player " + PhotonNetwork.CurrentRoom.PlayerCount);
            CreatePlayerEvent.onColorChoosed += CreateCharacter;
      
            if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersInRoom)
            {
      
                Debug.Log("the game start , number of players " + PhotonNetwork.CurrentRoom.PlayerCount);

            }
        }

        /// <summary>
        /// send an event to display enter and exit button to enter or leave the arena
        /// </summary>
        private void SendTheGameIsReadyEvent()
        {

            object[] content = new object[] { true };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(Constant.PunEventCode.theGameIsReadyEventCode, content,  raiseEventOptions, SendOptions.SendReliable);
            Debug.Log("SendTheGameIsReadyEvent is send with the code " + Constant.PunEventCode.theGameIsReadyEventCode);
        }

        /// <summary>
        /// if the player cannot join a room , he create his own room
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="message"></param>
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("joining room has failed, creating a new room");
            PhotonNetwork.CreateRoom(UnityEngine.Random.Range(0, 1000).ToString(), this.setUpRoomOptions());
        }

        /// <summary>
        /// set up room options with custom properties and max player in the room
        /// </summary>
        /// <returns></returns>
        private RoomOptions setUpRoomOptions()
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayersInRoom;

            // initial room properties
            Hashtable props = new Hashtable();
            props["readyPlayers"] = 0;

            // tell PUN which properties we want to sync
            roomOptions.CustomRoomProperties = props;

            return roomOptions; 
        }
        /// <summary>
        /// when the room is created
        /// </summary>
        public override void OnCreatedRoom()
        {
            Debug.Log("OnCreatedRoom -> room exists now");

            Hashtable props = new Hashtable();
            props["readyPlayers"] = 0;
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);

            Debug.Log("Room properties set: " + PhotonNetwork.CurrentRoom.CustomProperties["readyPlayers"]);
        }

        /// <summary>
        /// create an instance of character
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="color"></param>
        private void CreateCharacter(string nickname, Color color)
        {
            this.characters.Add(new Character(nickname, color.ToString(), 100)); ;
            Debug.Log("create new character "+characters.Count);
            colorSelectorPanel.SetActive(false);
            waitingPanel.SetActive(true);
            waitingPanel.GetComponent<WaitingPanel>().UpdateText(PhotonNetwork.CountOfPlayers.ToString(), maxPlayersInRoom.ToString());
            UpdateRoomReadyPlayer();
        }

        /// <summary>
        /// function to increase the number of ready players and set it in the room custom properties
        /// </summary>
        private void UpdateRoomReadyPlayer()
        {

            Hashtable props = PhotonNetwork.CurrentRoom.CustomProperties;
            int currentReady = 0;
            if (props != null && props.ContainsKey("readyPlayers") )
            {
                currentReady = (int)props["readyPlayers"];
            }

            Hashtable updatedProps = new Hashtable();
            updatedProps["readyPlayers"] = currentReady + 1;

            PhotonNetwork.CurrentRoom.SetCustomProperties(updatedProps);
        }

        /// <summary>
        /// detect room properties updated
        /// </summary>
        /// <param name="propertiesThatChanged"></param>
        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {

            if (PhotonNetwork.CurrentRoom.CustomProperties != null && 
                (int)PhotonNetwork.CurrentRoom.CustomProperties["readyPlayers"] == maxPlayersInRoom)
            {
                Debug.Log("OnRoomPropertiesUpdate :readyPlayers " + (int)PhotonNetwork.CurrentRoom.CustomProperties["readyPlayers"]);

                SendTheGameIsReadyEvent();
            }
        }
        /// <summary>
        /// enable the level and instantiate the character through the network
        /// </summary>
        public void EnableLevel()
        {
            level.SetActive(true);
            waitingPanel.SetActive(false);

            foreach (Character character in characters)
            {
                object[] data = new object[]
                {
                    character.color.ToString(), character.nickname
                };
                Queue<GameObject> queue = new Queue<GameObject>(spawnPoints);

                GameObject obj = PhotonNetwork.Instantiate("Ybot", queue.Dequeue().transform.position, Quaternion.identity, 0, data);
            }
            inGamePanel.SetActive(true);
        }

        
    }
}

