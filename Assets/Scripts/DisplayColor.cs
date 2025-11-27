using Mono.CompilerServices.SymbolWriter;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
public class DisplayColor : MonoBehaviourPunCallbacks
{
    public int[] buttonNumbers;
    public int[] viewID;
    public Color32[] colors;

    public GameObject namesObject;
    public GameObject waitForPlayers;

    public AudioClip[] gunShotSounds;
    public void Start()
    {
        namesObject = GameObject.Find("NameBG");
        waitForPlayers = GameObject.Find("WaitingBG");
        InvokeRepeating("CheckTime", 1, 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GetComponent<PhotonView>().IsMine && !waitForPlayers.activeInHierarchy)
            {
                RemoveData();
                RoomExit();
            }
        }
        if (this.GetComponent<Animator>().GetBool("Hit") == true)
        {
            StartCoroutine(Recover());
        }
    }

    void CheckTime()
    {
        if (namesObject.GetComponent<Timer>().timeStop == false)
        {
            this.gameObject.GetComponent<WeaponChange>().isDead = true;
            this.gameObject.GetComponent<PlayerMovement>().isDead = true;
            this.gameObject.GetComponent<PlayerMovement>().gameOver = true;
            this.gameObject.GetComponentInChildren<AimLootAtRef>().isDead = true;
            this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }
    public void Respawn(string name)
    {
        GetComponent<PhotonView>().RPC("ResetForReplay", RpcTarget.AllBuffered, name);
    }

    [PunRPC]
    void ResetForReplay(string name)
    {
        for (int i = 0; i< namesObject.GetComponent<NicknameScript>().names.Length; i++)
        {
            if (name == namesObject.GetComponent<NicknameScript>().names[i].text)
            {
                this.GetComponent<Animator>().SetBool("Dead", false);
                this.gameObject.GetComponent<WeaponChange>().isDead = false;
                this.gameObject.GetComponentInChildren<AimLootAtRef>().isDead = false;
                this.gameObject.layer = LayerMask.NameToLayer("Default");
                namesObject.GetComponent<NicknameScript>().healthBars[i].gameObject.GetComponent<Image>().fillAmount = 1;
            }
        }
    }
    public void DeliverDamage(string shooterName, string name, float damageAmt)
    {
        GetComponent<PhotonView>().RPC("GunDamage", RpcTarget.AllBuffered, shooterName, name,  damageAmt);
    }

    [PunRPC]
    void GunDamage(string shooterName, string name, float damageAmt)
    {
        for(int i = 0; i< namesObject.GetComponent<NicknameScript>().names.Length; i++)
        {
            if (name == namesObject.GetComponent<NicknameScript>().names[i].text)
            {
                if (namesObject.GetComponent<NicknameScript>()
                    .healthBars[i].gameObject.GetComponent<Image>().fillAmount > 0.1f)
                {
                    this.GetComponent<Animator>().SetBool("Hit", true);
                    namesObject.GetComponent<NicknameScript>().healthBars[i].gameObject.GetComponent<Image>().fillAmount -= damageAmt;
                }
                else
                {
                    namesObject.GetComponent<NicknameScript>().healthBars[i].gameObject.GetComponent<Image>().fillAmount = 0;
                    this.GetComponent<Animator>().SetBool("Dead", true);
                    this.gameObject.GetComponent<WeaponChange>().isDead = true;
                    this.gameObject.GetComponent<PlayerMovement>().isDead = true;
                    this.gameObject.GetComponentInChildren<AimLootAtRef>().isDead = true;
                    namesObject.GetComponent<NicknameScript>().RunMessage(shooterName, name);
                    this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                }
               

            }
        }
    }
    void RemoveData()
    {
        GetComponent<PhotonView>().RPC("RemoveMe", RpcTarget.AllBuffered);
    }

    void RoomExit()
    {
        StartCoroutine("GetReadyToLeave");
    }


    [PunRPC]
    void RemoveMe()
    {
        for(int i=0;i < namesObject.gameObject.GetComponent<NicknameScript>().names.Length;i++)
        {
            if (this.GetComponent<PhotonView>().Owner.NickName == namesObject.gameObject.GetComponent<NicknameScript>().names[i].text)
            {
                namesObject.GetComponent<NicknameScript>().names[i].gameObject.gameObject.SetActive(false);
                namesObject.GetComponent<NicknameScript>().healthBars[i].gameObject.gameObject.SetActive(false);
            }
        }
    }

    public void ChooseColor()
    {
        GetComponent<PhotonView>().RPC("AssignColor", RpcTarget.AllBuffered);
    }
    public void PlayGunShot(string name, int weaponNumber)
    {
        GetComponent<PhotonView>().RPC("PlayerSound", RpcTarget.All, name, weaponNumber);
    }

    [PunRPC]
    void PlayerSound(string name, int weaponNumber)
    {
        for(int i=0; i < namesObject.GetComponent<NicknameScript>().name.Length; i++)
        {
            if (name == namesObject.GetComponent<NicknameScript>().names[i].text)
            {
                GetComponent<AudioSource>().clip = gunShotSounds[i];
                GetComponent<AudioSource>().Play();

            }
        }
    }

    [PunRPC]
    void AssignColor()
    {
        for(int i = 0; i < buttonNumbers.Length; i++)
        {
            if (this.GetComponent<PhotonView>().ViewID == viewID[i])
            {
                this.transform.GetChild(1).GetComponent<Renderer>().material.color = colors[i];
                namesObject.GetComponent<NicknameScript>().names[i].gameObject.SetActive(true);
                namesObject.GetComponent<NicknameScript>().healthBars[i].gameObject.SetActive(true);
                namesObject.GetComponent<NicknameScript>().names[i].text = this.GetComponent<PhotonView>().Owner.NickName;
            }
        }
    }


    IEnumerator GetReadyToLeave()
    {
        yield return new WaitForSeconds(1);
        namesObject.GetComponent<NicknameScript>().Leaving();
        Cursor.visible = true;
        PhotonNetwork.LeaveRoom();
    }

    IEnumerator Recover()
    {
        yield return new WaitForSeconds(0.03f);
        this.GetComponent<Animator>().SetBool("Hit", false);
    }

}
