using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class weaponPickups : MonoBehaviour
{

    private AudioSource audioPlayer;
    public float respawnTime = 5;
    public int weaponType = 1;
    public int ammoRefillAmt = 60;
    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.GetComponent<PhotonView>().RPC("PlayPickupAudio", RpcTarget.All);
            this.GetComponent<PhotonView>().RPC("TurnOff", RpcTarget.All);
            other.GetComponent<WeaponChange>().ammoAmounts[weaponType -1] += ammoRefillAmt;
            other.GetComponent<WeaponChange>().UpdatePickup();
        }
    }

    [PunRPC]
    void PlayPickupAudio()
    {
        audioPlayer.Play();
    }

    [PunRPC]
    void TurnOff()
    {
        if (weaponType == 1)
        {
            this.GetComponent<Renderer>().enabled = false;
            this.GetComponent<Collider>().enabled = false;
        } else
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.GetComponent<Collider>().enabled = false;
        }
     
        StartCoroutine(WaitToRespawn());
    }

    [PunRPC]
    void TurnOn()
    {
        if (weaponType == 1)
        {
            this.GetComponent<Renderer>().enabled = true;
            this.GetComponent<Collider>().enabled = true;
        }
        else
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
            this.GetComponent<Collider>().enabled = true;
        }
        StartCoroutine(WaitToRespawn());
    }

    IEnumerator WaitToRespawn()
    {
        yield return new WaitForSeconds(25);
        this.GetComponent<PhotonView>().RPC("TurnOn", RpcTarget.All);
    }
}

