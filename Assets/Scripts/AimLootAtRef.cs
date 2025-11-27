using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations;

public class AimLootAtRef : MonoBehaviour
{
    private GameObject lookAtObject;
    public bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        lookAtObject = GameObject.Find("AimRef");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (this.gameObject.GetComponentInParent<PhotonView>().IsMine && isDead == false) 
        {
            this.transform.position = lookAtObject.transform.position;
        }
    }
}
