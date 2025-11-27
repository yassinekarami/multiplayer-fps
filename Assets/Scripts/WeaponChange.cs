using Cinemachine;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
public class WeaponChange : MonoBehaviour
{
    public TwoBoneIKConstraint leftHand, rightHand;
    public RigBuilder rigBuilder;
    public Transform[] leftTargets;
    public Transform[] rightTargets;
    public GameObject[] weapons;

    private int weaponNumber = 0;
    private GameObject testForWeapons;

    private Image weaponIcon;
    private Text ammoAmountText;
    public Sprite[] weaponIcons;
    public int[] ammoAmounts;
    public GameObject[] muzzleFlash;
    private string shooterName;
    private string gotShotName;
    public float[] damageAmts;
    public bool isDead = false;
    private GameObject choosePanel;


    private CinemachineVirtualCamera cam;
    private GameObject camObject;

    public MultiAimConstraint[] aimTargets;
    private Transform aimTarget;

    // Start is called before the first frame update
    void Start()
    {
        choosePanel = GameObject.Find("ChoosePanel");
        weaponIcon = GameObject.Find("WeaponUI").GetComponent<Image>();
        ammoAmountText = GameObject.Find("AmmoAmount").GetComponent<Text>();

        camObject = GameObject.Find("PlayerCam");

        ammoAmounts[0] = 60;
        ammoAmounts[1] = 0;
        ammoAmounts[2] = 0;
        ammoAmountText.text = ammoAmounts[0].ToString();

        if (this.GetComponent<PhotonView>().IsMine)
        {
            cam = camObject.GetComponent<CinemachineVirtualCamera>();
            cam.LookAt = this.transform;
            cam.Follow = this.transform;

        } else
        {
            this.GetComponent<PlayerMovement>().enabled = false;
        }

        testForWeapons = GameObject.Find("Weapon1Pickup(clone)");
        if (testForWeapons == null)
        {
            if (this.gameObject.GetComponent<PhotonView>().Owner.IsMasterClient) 
            {
                var spawner = GameObject.Find("SpawnScript");
                spawner.GetComponent<SpawnCharacters>().SpawnWeaponsStart();
            }

        }
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isDead == false && choosePanel.activeInHierarchy == false)
        {
            if (GetComponent<PhotonView>().IsMine && ammoAmounts[weaponNumber] > 0)
            {
                ammoAmounts[weaponNumber]--;
                ammoAmountText.text = ammoAmounts[weaponNumber].ToString();
                GetComponent<DisplayColor>().PlayGunShot(GetComponent<PhotonView>().Owner.NickName, weaponNumber);
                this.GetComponent<PhotonView>().RPC("GunMuzzleFlash", RpcTarget.All);
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                if (Physics.Raycast(ray, out hit, 500))
                {
                    if (hit.transform.gameObject.GetComponent<PhotonView>() != null)
                    {
                        gotShotName = hit.transform.gameObject.GetComponent<PhotonView>().Owner.NickName;
                    }
                    if (hit.transform.gameObject.GetComponent<DisplayColor>() != null)
                    {
                        hit.transform.gameObject.GetComponent<DisplayColor>().DeliverDamage(
                            GetComponent<PhotonView>().Owner.NickName,
                            hit.transform.gameObject.GetComponent<PhotonView>().Owner.NickName,
                            damageAmts[weaponNumber]);
                    }
                    shooterName = GetComponent<PhotonView>().Owner.NickName;
                    Debug.Log(gotShotName + " got hit by " + shooterName);

                }
                this.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
        if (Input.GetMouseButtonDown(1) && GetComponent<PhotonView>().IsMine && isDead == false)
        {
            this.GetComponent<PhotonView>().RPC("Change", RpcTarget.AllBuffered);
            if (weaponNumber > weapons.Length - 1)
            {
                weaponIcon.GetComponent<Image>().sprite = weaponIcons[0];
                ammoAmountText.text = ammoAmounts[0].ToString();
                weaponNumber = 0;
            }

            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].SetActive(false);
            }
            weapons[weaponNumber].SetActive(true);
            weaponIcon.GetComponent<Image>().sprite = weaponIcons[weaponNumber];
            ammoAmountText.text = ammoAmounts[weaponNumber].ToString();
            leftHand.data.target = leftTargets[weaponNumber];
            rightHand.data.target = rightTargets[weaponNumber];
            // lefThumb.data.target = thumbTargets[weaponNumber];
            rigBuilder.Build();
        }
    }
    
    public void UpdatePickup()
    {
        ammoAmountText.text = ammoAmounts[weaponNumber].ToString();
    }
    [PunRPC]
    void GunMuzzleFlash() {
        muzzleFlash[weaponNumber].SetActive(true);
        StartCoroutine("MuzzleOff"); 
    }

    IEnumerator MuzzleOff()
    {
        yield return new WaitForSeconds(0.03f);
        this.GetComponent<PhotonView>().RPC("MuzzleFlashOff", RpcTarget.All);
   
    }

    [PunRPC]
    void MuzzleFlashOff()
    {
        muzzleFlash[weaponNumber].SetActive(false);
    }
    [PunRPC]
    void Change()
    {
        weaponNumber++;
        this.GetComponent<PhotonView>().RPC("Change", RpcTarget.AllBuffered);
        if (weaponNumber > weapons.Length - 1)
        {
            weaponNumber = 0;
        }
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
        weapons[weaponNumber].SetActive(true);
        leftHand.data.target = leftTargets[weaponNumber];
        rightHand.data.target = rightTargets[weaponNumber];
        rigBuilder.Build();
    }
}
