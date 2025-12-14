using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace Manager
{
    public class LaucherManager : MonoBehaviourPunCallbacks
    {
        public InputField playerNickname;
        private string setName = "";

        public GameObject connecting;
        // Start is called before the first frame update
        void Start()
        {
            connecting.SetActive(false);

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateText()
        {
            setName = playerNickname.text;
            PhotonNetwork.LocalPlayer.NickName = setName;
        }
        public void EnterButton()
        {
            if (setName != "")
            {
                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.ConnectUsingSettings();
                connecting.SetActive(true);
            }
        }

        public void ExitButton()
        {
            Application.Quit();
        }
        public override void OnConnectedToMaster()
        {
            Debug.Log("connected to the master");
            SceneManager.LoadScene("Lobby");
        }

    }


}
