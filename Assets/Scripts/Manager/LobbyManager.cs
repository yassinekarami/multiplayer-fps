using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;

namespace Manager
{
    /// <summary>
    /// containts all the method to create /join a room 
    /// also launch the game
    /// </summary>
    public class LobbyManager : MonoBehaviourPunCallbacks
    {

        public void LaunchKillCountGameMode()
        {
            SceneManager.LoadScene("KillCount");
        
        }

        public void LaunchTeamBattleMode()
        {

        }

        public void LaunchNoRespawnMode()
        {

        }

   


    }

}
